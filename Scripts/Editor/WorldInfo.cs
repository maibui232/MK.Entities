namespace MK.Entities.Editor
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class WorldInfo : VisualElement, IDisposable
    {
        private readonly World world;
        private readonly Dictionary<Entity, EntityInfo> entityToInfo = new();
        private readonly VisualElement entityContainer;
        private readonly Foldout worldFoldout;

        public WorldInfo(World world)
        {
            this.world = world;
            
            // Create world foldout
            this.worldFoldout = new Foldout
            {
                text = $"World: {world.GetHashCode()}",
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
                    borderBottomRightRadius = 4
                }
            };
            this.Add(this.worldFoldout);

            // Style the foldout header
            this.worldFoldout.Q<Toggle>().Q<Label>().style.fontSize = 14;
            this.worldFoldout.Q<Toggle>().Q<Label>().style.unityFontStyleAndWeight = FontStyle.Bold;
            
            // Create entity container
            this.entityContainer = new VisualElement
            {
                style =
                {
                    marginLeft = 8
                }
            };
            this.worldFoldout.Add(this.entityContainer);
            
            // Add entity information
            this.Update();
        }

        public void Update()
        {
            if (WorldDiagnostics.Worlds == null)
            {
                return;
            }

            // Remove entities that no longer exist
            var entitiesToRemove = new List<Entity>();
            foreach (var (entity, _) in this.entityToInfo)
            {
                if (!WorldDiagnostics.EntityToDiagnostics.ContainsKey(entity))
                {
                    entitiesToRemove.Add(entity);
                }
            }

            foreach (var entity in entitiesToRemove)
            {
                if (this.entityToInfo.Remove(entity, out var entityInfo))
                {
                    entityInfo.Dispose();
                    this.entityContainer.Remove(entityInfo);
                }
            }

            // Add or update existing entities
            foreach (var entity in WorldDiagnostics.EntityToDiagnostics.Keys)
            {
                if (!this.entityToInfo.TryGetValue(entity, out var entityInfo))
                {
                    entityInfo = new EntityInfo(entity);
                    this.entityToInfo.Add(entity, entityInfo);
                    this.entityContainer.Add(entityInfo);
                }
                else
                {
                    entityInfo.UpdateComponentInfo();
                }
            }
        }

        public void Dispose()
        {
            foreach (var entityInfo in this.entityToInfo.Values)
            {
                entityInfo.Dispose();
            }
            this.entityToInfo.Clear();
            this.entityContainer.Clear();
        }
    }
} 