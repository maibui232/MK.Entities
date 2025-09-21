using UnityEditor;
using UnityEngine;
using MK.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace MK.Entities.Editor
{
    public class EntityInfoDrawer
    {
#region Fields

        private readonly Dictionary<object, bool> componentFoldouts = new();

#endregion

#region Public Methods

        public void DrawEntityComponents(Entity entity, VisualElement container)
        {
            // if (entity == null) return;// todo fix

            var components = EntityEditorUtility.GetComponents(entity).ToArray();
            if (!components.Any())
            {
                var noComponentsLabel = new Label("No components found on this entity");
                noComponentsLabel.AddToClassList("help-box");
                container.Add(noComponentsLabel);
                return;
            }

            var componentsLabel = new Label($"Components-{EntityEditorUtility.GetEntityName(entity)}");
            componentsLabel.AddToClassList("header");
            container.Add(componentsLabel);

            foreach (var component in components)
            {
                this.DrawComponent(component, container);
            }
        }

#endregion

#region Private Methods

        private void DrawComponent(object component, VisualElement container)
        {
            var componentName = EntityEditorUtility.GetComponentName(component);
            var foldout = new Foldout
            {
                text = componentName,
                value = this.componentFoldouts.GetValueOrDefault(component, true)
            };
            foldout.RegisterValueChangedCallback(evt => this.componentFoldouts[component] = evt.newValue);
            container.Add(foldout);

            if (foldout.value)
            {
                this.DrawComponentProperties(component, foldout.contentContainer);
                this.DrawComponentFields(component, foldout.contentContainer);
            }
        }

        private void DrawComponentFields(object component, VisualElement container)
        {
            var fields = EntityReflectionCache.GetCachedFields(component.GetType());
            if (!fields.Any()) return;

            var fieldsLabel = new Label("Fields");
            fieldsLabel.AddToClassList("sub-header");
            container.Add(fieldsLabel);

            foreach (var field in fields)
            {
                this.DrawField(component, field, container);
            }
        }

        private void DrawComponentProperties(object component, VisualElement container)
        {
            var properties = EntityReflectionCache.GetCachedProperties(component.GetType());
            if (!properties.Any()) return;

            var propertiesLabel = new Label("Properties");
            propertiesLabel.AddToClassList("sub-header");
            container.Add(propertiesLabel);

            foreach (var property in properties)
            {
                this.DrawProperty(component, property, container);
            }
        }

        private void DrawField(object component, FieldInfo field, VisualElement container)
        {
            var fieldContainer = new VisualElement();
            fieldContainer.AddToClassList("field-container");

            var label = new Label(field.Name);
            label.AddToClassList("field-label");
            fieldContainer.Add(label);

            try
            {
                var value = field.GetValue(component);
                var valueLabel = new Label(value?.ToString() ?? "null");
                valueLabel.AddToClassList("field-value");
                fieldContainer.Add(valueLabel);
            }
            catch
            {
                var errorLabel = new Label("Error accessing field");
                errorLabel.AddToClassList("error-label");
                fieldContainer.Add(errorLabel);
            }

            container.Add(fieldContainer);
        }

        private void DrawProperty(object component, PropertyInfo property, VisualElement container)
        {
            var propertyContainer = new VisualElement();
            propertyContainer.AddToClassList("property-container");

            var label = new Label(property.Name);
            label.AddToClassList("property-label");
            propertyContainer.Add(label);

            try
            {
                var value = property.GetValue(component);
                var valueLabel = new Label(value?.ToString() ?? "null");
                valueLabel.AddToClassList("property-value");
                propertyContainer.Add(valueLabel);
            }
            catch
            {
                var errorLabel = new Label("Error accessing property");
                errorLabel.AddToClassList("error-label");
                propertyContainer.Add(errorLabel);
            }

            container.Add(propertyContainer);
        }

#endregion
    }
}