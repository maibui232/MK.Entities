namespace MK.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class Collector : ICollector
    {
        protected Entity[] Entities { get; private set; }

        private readonly Type[] ComponentTypes;

        internal Collector()
        {
            this.ComponentTypes = this.GetType().GetGenericArguments();
        }

        void ICollector.Collect(IEnumerable<Entity> entities)
        {
            this.Entities = entities.Where(entity => this.ComponentTypes.All(entity.HasComponent)).ToArray();
            this.OnCollect();
        }

        protected abstract void OnCollect();
    }

    public class Collector<T> : Collector
        where T : IComponent
    {
        public T[] Components { get; private set; }

        protected override void OnCollect()
        {
            this.Components = this.Entities
               .Select(entity => entity.GetComponent<T>())
               .ToArray();
        }
    }

    public class Collector<T1, T2> : Collector
        where T1 : IComponent
        where T2 : IComponent
    {
        public (Entity, T1, T2)[] Components { get; private set; }

        protected override void OnCollect()
        {
            this.Components = this.Entities
               .Select(entity => (entity,
                                  entity.GetComponent<T1>(),
                                  entity.GetComponent<T2>()))
               .ToArray();
        }
    }

    public class Collector<T1, T2, T3> : Collector
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
        public (Entity, T1, T2, T3)[] Components { get; private set; }

        protected override void OnCollect()
        {
            this.Components = this.Entities
               .Select(entity => (entity,
                                  entity.GetComponent<T1>(),
                                  entity.GetComponent<T2>(),
                                  entity.GetComponent<T3>()))
               .ToArray();
        }
    }

    public class Collector<T1, T2, T3, T4> : Collector
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
    {
        public (Entity, T1, T2, T3, T4)[] Components { get; private set; }

        protected override void OnCollect()
        {
            this.Components = this.Entities
               .Select(entity => (entity,
                                  entity.GetComponent<T1>(),
                                  entity.GetComponent<T2>(),
                                  entity.GetComponent<T3>(),
                                  entity.GetComponent<T4>()))
               .ToArray();
        }
    }

    public class Collector<T1, T2, T3, T4, T5> : Collector
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
    {
        public (Entity, T1, T2, T3, T4, T5)[] Components { get; private set; }

        protected override void OnCollect()
        {
            this.Components = this.Entities
               .Select(entity => (entity,
                                  entity.GetComponent<T1>(),
                                  entity.GetComponent<T2>(),
                                  entity.GetComponent<T3>(),
                                  entity.GetComponent<T4>(),
                                  entity.GetComponent<T5>()))
               .ToArray();
        }
    }

    public class Collector<T1, T2, T3, T4, T5, T6> : Collector
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
    {
        public (Entity, T1, T2, T3, T4, T5, T6)[] Components { get; private set; }

        protected override void OnCollect()
        {
            this.Components = this.Entities
               .Select(entity => (entity,
                                  entity.GetComponent<T1>(),
                                  entity.GetComponent<T2>(),
                                  entity.GetComponent<T3>(),
                                  entity.GetComponent<T4>(),
                                  entity.GetComponent<T5>(),
                                  entity.GetComponent<T6>()))
               .ToArray();
        }
    }

    public class Collector<T1, T2, T3, T4, T5, T6, T7> : Collector
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
    {
        public (Entity, T1, T2, T3, T4, T5, T6, T7)[] Components { get; private set; }

        protected override void OnCollect()
        {
            this.Components = this.Entities
               .Select(entity => (entity,
                                  entity.GetComponent<T1>(),
                                  entity.GetComponent<T2>(),
                                  entity.GetComponent<T3>(),
                                  entity.GetComponent<T4>(),
                                  entity.GetComponent<T5>(),
                                  entity.GetComponent<T6>(),
                                  entity.GetComponent<T7>()))
               .ToArray();
        }
    }

    public class Collector<T1, T2, T3, T4, T5, T6, T7, T8> : Collector
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent
    {
        public (Entity, T1, T2, T3, T4, T5, T6, T7, T8)[] Components { get; private set; }

        protected override void OnCollect()
        {
            this.Components = this.Entities
               .Select(entity => (entity,
                                  entity.GetComponent<T1>(),
                                  entity.GetComponent<T2>(),
                                  entity.GetComponent<T3>(),
                                  entity.GetComponent<T4>(),
                                  entity.GetComponent<T5>(),
                                  entity.GetComponent<T6>(),
                                  entity.GetComponent<T7>(),
                                  entity.GetComponent<T8>()))
               .ToArray();
        }
    }

    public class Collector<T1, T2, T3, T4, T5, T6, T7, T8, T9> : Collector
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent
        where T9 : IComponent
    {
        public (Entity, T1, T2, T3, T4, T5, T6, T7, T8, T9)[] Components { get; private set; }

        protected override void OnCollect()
        {
            this.Components = this.Entities
               .Select(entity => (entity,
                                  entity.GetComponent<T1>(),
                                  entity.GetComponent<T2>(),
                                  entity.GetComponent<T3>(),
                                  entity.GetComponent<T4>(),
                                  entity.GetComponent<T5>(),
                                  entity.GetComponent<T6>(),
                                  entity.GetComponent<T7>(),
                                  entity.GetComponent<T8>(),
                                  entity.GetComponent<T9>()))
               .ToArray();
        }
    }

    public class Collector<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : Collector
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent
        where T9 : IComponent
        where T10 : IComponent
    {
        public (Entity, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)[] Components { get; private set; }

        protected override void OnCollect()
        {
            this.Components = this.Entities
               .Select(entity => (entity,
                                  entity.GetComponent<T1>(),
                                  entity.GetComponent<T2>(),
                                  entity.GetComponent<T3>(),
                                  entity.GetComponent<T4>(),
                                  entity.GetComponent<T5>(),
                                  entity.GetComponent<T6>(),
                                  entity.GetComponent<T7>(),
                                  entity.GetComponent<T8>(),
                                  entity.GetComponent<T9>(),
                                  entity.GetComponent<T10>()))
               .ToArray();
        }
    }
}