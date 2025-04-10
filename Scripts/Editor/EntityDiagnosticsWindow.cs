namespace MK.Entities.Editor
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class EntityDiagnosticsWindow : EditorWindow
    {
        private readonly Dictionary<World, WorldInfo> worldToInfo = new();
        private VisualElement worldContainer;
        private Button refreshButton;
        private bool isGUICreated;

        [MenuItem("Window/Entity Diagnostics")]
        public static void ShowWindow()
        {
            var window = GetWindow<EntityDiagnosticsWindow>();
            window.titleContent = new GUIContent("Entity Diagnostics");
            window.minSize = new Vector2(400, 600);
        }

        private void CreateGUI()
        {
            if (this.isGUICreated)
            {
                return;
            }

            var root = this.rootVisualElement;
            root.style.paddingTop = 8;
            root.style.paddingBottom = 8;
            root.style.paddingLeft = 8;
            root.style.paddingRight = 8;

            // Create header
            var header = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    justifyContent = Justify.SpaceBetween,
                    alignItems = Align.Center,
                    marginBottom = 8
                }
            };
            root.Add(header);

            header.Add(new Label("Entity Diagnostics")
            {
                style =
                {
                    fontSize = 16,
                    unityFontStyleAndWeight = FontStyle.Bold,
                    marginLeft = 4
                }
            });

            this.refreshButton = new Button(this.Reload) { text = "Refresh" };
            this.refreshButton.style.width = 80;
            this.refreshButton.style.height = 24;
            this.refreshButton.style.marginRight = 4;
            header.Add(this.refreshButton);

            // Create scroll view for worlds
            var worldScrollView = new ScrollView
            {
                style =
                {
                    flexGrow = 1
                }
            };
            root.Add(worldScrollView);

            // Add world container to scroll view
            this.worldContainer = new VisualElement();
            this.worldContainer.style.paddingTop = 8;
            this.worldContainer.style.paddingBottom = 8;
            this.worldContainer.style.paddingLeft = 8;
            this.worldContainer.style.paddingRight = 8;
            worldScrollView.Add(this.worldContainer);

            this.isGUICreated = true;
            this.Reload();
        }

        private void OnEnable()
        {
            this.isGUICreated = false;
            WorldDiagnostics.WorldAdded += this.OnWorldAdded;
            WorldDiagnostics.WorldRemoved += this.OnWorldRemoved;
        }

        private void OnDisable()
        {
            WorldDiagnostics.WorldAdded -= this.OnWorldAdded;
            WorldDiagnostics.WorldRemoved -= this.OnWorldRemoved;
            this.Clear();
            this.isGUICreated = false;
        }

        private void Update()
        {
            if (!this.isGUICreated)
            {
                return;
            }

            if (WorldDiagnostics.Worlds == null)
            {
                return;
            }

            foreach (var world in WorldDiagnostics.Worlds)
            {
                if (this.worldToInfo.TryGetValue(world, out var worldInfo))
                {
                    worldInfo.Update();
                }
            }
        }

        private void Reload()
        {
            if (!this.isGUICreated || this.worldContainer == null)
            {
                return;
            }

            this.Clear();

            if (WorldDiagnostics.Worlds == null)
            {
                this.worldContainer.Add(new Label("No active worlds found")
                {
                    style =
                    {
                        fontSize = 14,
                        unityTextAlign = TextAnchor.MiddleCenter,
                        marginTop = 20,
                        marginBottom = 20,
                        paddingTop = 10,
                        paddingBottom = 10,
                        paddingLeft = 10,
                        paddingRight = 10,
                        backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.1f),
                        borderTopLeftRadius = 4,
                        borderTopRightRadius = 4,
                        borderBottomLeftRadius = 4,
                        borderBottomRightRadius = 4
                    }
                });
                return;
            }

            foreach (var world in WorldDiagnostics.Worlds)
            {
                this.OnWorldAdded(world);
            }
        }

        private void OnWorldAdded(World world)
        {
            if (!this.isGUICreated || this.worldContainer == null)
            {
                return;
            }

            if (this.worldToInfo.ContainsKey(world))
            {
                return;
            }

            var worldInfo = new WorldInfo(world);
            this.worldToInfo.Add(world, worldInfo);
            this.worldContainer.Add(worldInfo);

            // Add separator after world info
            var separator = new VisualElement
            {
                style =
                {
                    height          = 2,
                    backgroundColor = new Color(0.3f, 0.3f, 0.3f, 0.3f),
                    marginTop       = 12,
                    marginBottom    = 12
                }
            };
            this.worldContainer.Add(separator);
        }

        private void OnWorldRemoved(World world)
        {
            if (!this.isGUICreated || this.worldContainer == null)
            {
                return;
            }

            if (this.worldToInfo.Remove(world, out var worldInfo))
            {
                worldInfo.Dispose();
                this.worldContainer.Remove(worldInfo);
                
                // Remove the separator after the world info if it exists
                var worldIndex = this.worldContainer.IndexOf(worldInfo);
                if (worldIndex >= 0 && worldIndex < this.worldContainer.childCount - 1)
                {
                    this.worldContainer.RemoveAt(worldIndex + 1);
                }
            }
        }

        private void Clear()
        {
            foreach (var worldInfo in this.worldToInfo.Values)
            {
                worldInfo.Dispose();
            }
            this.worldToInfo.Clear();

            if (this.worldContainer != null)
            {
                this.worldContainer.Clear();
            }
        }
    }
}