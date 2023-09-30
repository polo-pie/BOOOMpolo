using UnityEngine;
using UnityEngine.UI;

namespace Engine.UI
{
    
    public abstract class UIWindow: UIElement
    {
        public abstract WindowType WindowType { get; }
        
        public abstract string PrefabPath { get; }

        private Canvas _canvas;

        private GraphicRaycaster _raycaster;
        
        public int WindowLayer { private set; get; }

        private bool _closed = false;
        private bool _create = false;

        public int Depth
        {
            get
            {
                if (_canvas != null)
                    return _canvas.sortingOrder;
                return 0;
            }

            set
            {
                if(_canvas == null || _canvas.sortingOrder == value)
                    return;

                _canvas.sortingOrder = value;   
            }
        }
        
        public override void Close()
        {
            if (_closed)
                return;
            
            _closed = true;
            
            Destroy(true);
            UIModule.Instance.CloseWindow(GetType());
        }

        internal void InternalCreate()
        {
            if (!_create)
            {
                _create = true;
                _go = GameObject.Instantiate(Resources.Load<GameObject>(PrefabPath), UIModule.Instance.UIRoot);
                if (_go != null)
                {
                    OnCreate();
                }
                else
                {
                    Debug.LogError($"[UI][Window][InternalCreate] Create Window {GetType()} fail gameObject is null, path: {PrefabPath}");
                    Close();
                }
            }
        }
    }
}