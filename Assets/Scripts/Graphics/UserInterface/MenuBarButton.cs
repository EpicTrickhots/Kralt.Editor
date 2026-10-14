using Aaron.Toolkit.Graphics.UserInterface;
using UnityEngine;

namespace Kralt.Editor.Graphics.UserInterface
{
    public class MenuBarButton : InteractableContainer
    {
        [Header("MenuBarButton Settings")]
        [SerializeField] GameObject dropdown;
        [SerializeField] Color defaultColor, hoverColor, mouseDownColor;
        
        private bool dropdownIsOpen = false;
        
        public void ToggleDropdown()
        {
            dropdownIsOpen = !dropdownIsOpen;
            dropdown.SetActive(dropdownIsOpen);
        } 

        protected override void OnContainerClick()
        {
            SetContainerToDefaultState();
            OnContainerInteract();
            ToggleDropdown();
        }
        
        protected override void OnContainerInteract()
        {
            color = MouseDown ? mouseDownColor : (MouseOver ? hoverColor : defaultColor);
        }
    }
}
