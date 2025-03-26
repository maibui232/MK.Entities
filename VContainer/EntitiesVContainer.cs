namespace MK.Entities.VContainer
{
    using global::VContainer;
    using MK.Extensions;

    public static class EntitiesVContainer
    {
        public static void RegisterEntities(this IContainerBuilder builder)
        {
            builder.Register<VContainerSystemProvider>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.NonLazy<VContainerSystemProvider>();
        }
    }
}