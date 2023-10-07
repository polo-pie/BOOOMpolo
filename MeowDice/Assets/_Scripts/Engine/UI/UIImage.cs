using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

namespace Engine.UI
{
    public class UIImage: UIElement
    {
        private Image _image;
        protected string _lastImagePath;

        protected override void OnCreate()
        {
            _image = Go.GetComponent<Image>();
            if (_image == null)
            {
                Debug.LogError($"[Engine][UIImage][OnCreate] component Image not found, element {Go.name}");
                return;
            }

            _lastImagePath = "";
        }

        protected override void OnRefresh()
        {
            if (!inputData.TryGetValue("path", out object value))
            {
                Debug.LogError("[Engine][UIImage][OnRefreshData] key path not found in inputData");
                return;
            }
            
            if (value is string path)
            {
                if (_lastImagePath != path && !string.IsNullOrEmpty(path))
                {
                    Debug.Log($"[Engine][UIImage][OnRefresh] load image {path}");
                    var texture2D = Resources.Load<Texture2D>(path);
                    Sprite sprite = Sprite.Create(texture2D, Rect.MinMaxRect(0, 0, texture2D.width, texture2D.height), Vector2.one / 2);
                    _image.sprite = sprite;
                    _lastImagePath = path;
                }
            }
            else
            {
                Debug.LogError("[Engine][UIImage][OnRefreshData] path data type is not string");
            }

            
        }
        
    }
}