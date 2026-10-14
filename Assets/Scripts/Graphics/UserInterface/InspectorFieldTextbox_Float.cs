using Aaron.Toolkit.Graphics.UserInterface;
using UnityEngine;

namespace Kralt.Editor.Graphics.UserInterface
{
    /// <summary>
    /// Float input field for the kralt editor inspector
    /// </summary>
    public class InspectorFieldTextbox_Float : Textbox
    {
        [Header("Custom Settings")]
        [SerializeField] Color defaultColor;
        [SerializeField] Color hoverColor;
        [SerializeField] Color selectedColor;
        
        float m_FieldValue;
        
        public float FieldValue
        {
            get => m_FieldValue;
            set
            {
                m_FieldValue = value;
            }
        }
        
        protected override void OnReformatInputText()
        {
            if (!float.TryParse(Text, out _))
                Text = "0.0";
            else if (!Text.Contains('.'))
                Text += ".0";
        }
        
        protected override void OnContainerInteract()
        {
            color = Selected ? selectedColor : (MouseOver ? hoverColor : defaultColor);
        }
    }
}
