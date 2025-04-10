namespace MK.Entities
{
    using System;

    public class DefaultSystemProvider : ISystemProvider
    {
        TSystem ISystemProvider.CreateSystem<TSystem>()
        {
            return Activator.CreateInstance<TSystem>();
        }
    }
}