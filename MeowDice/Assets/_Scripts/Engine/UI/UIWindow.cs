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
            Destroy(true);
        }
    }
}