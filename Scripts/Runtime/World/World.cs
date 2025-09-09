namespace MK.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public sealed class World
    {
        private readonly Dictionary<Type, ISystem> typeToSystem = new();
        private readonly ISystemProvider           systemProvider;
        private readonly List<ISystem>             sortedSystems = new();
        private readonly Dictionary<UpdateOrder, List<ISystem>> updateOrderGroups = new();

        private World(ISystemProvider systemProvider, string worldName = "World", bool autoUpdate = true)
        {
            this.systemProvider = systemProvider;
            this.EntityManager  = new EntityManager(new EntityPoolableController());
            
            foreach (UpdateOrder order in Enum.GetValues(typeof(UpdateOrder)))
            {
                this.updateOrderGroups[order] = new List<ISystem>();
            }
            
            if (autoUpdate)
            {
                var go = new GameObject($"[{worldName}]");
                this.Runner = go.AddComponent<WorldRunner>();
                this.Runner.Initialize(this);
                Object.DontDestroyOnLoad(go);
            }
            
#if UNITY_EDITOR
            WorldDiagnostic.AddWorld(this);                  
#endif
        }

        public static World NewWorld(ISystemProvider systemProvider, string worldName = "World", bool autoUpdate = true)
        {
            return new World(systemProvider, worldName, autoUpdate);
        }

        public EntityManager EntityManager { get; }
        public IWorldRunner WorldRunner => this.Runner;
        public WorldRunner Runner { get; private set; }

        public void AddSystem<TSystem>() where TSystem : class, ISystem
        {
            var type = typeof(TSystem);
            if (this.typeToSystem.ContainsKey(type))
            {
                throw new InvalidOperationException($"There is already a system of type {type.FullName}");
            }

            var system = this.systemProvider.CreateSystem<TSystem>();
            this.typeToSystem.Add(type, system);
        }

        public void BuildWorld()
        {
            this.SortSystems();
            
            foreach (var system in this.sortedSystems)
            {
                system.OnCreate(this);
            }
            
            this.Runner?.NotifyWorldBuilt();
        }
        
        public void SetFixedUpdateRate(float rate)
        {
            this.Runner?.SetFixedUpdateRate(rate);
        }
        
        internal void BeginFrame()
        {
            this.EntityManager.PlaybackECB();
        }
        
        internal void EndFrame()
        {
            this.EntityManager.PlaybackECB();
        }
        
        internal void IterateWorld(UpdateOrder updateOrder)
        {
            if (this.updateOrderGroups.TryGetValue(updateOrder, out var systems))
            {
                foreach (var system in systems)
                {
                    system.OnUpdate(this);
                }
            }
        }

        public void DestroyWorld()
        {
            for (var i = this.sortedSystems.Count - 1; i >= 0; i--)
            {
                this.sortedSystems[i].OnCleanUp(this);
            }
            
            if (this.Runner != null)
            {
                GameObject.Destroy(this.Runner.gameObject);
            }
            
#if UNITY_EDITOR
            WorldDiagnostic.RemoveWorld(this);                  
#endif
        }
        
        private void SortSystems()
        {
            this.sortedSystems.Clear();
            foreach (var group in this.updateOrderGroups.Values)
            {
                group.Clear();
            }
            
            var systemInfos = new List<SystemInfo>();
            
            foreach (var (type, system) in this.typeToSystem)
            {
                var systemOrder = this.GetSystemOrder(type);
                var updateOrder = this.GetUpdateOrder(type);
                
                systemInfos.Add(new SystemInfo
                {
                    System = system,
                    Type = type,
                    Order = systemOrder,
                    UpdateOrder = updateOrder
                });
            }
            
            systemInfos.Sort((a, b) =>
            {
                var orderComparison = a.Order.CompareTo(b.Order);
                return orderComparison != 0 ? orderComparison : string.Compare(a.Type.Name, b.Type.Name, StringComparison.Ordinal);
            });
            
            foreach (var info in systemInfos)
            {
                this.sortedSystems.Add(info.System);
                this.updateOrderGroups[info.UpdateOrder].Add(info.System);
            }
        }
        
        private int GetSystemOrder(Type systemType)
        {
            var attribute = systemType.GetCustomAttribute<SystemOrderAttribute>();
            return attribute?.Order ?? 0;
        }
        
        private UpdateOrder GetUpdateOrder(Type systemType)
        {
            var attribute = systemType.GetCustomAttribute<UpdateOrderAttribute>();
            return attribute?.Order ?? UpdateOrder.Update;
        }
        
        private class SystemInfo
        {
            public ISystem System { get; set; }
            public Type Type { get; set; }
            public int Order { get; set; }
            public UpdateOrder UpdateOrder { get; set; }
        }
    }
}