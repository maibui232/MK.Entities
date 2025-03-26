namespace MK.Entities.VContainer
{
    using global::VContainer;
    using MK.Entities.Runtime;
    using MK.Extensions;

    public class VContainerSystemProvider : ISystemProvider
    {
        private readonly IObjectResolver objectResolver;

        public VContainerSystemProvider(IObjectResolver objectResolver)
        {
            this.objectResolver = objectResolver;
        }

        TSystem ISystemProvider.CreateSystem<TSystem>()
        {
            return this.objectResolver.InstantiateConcrete<TSystem>();
        }
    }
}