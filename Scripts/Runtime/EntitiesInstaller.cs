namespace MK.Entities
{
    using MK.DependencyInjection;

    public static class EntitiesInstaller
    {
        public static void InstallEntities(IBuilder builder)
        {
            builder.Register<SystemProvider>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        }
    }
}