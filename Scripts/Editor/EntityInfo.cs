namespace MK.Entities.Editor
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class EntityInfo : VisualElement, IDisposable
    {
        private readonly EntityDiagnostics entityDiagnostics;
        private readonly Dictionary<Type, VisualElement> componentToVisualElement = new();
        private readonly Foldout entityFoldout;
        private readonly VisualElement componentContainer;

        public EntityInfo(Entity entity)
        {
            this.entityDiagnostics = new EntityDiagnostics(entity);

            // Create entity header container
            var headerContainer = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    justifyContent = Justify.SpaceBetween,
                    alignItems = Align.Center
                }
            };

            // Create entity foldout
            this.entityFoldout = new Foldout
            {
                text = $"Entity: {entityDiagnostics.Name} (Index: {this.entityDiagnostics.Index}) - {this.entityDiagnostics.Entity.GetHashCode()}",
                value = true,
                style =
                {
                    marginBottom = 4,
                    paddingTop = 4,
                    paddingBottom = 4,
                    paddingLeft = 4,
                    paddingRight = 4,
                    backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.05f),
                    borderTopLeftRadius = 4,
                    borderTopRightRadius = 4,
                    borderBottomLeftRadius = 4,
                    borderBottomRightRadius = 4,
                    flexGrow = 1
                }
            };
            this.Add(headerContainer);
            headerContainer.Add(this.entityFoldout);

            // Style the foldout header
            this.entityFoldout.Q<Toggle>().Q<Label>().style.fontSize = 13;

            // Create component container
            this.componentContainer = new VisualElement
            {
                style =
                {
                    marginLeft = 8
                }
            };
            this.entityFoldout.Add(this.componentContainer);

            // Add component information
            this.UpdateComponentInfo();
        }

        public void UpdateComponentInfo()
        {
            // Remove components that no longer exist
            var componentsToRemove = new List<Type>();
            foreach (var (componentType, _) in this.componentToVisualElement)
            {
                if (!this.entityDiagnostics.HasComponent(componentType))
                {
                    componentsToRemove.Add(componentType);
                }
            }

            foreach (var componentType in componentsToRemove)
            {
                if (this.componentToVisualElement.Remove(componentType, out var visualElement))
                {
                    this.componentContainer.Remove(visualElement);
                }
            }

            // Add or update existing components
            foreach (var component in this.entityDiagnostics.Components)
            {
                var componentType = component.GetType();
                if (!this.componentToVisualElement.TryGetValue(componentType, out var componentInfo))
                {
                    // Create component header container
                    var componentHeader = new VisualElement
                    {
                        style =
                        {
                            flexDirection = FlexDirection.Row,
                            justifyContent = Justify.SpaceBetween,
                            alignItems = Align.Center
                        }
                    };

                    var componentFoldout = new Foldout
                    {
                        text = $"Component: {componentType.Name}",
                        value = true,
                        style =
                        {
                            marginTop = 4,
                            marginLeft = 8,
                            paddingTop = 4,
                            paddingBottom = 4,
                            paddingLeft = 4,
                            paddingRight = 4,
                            borderBottomWidth = 1,
                            borderBottomColor = new Color(0.2f, 0.2f, 0.2f),
                            flexGrow = 1
                        }
                    };
                    
                    // Style the component foldout header
                    componentFoldout.Q<Toggle>().Q<Label>().style.fontSize = 12;
                    componentFoldout.Q<Toggle>().Q<Label>().style.unityFontStyleAndWeight = FontStyle.Bold;

                    componentHeader.Add(componentFoldout);

                    this.componentToVisualElement.Add(componentType, componentFoldout);
                    this.componentContainer.Add(componentHeader);
                    componentInfo = componentFoldout;
                }

                // Update component properties
                var propertiesContainer = componentInfo.Q<VisualElement>("properties");
                if (propertiesContainer == null)
                {
                    propertiesContainer = new VisualElement 
                    {
                        name = "properties",
                        style =
                        {
                            marginLeft = 8
                        }
                    };
                    componentInfo.Add(propertiesContainer);
                }
                else
                {
                    propertiesContainer.Clear();
                }

                var properties = componentType.GetProperties();
                foreach (var property in properties)
                {
                    try
                    {
                        var value = property.GetValue(component);
                        var valueText = value?.ToString() ?? "null";
                        propertiesContainer.Add(new Label($"  {property.Name}: {valueText}")
                        {
                            style =
                            {
                                fontSize = 11,
                                marginBottom = 2
                            }
                        });
                    }
                    catch (Exception e)
                    {
                        propertiesContainer.Add(new Label($"  {property.Name}: Error - {e.Message}")
                        {
                            style =
                            {
                                fontSize = 11,
                                marginBottom = 2,
                                color = Color.red
                            }
                        });
                    }
                }
            }
        }

        public void Dispose()
        {
            this.componentContainer.Clear();
            this.componentToVisualElement.Clear();
        }
    }
}