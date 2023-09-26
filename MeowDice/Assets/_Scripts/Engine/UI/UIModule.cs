using System;
using System.Collections.Generic;
using Engine.Runtime;
using UnityEngine;

namespace Engine.UI
{
    public class UIModule: UnitySingleton<UIModule>
    {

        [SerializeField] private Camera _uiCamera = null;

        private readonly List<UIWindow> _stack = new List<UIWindow>();

        public Transform UIRoot => gameObject.transform;

        public Camera UICamera => _uiCamera;

        protected override void OnLoad()
        {
            gameObject.gameObject.layer = LayerMask.NameToLayer("UI");
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
    }
}