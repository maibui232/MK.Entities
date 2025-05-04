namespace MK.Entities
{
    using System;
    using System.Collections.Generic;

    public sealed class World
    {
        private readonly Dictionary<Type, ISystem> typeToSystem = new();
        private readonly ISystemProvider           systemProvider;

        private World(ISystemProvider systemProvider)
        {
            this.systemProvider = systemProvider;
            this.EntityManager  = new EntityManager(new EntityFactory());
#if UNITY_EDITOR
            WorldDiagnostic.AddWorld(this);                  
#endif
        }

        public static World NewWorld(ISystemProvider systemProvider)
        {
            return new World(systemProvider);
        }

        public EntityManager EntityManager { get; }

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
            foreach (var (_, system) in this.typeToSystem)
            {
                system.OnCreate(this);
            }
        }

        public void IterateWorld()
        {
            this.EntityManager.PlaybackECB();
            foreach (var (_, system) in this.typeToSystem)
            {
                system.OnUpdate(this);
            }

            this.EntityManager.PlaybackECB();
        }

        public void DestroyWorld()
        {
            foreach (var (_, system) in this.typeToSystem)
            {
                system.OnCleanUp(this);
            }
#if UNITY_EDITOR
            WorldDiagnostic.RemoveWorld(this);                  
#endif
        }
    }
}