using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Object = System.Object;

namespace Engine.UI
{
    // public abstract class UIElementData
    // {
    //     public bool visible;
    // }
    //
    public interface IUIElement
    {
        public GameObject Go { get; }
        public string Name { get; }
        public UIElement Parent { get; }
        public Dictionary<string, UIElement> ChildUIElements { get; }
        public bool Visible { get; }

        void InitData(Dictionary<string, object> data);
        
        void SetVisible(bool value);

        void Close();
    }
    
    public abstract class UIElement: IUIElement
    {
        private bool _destroyed = false;
        private int _index = 0;
        private string _bindValueKey = "";
        
        protected GameObject _go;
        public GameObject Go => _go;
        
        public RectTransform RectTransform => Go.transform as RectTransform;

        public string Name => _go.name;
        
        private UIElement _parent;
        public UIElement Parent => _parent;

        private Dictionary<string, UIElement> _childElements = new Dictionary<string, UIElement>();
        public Dictionary<string, UIElement> ChildUIElements => _childElements;

        private bool _visible;
        public bool Visible => _visible;

        protected Dictionary<string, object> inputData;
        protected Dictionary<string, object> outputData;

        public void SetVisible(bool value)
        {
            if (_go.activeSelf == value && _visible == value)
                return;
            _go.SetActive(value);
            _visible = value;
            if(value)
                OnVisible();
            else
                OnHidden();
        }

        #region Life Circle

        public virtual void Close()
        {
            Destroy();
        }

        protected void Destroy(bool destroyGo=false)
        {
            if(_destroyed)
                return;
            
            foreach (var childElement in _childElements.Values)
            {
                childElement.Destroy();
            }
            
            OnDestroy();
            _childElements.Clear();
            
            if(destroyGo && _go != null)
                UnityEngine.Object.Destroy(_go);

            _destroyed = true;
        }
    
        public void InitData(Dictionary<string, object> data)
        {
            inputData = data;
            RefreshData(data);
            // OnRefresh();

            // foreach (var childElement in _childElements.Values)
            // {
            //     childElement.InitData(outputData);
            // }
            BindProperty();

            OnInit();
        }

        protected virtual void OnCreate(){}
        
        protected virtual void OnInit(){}

        protected virtual void OnVisible(){}
        
        protected virtual void OnHidden(){}

        protected virtual void OnDestroy(){}

        protected virtual void OnRefreshData(){}
        
        /// <summary>
        /// Code generate by export script
        /// </summary>
        protected virtual void OnRefreshDataAuto(){}
        
        protected virtual void OnRefresh(){}
        
        protected virtual void BindProperty() {}
        
        #endregion

        #region Element Operate

        public T AddUIElement<T>(GameObject go, bool visible = true) where T: UIElement
        {
            var element = AddUIElement(typeof(T), go, visible);
            if (element == null)
                return null;
            return element as T;
        }

        public UIElement AddUIElement(Type elementType, GameObject go, bool visible = true)
        {
            UIElement element = Activator.CreateInstance(elementType) as UIElement;
            if (element == null)
            {
                Debug.LogError($"[Engine][UIElement][AddUIElement] create element fail, type {elementType}");
                return null;
            }

            if (go == null)
            {
                Debug.LogError($"[Engine][UIElement][AddUIElement] create element fail, gameObject is null, type {elementType}");
                return null;
            }
            
            if (ChildUIElements.ContainsKey(go.name))
            {
                Debug.LogWarning($"[Engine][UIElement][AddUIElement] element key {go.name} already exist, check is any chile game object with same name");
                go.name = go.name + $"_{_index++}";
            }
            ChildUIElements[go.name] = element;
            element._go = go;
            element._parent = this;
            
            // DoAddElement(go);
            
            element.OnCreate();
            
            element.SetVisible(true);
            
            return element;
        }

        /// <summary>
        /// Override by inherit class, to bind property or variable
        /// </summary>
        /// <param name="go"></param>
        protected virtual void DoAddElement(GameObject go){}

        public void RemoveElement(string uiElementName)
        {
            if (!ChildUIElements.TryGetValue(uiElementName, out var element))
            {
                Debug.LogWarning($"[Engine][UIElement][AddUIElement] {uiElementName} not exist in child, this element {_go.name}");
                return;
            }
            
            element.Destroy();
            ChildUIElements.Remove(uiElementName);
        }

        public void RefreshUIElement(Dictionary<string, object> data)
        {
            if (data == null)
            {
                Debug.LogError($"[Engine][UIElement][RefreshUIElement] data is null, element {Go.name}");
                return;
            }
            
            RefreshData(data);

            if (Visible)
            {
                OnRefresh();
            }
        }


        private void RefreshData(Dictionary<string, object> data)
        {
            inputData = data;
            OnRefreshDataAuto();
            OnRefreshData();
        }

        #endregion
        
    }
}