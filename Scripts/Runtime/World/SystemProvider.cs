namespace MK.Entities
{
    using MK.DependencyInjection;

    internal sealed class SystemProvider : ISystemProvider
    {
        private readonly IResolver resolver;

        public SystemProvider(IResolver resolver)
        {
            this.resolver = resolver;
        }

        TSystem ISystemProvider.CreateSystem<TSystem>()
        {
            return this.resolver.Instantiate<TSystem>();
        }
    }
}