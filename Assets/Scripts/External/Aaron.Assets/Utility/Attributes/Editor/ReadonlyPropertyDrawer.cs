using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aaron.Toolkit.Utils
{
    [CustomPropertyDrawer(typeof(ReadonlyAttribute))]
    public class ReadonlyPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement container = new VisualElement();
            PropertyField propertyField = new PropertyField(property);
            
            container.SetEnabled(false);
            container.Add(propertyField);
            
            return container;
        }
    }
}

