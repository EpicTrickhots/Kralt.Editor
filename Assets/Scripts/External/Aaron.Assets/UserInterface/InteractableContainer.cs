using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Aaron.Toolkit.Graphics.UserInterface
{
    /// <summary>
    /// Container with virtual user-interaction methods.
    /// </summary>
    public abstract class InteractableContainer : Container, IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>
        /// If false, no interactable events will be invoked
        /// </summary>
        public bool isInteractable = true;
        
        /// <summary>
        /// If false, selection events will not be invoked
        /// </summary>
        public bool isSelectable = true;

        /// <summary>
        /// Returns true if the cursor is currently hovering over this <see cref="Container"/>
        /// </summary>
        protected bool MouseOver { get; private set; }

        /// <summary>
        /// Returns true if the left or right mouse button is pressed down while hovering over this <see cref="Container"/>
        /// </summary>
        protected bool MouseDown { get; private set; }

        /// <summary>
        /// Returns true if this <see cref="Container"/> is currently selected
        /// </summary>
        protected bool Selected { get; private set; }

        /// <summary>
        /// Invoked when the cursor starts hovering over this <see cref="Container"/>
        /// </summary>
        /// <param name="eventData"></param>
        protected virtual void OnContainerEnter(PointerEventData eventData) { }

        /// <summary>
        /// Invoked when the cursor stops hovering over this <see cref="Container"/>
        /// </summary>
        /// <param name="eventData"></param>
        protected virtual void OnContainerExit(PointerEventData eventData) { }

        /// <summary>
        /// Invoked when the cursor left/right clicks on this <see cref="Container"/>
        /// </summary>
        protected virtual void OnContainerClick() { }
        
        /// <summary>
        /// Invoked on the frame that the mouse is pressed down while hovering over this <see cref="Container"/>
        /// </summary>
        protected virtual void OnContainerMouseDown() { }

        /// <summary>
        /// Invoked on the frame that the mouse is released while hovering over this <see cref="Container"/>
        /// </summary>
        protected virtual void OnContainerMouseUp() { }
        
        /// <summary>
        /// Invoked when this <see cref="Container"/> starts being selected
        /// </summary>
        protected virtual void OnContainerSelect() {  }

        /// <summary>
        /// Invoked when this <see cref="Container"/> stops being selected
        /// </summary>
        protected virtual void OnContainerDeselect() {  }

        /// <summary>
        /// Invoked when this <see cref="Container"/> is hovered over, selected, deselected, or stops being hovered over
        /// </summary>
        protected virtual void OnContainerInteract() { }
        
        protected void SetContainerToDefaultState()
        {
            MouseOver = false;
            MouseDown = false;
            Selected = false;
        }
        
        /// <summary>
        /// Manually selects this <see cref="InteractableContainer"/>
        /// </summary>
        protected void SelectContainer()
        {
            if (!Selected)
            {
                Selected = true;
                OnContainerSelect();
                OnContainerInteract();
            }
        }

        /// <summary>
        /// Manually deselects this <see cref="InteractableContainer"/>
        /// </summary>
        protected void DeselectContainer()
        {
            if (Selected)
            {
                Selected = false;
                OnContainerDeselect();
                OnContainerInteract();
            }
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isInteractable)
            {
                MouseOver = true;
                OnContainerEnter(eventData);
                OnContainerInteract();
            }
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            if (isInteractable)
            {
                MouseOver = false;
                OnContainerExit(eventData);
                OnContainerInteract();
            }
        }
        
        // Clunky ahh code
        protected virtual void Update()
        {
            if (!isInteractable)
                return;

            if (Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame)
            {
                // Still handle MouseDown whether or not the container is selectable
                MouseDown = (isSelectable && MouseOver && !Selected) || (!isSelectable && MouseOver);
                OnContainerMouseDown();
                OnContainerInteract();

                if (!MouseOver && Selected && isSelectable)
                    DeselectContainer();
            }
            
            if (MouseDown && (Mouse.current.leftButton.wasReleasedThisFrame || Mouse.current.rightButton.wasReleasedThisFrame))
            {
                MouseDown = false;
                OnContainerMouseUp();
                OnContainerClick();
                OnContainerInteract();
                
                if (!Selected && isSelectable)
                    SelectContainer();
            }
        }
        
        public Vector2 RelativeCursorPosition()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Mouse.current.position.ReadValue(), null, out Vector2 point);
            return point;
        }
    }
}

