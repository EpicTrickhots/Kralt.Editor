using UnityEngine;
using UnityEngine.InputSystem;

namespace Aaron.Toolkit.Graphics.UserInterface
{
    public class ScrollRect : InteractableContainer
    {
        [Header("Scroll Rect Settings")]
        [SerializeField] RectTransform m_Content;
        
        [SerializeField] float m_ScrollSpeed, m_ScrollDamping;
        
        float targetY;

        protected override void Update()
        {
            base.Update();
            
            if (MouseOver)
            {
                float scroll = Mouse.current.scroll.ReadValue().y;
                
                if (scroll > 0.0f)
                    Scroll(-1);
                else if (scroll < 0.0f)
                    Scroll(1);
            }
            
            UpdateDisplay();
        }
        
        private void UpdateDisplay()
        {
            Vector2 targetPosition = new(m_Content.anchoredPosition.x, targetY);
            m_Content.anchoredPosition = Vector3.Lerp(m_Content.anchoredPosition, targetPosition, m_ScrollDamping * Time.deltaTime);
        }
        
        private void Scroll(int direction)
        {
            float difference = m_Content.rect.height - rectTransform.rect.height;
            
            if(Mathf.Sign(difference) != -1f)
            {
                float scrollAmount = direction * m_ScrollSpeed;
                targetY = Mathf.Clamp(targetY + scrollAmount, 0f, difference);
            }
        }
    }
}
