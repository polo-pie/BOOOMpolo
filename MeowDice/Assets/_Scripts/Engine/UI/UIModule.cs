using System;
using System.Collections.Generic;
using Engine.Runtime;
using Unity.Mathematics;
using UnityEngine;

namespace Engine.UI
{
    public class UIModule: UnitySingleton<UIModule>
    {

        [SerializeField] private Camera _uiCamera = null;

        private readonly List<UIWindow> _stack = new List<UIWindow>();

        public Transform UIRoot;

        public Camera UICamera => _uiCamera;

        public const int LayerDeep = 2000;
        public const int WindowDeep = 100;

        protected override void OnLoad()
        {
            UIRoot.gameObject.layer = LayerMask.NameToLayer("UI");
        }

        public bool HasWindow<T>() where T: UIWindow
        {
            return HasWindow(typeof(T));
        }

        public bool HasWindow(Type windowType)
        {
            foreach (var window in _stack)
            {
                if (window.GetType() == windowType)
                    return true;
            }

            return false;
        }

        public T GetWindow<T>() where T : UIWindow
        {
            var window = GetWindow(typeof(T));

            return window as T;
        }

        public UIWindow GetWindow(Type windowType)
        {
            foreach (var window in _stack)
            {
                if (window.GetType() == windowType)
                    return window;
            }

            return null;
        }

        public T ShowUI<T>(Dictionary<string, object> inputData = null) where T: UIWindow
        {
            return ShowUI(typeof(T), inputData) as T;
        }

        public UIWindow ShowUI(Type windowType, Dictionary<string, object> inputData = null)
        {
            var window = GetWindow(windowType);
            if (window != null)
            {
                window.SetVisible(true);
                if (inputData != null)
                {
                    window.RefreshUIElement(inputData);
                }
                return window;
            }

            window = Activator.CreateInstance(windowType) as UIWindow;
            if (window != null)
            {
                window.InternalCreate();
                // window.Go.transform.SetParent(UIRoot.transform, true);
                if (inputData != null)
                {
                    window.InitData(inputData);
                }
            }
            else
            {
                Debug.LogError($"[Engine][ShowUI] create window {windowType} fail");
            }
            
            
            return window;
        }

        public void CloseWindow(Type windowType)
        {
            var window = GetWindow(windowType);
            if (window == null)
                return;
            
        }

        private void Push(UIWindow window)
        {
            if (HasWindow(window.GetType()))
            {
                Debug.LogError($"window {window.GetType()} is exist.");
                return;
            }

            int insertIndex = -1;
            for (int i = 0; i < _stack.Count; i++)
            {
                if (window.WindowLayer == _stack[i].WindowLayer)
                    insertIndex = i + 1;
            }

            if (insertIndex == -1)
            {
                for (int i = 0; i < _stack.Count; i++)
                {
                    if (window.WindowLayer > _stack[i].WindowLayer)
                        insertIndex = i + 1;
                }
            }

            insertIndex = math.max(0, insertIndex);
            _stack.Insert(insertIndex, window);
        }

        private void Pop(UIWindow window)
        {
            _stack.Remove(window);
        }
    }
}