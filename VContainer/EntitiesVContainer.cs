namespace MK.Entities.VContainer
{
    using global::MK.Extensions;
    using global::VContainer;

    public static class EntitiesVContainer
    {
        public static void RegisterEntities(this IContainerBuilder builder)
        {
            builder.Register<VContainerSystemProvider>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.NonLazy<VContainerSystemProvider>();
        }
    }
}