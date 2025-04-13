using UnityEditor;
using UnityEngine;
using MK.Entities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace MK.Entities.Editor
{
    public class EntitiesHierarchyWindow : EditorWindow
    {
#region Fields

        private readonly EntityInfoDrawer entityDrawer = new();
        private          Entity          selectedEntity;

        private ListView entityListView;
        private VisualElement componentsPanel;
        private Label noWorldsLabel;
        private Label noEntitySelectedLabel;
        private List<Entity> currentEntities = new();

#endregion

#region Public Methods

        [MenuItem("Window/MK Entities/Entities Hierarchy")]
        public static void ShowWindow() { GetWindow<EntitiesHierarchyWindow>("Entities Hierarchy"); }

#endregion

#region Unity Methods

        private void CreateGUI()
        {
            // Load and clone the UXML template
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Submodules/Entities/Scripts/Editor/EntitiesHierarchyWindow.uxml");
            visualTree.CloneTree(rootVisualElement);

            // Get references to UI elements
            this.entityListView = rootVisualElement.Q<ListView>("entityListView");
            this.componentsPanel = rootVisualElement.Q<VisualElement>("componentsPanel");
            this.noWorldsLabel = rootVisualElement.Q<Label>("noWorldsLabel");
            this.noEntitySelectedLabel = rootVisualElement.Q<Label>("noEntitySelectedLabel");

            // Setup entity list view
            this.SetupEntityListView();
            
            // Initial UI state
            this.UpdateUIState();
        }

        private void OnInspectorUpdate() { this.UpdateUIState(); }

#endregion

#region Private Methods

        private void SetupEntityListView()
        {
            this.entityListView.makeItem = () => new Button();
            this.entityListView.bindItem = (element, itemIndex) =>
            {
                var button = (Button)element;
                var entity = this.currentEntities[itemIndex];

                button.text    =  EntityEditorUtility.GetEntityName(entity);
                button.clicked += () => this.OnEntitySelected(entity);
                
                // Add hover effect
                button.RegisterCallback<MouseEnterEvent>(evt => button.AddToClassList("hover"));
                button.RegisterCallback<MouseLeaveEvent>(evt => button.RemoveFromClassList("hover"));
                
                // Update selection state
                if (this.selectedEntity == entity)
                {
                    button.AddToClassList("selected");
                    this.entityListView.selectedIndex = itemIndex;
                }
                else
                {
                    button.RemoveFromClassList("selected");
                }
            };
            
            this.entityListView.selectionType = SelectionType.Single;
            this.entityListView.itemsSource = this.currentEntities;
            
            // Add selection changed callback
            this.entityListView.selectionChanged += (items) =>
            {
                if (this.entityListView.selectedIndex >= 0)
                {
                    this.OnEntitySelected(this.currentEntities[this.entityListView.selectedIndex]);
                }
            };
        }

        private IEnumerable<Entity> GetAllEntities()
        {
            return WorldDiagnostic.Worlds.SelectMany(EntityEditorUtility.GetEntities);
        }

        private void UpdateUIState()
        {
            // Get current entities
            var newEntities = this.GetAllEntities().ToList();
            
            // Only update if entities have changed
            if (!this.currentEntities.SequenceEqual(newEntities))
            {
                this.currentEntities = newEntities;
                this.entityListView.itemsSource = this.currentEntities;
                this.entityListView.RefreshItems();
            }

            // Update visibility of no worlds message
            this.noWorldsLabel.style.display = this.currentEntities.Count == 0 ? DisplayStyle.Flex : DisplayStyle.None;
            this.entityListView.style.display = this.currentEntities.Count > 0 ? DisplayStyle.Flex : DisplayStyle.None;

            // Update components panel
            if (this.selectedEntity == null)
            {
                this.componentsPanel.Clear();
                this.componentsPanel.Add(this.noEntitySelectedLabel);
            }
            else
            {
                this.componentsPanel.Clear();
                var componentsContainer = new VisualElement();
                this.entityDrawer.DrawEntityComponents(this.selectedEntity, componentsContainer);
                this.componentsPanel.Add(componentsContainer);
            }
        }

        private void OnEntitySelected(Entity entity)
        {
            this.selectedEntity = entity;
            this.UpdateUIState();
        }

#endregion
    }
}