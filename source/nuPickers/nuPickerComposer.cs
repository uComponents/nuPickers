using nuPickers.Components;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace nuPickers
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]

    public class nuPickerComposer :  ICoreComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register<CacheInvalidationComponent>(
                Lifetime.Singleton);
            composition.Register<EmbeddedResourceCompontent>(Lifetime.Singleton);

            composition.Register<RelationMappingComponent>(Lifetime.Singleton);

        }
    }
}