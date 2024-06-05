using TMPro;
using UnityEngine;

namespace mandatory_features
{
    public class FPSDisplay : MonoBehaviour
    {
        public TextMeshProUGUI fpsText;

        private float _deltaTime;
        private bool _isFPSEnabled;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                ToggleFPS();
            }

            if (_isFPSEnabled)
            {
                _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
                float fps = 1.0f / _deltaTime;
                fpsText.text = string.Format("{0:0.} FPS", fps);
            }
            else
            {
                fpsText.text = "";
            }
        }

        private void ToggleFPS()
        {
            _isFPSEnabled = !_isFPSEnabled;
            fpsText.enabled = _isFPSEnabled;
        }
    }
}