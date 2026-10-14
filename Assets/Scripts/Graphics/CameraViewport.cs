using UnityEngine;
using UnityEngine.EventSystems;

namespace Kralt.Editor.Graphics
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public class CameraViewport : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public static bool MouseOver { get; private set; } = false;
        
        RectTransform rectTransform;
        
        public void UpdateViewportRect()
        {
            if(rectTransform == null)
                rectTransform = GetComponent<RectTransform>();
            
            Vector3 localPosition = new(rectTransform.rect.x, rectTransform.rect.y);
            Vector3 worldPosition = transform.localToWorldMatrix.MultiplyPoint(localPosition);
            
            float xNorm = worldPosition.x / 1920f;
            float yNorm = worldPosition.y / 1080f;
            
            float widthNorm = rectTransform.rect.width / 1920f;
            float heightNorm = rectTransform.rect.height / 1080f;
            
            Rect rect = new(xNorm, yNorm, widthNorm, heightNorm);

            Camera.main.rect = rect;
        }
        
        public void OnPointerEnter(PointerEventData eventData) => MouseOver = true;
        public void OnPointerExit(PointerEventData eventData) => MouseOver = false;
    }
}
