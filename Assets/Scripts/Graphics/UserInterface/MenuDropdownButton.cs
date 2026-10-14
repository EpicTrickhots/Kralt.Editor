using Aaron.Toolkit.Graphics.UserInterface;
using UnityEngine;
using UnityEngine.Events;

namespace Kralt.Editor.Graphics.UserInterface
{
    public class MenuDropdownButton : InteractableContainer
    {
        [SerializeField] Color defaultColor, hoverColor, mouseDownColor;
        
        [SerializeField] UnityEvent onClick;

        protected override void OnContainerClick()
        {
            onClick.Invoke();
        }
        
        protected override void OnContainerInteract()
        {
            color = MouseDown ? mouseDownColor : (MouseOver ? hoverColor : defaultColor);
        }
    }
}
