using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sandbox.CustomEditor.Editor
{
    [CustomPropertyDrawer(typeof(SandboxProperty))]
    public class SandboxPropertyEditor : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var sandboxProperty = property.boxedValue as SandboxProperty;

            // Create a container for the field
            var container = new VisualElement();
            container.style.flexDirection = FlexDirection.Row;
            container.style.flexWrap = Wrap.Wrap;

            // Create a label and text field
            var label = new Label("This is a SandboxProperty:");
            label.style.fontSize = 18;

            var textField = new TextField() { value = sandboxProperty.someString };
            textField.RegisterValueChangedCallback(x =>
            {
                sandboxProperty.someString = x.newValue;
                property.boxedValue = sandboxProperty;

                property.serializedObject.ApplyModifiedProperties();
            });
            textField.style.minWidth = 200;

            var intField = new IntegerField() { value = sandboxProperty.someInt };
            intField.style.minWidth = 100;

            container.Add(label);
            container.Add(textField);
            container.Add(intField);

            return container;
        }
    }
}
