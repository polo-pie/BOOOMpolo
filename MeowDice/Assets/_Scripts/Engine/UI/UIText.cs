using UnityEngine;
using UnityEngine.UI;

namespace Engine.UI
{
    public class UIText: UIElement
    {
        private Text _text;

        protected override void OnCreate()
        {
            _text = GameObject.GetComponent<Text>();

            if (_text == null)
            {
                Debug.LogError($"[Engine][UIText][OnCreate] component Text not found, element {GameObject.name}");
            }
        }

        protected override void OnRefresh()
        {
            if (!inputData.TryGetValue("text", out object value))
            {
                Debug.LogError("[Engine][UIText][OnRefreshData] key text not found in inputData");
                return;
            }
            
            _text.text = value.ToString();
        }
    }
}