using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Kralt.Editor.InputHandling
{
    public static class KInput
    {
        #region Keybinds
        
        public const Key EDITOR_MOVE_FORWARD = Key.W;
        public const Key EDITOR_MOVE_BACKWARD = Key.S;

        public const Key EDITOR_MOVE_LEFT = Key.A;
        public const Key EDITOR_MOVE_RIGHT = Key.D;

        public const Key EDITOR_MOVE_UP = Key.Space;
        public const Key EDITOR_MOVE_DOWN = Key.LeftShift;
        
        #endregion

        public static bool KeyDown(Key key) => Keyboard.current[key].wasPressedThisFrame;

        public static bool KeyHeld(Key key) => Keyboard.current[key].isPressed;

        public static bool KeyUp(Key key) => Keyboard.current[key].wasReleasedThisFrame;

        public static bool LmbDown => Mouse.current.leftButton.wasPressedThisFrame;

        public static bool LmbHeld => Mouse.current.leftButton.isPressed;
        
        public static bool LmbUp => Mouse.current.leftButton.wasReleasedThisFrame;
        
        public static bool RmbDown => Mouse.current.rightButton.wasPressedThisFrame;
        
        public static bool RmbHeld => Mouse.current.rightButton.isPressed;
        
        public static bool RmbUp => Mouse.current.rightButton.wasReleasedThisFrame;
        
        public static Vector2 ScrollDelta => Mouse.current.scroll.ReadValue();
        
        public static Vector2 MouseDelta => Mouse.current.delta.ReadValue();
        
        public static Vector2 MousePosition => Mouse.current.position.ReadValue();
        
        public static Vector2 EditorMovementVector
        {
            get
            {
                float horizontal = 0f;
                float vertical = 0f;
                
                if (KeyHeld(EDITOR_MOVE_RIGHT)) horizontal += 1;
                if (KeyHeld(EDITOR_MOVE_LEFT)) horizontal -= 1;
                
                if (KeyHeld(EDITOR_MOVE_FORWARD)) vertical += 1;
                if (KeyHeld(EDITOR_MOVE_BACKWARD)) vertical -= 1;
                
                return new(horizontal, vertical);
            }
        }
        
        public static float VerticalMovement
        {
            get
            {
                float movement = 0f;
                
                if (KeyHeld(EDITOR_MOVE_UP)) movement += 1;
                if (KeyHeld(EDITOR_MOVE_DOWN)) movement -= 1;
                
                return movement;
            }
        }
    }
}

