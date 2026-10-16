using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Kralt.Editor.InputHandling;

namespace Kralt.Editor.Graphics.UserInterface
{
    public class InspectorResizeRect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] 
        RectTransform inspectorRect, viewportRect;
        
        [SerializeField] 
        Color defaultColor, hoverColor;
        
        [SerializeField]
        float inspectorWidthMax, inpsectorWidthMin;
        
        [SerializeField] 
        CameraViewport viewport;
        
        private bool mouseOver = false;
        private bool isDragging = false;
        
        void Update()
        {
            if(mouseOver && KraltInput.LmbDown) isDragging = true;
            if(KraltInput.LmbUp) isDragging = false;

            if (isDragging)
            {
                float left = KraltInput.MousePosition.x / Screen.width * 1920f;
                left = Mathf.Clamp(left, inpsectorWidthMin, inspectorWidthMax);
                float right = Mathf.Abs(1920f - left);
                
                viewportRect.offsetMax = new Vector2(-right, viewportRect.offsetMax.y); // Right must be negative
                inspectorRect.offsetMin = new Vector2(left, inspectorRect.offsetMin.y);
                viewport.UpdateViewportRect();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            GetComponent<Image>().color = hoverColor;
            mouseOver = true;
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            GetComponent<Image>().color = defaultColor;
            mouseOver = false;
        }
    }
}
