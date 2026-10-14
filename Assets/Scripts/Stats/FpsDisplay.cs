using TMPro;
using UnityEngine;

namespace Kralt.Editor.Stats
{
    public class FpsDisplay : MonoBehaviour
    {
        [SerializeField] TMP_Text fpsText, msText;
        [SerializeField] Color highColor, lowColor;
        [SerializeField] float fpsThreshold = 60f;
        [SerializeField] float updateInterval = 1f;
        
        private float nextUpdate = 0f;
        
        void Update()
        {
            if(Time.time < nextUpdate) return;
            nextUpdate += updateInterval;

            float fps = 1f / Time.deltaTime;
            Color c = fps > fpsThreshold ? highColor : lowColor;
            fpsText.color = c;
            msText.color = c;
            fpsText.text = $"{fps:F2} FPS";
            msText.text = $"{Time.deltaTime * 1000f:F2} ms";
        }
    }
}
