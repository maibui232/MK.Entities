namespace MK.Entities
{
    using MK.Kernel;

    public static class EntitiesKernel
    {
        public static void EntitiesConfigure(this IBuilder builder)
        {
            builder.Add<SystemProvider>().AsImplementedInterface();
        }
    }
}