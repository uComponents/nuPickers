using nuPickers.Components;
using nuPickers.Shared.DataSource;
using nuPickers.Shared.EnumDataSource;
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
            composition.Register(typeof(DataSourceApiController), Lifetime.Request);
            composition.Register(typeof(EnumDataSourceApiController), Lifetime.Request);

        }
    }
}