namespace MK.Entities.VContainer
{
    using global::MK.Extensions;
    using global::VContainer;

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