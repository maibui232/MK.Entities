namespace MK.Entities
{
    using UnityEngine;

    public sealed class WorldRunner : MonoBehaviour, IWorldRunner
    {
        private World world;
        private bool  isBuilt;
        private bool  isPaused;
        
        private float fixedUpdateTimer;
        private float fixedUpdateRate = 50f;
        private float FixedDeltaTime => 1f / fixedUpdateRate;

        internal void Initialize(World targetWorld)
        {
            this.world = targetWorld;
        }

        internal void NotifyWorldBuilt()
        {
            this.isBuilt = true;
        }
        
        public void SetFixedUpdateRate(float rate)
        {
            this.fixedUpdateRate = Mathf.Max(1f, rate);
        }

        void IWorldRunner.Pause()
        {
            this.isPaused = true;
        }

        void IWorldRunner.Resume()
        {
            this.isPaused = false;
        }

        private void Update()
        {
            if (!this.isBuilt || this.isPaused) return;
            
            this.world.BeginFrame();
            this.world.IterateWorld(UpdateOrder.PreUpdate);
            this.world.IterateWorld(UpdateOrder.Update);
            
            this.fixedUpdateTimer += Time.deltaTime;
            while (this.fixedUpdateTimer >= this.FixedDeltaTime)
            {
                this.world.IterateWorld(UpdateOrder.FixedUpdate);
                this.fixedUpdateTimer -= this.FixedDeltaTime;
            }
        }

        private void LateUpdate()
        {
            if (!this.isBuilt || this.isPaused) return;
            this.world.IterateWorld(UpdateOrder.LateUpdate);
            this.world.EndFrame();
        }

        private void OnDestroy()
        {
            this.world?.DestroyWorld();
        }
    }
}