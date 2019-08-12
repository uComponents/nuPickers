using nuPickers.Components;
using nuPickers.EmbeddedResource;
using nuPickers.Shared.DataSource;
using nuPickers.Shared.EnumDataSource;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Web.Runtime;

namespace nuPickers
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    [ComposeAfter(typeof(WebFinalComposer))]
    public class nuPickerComposer :  IComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register(factory => new EmbeddedResourceController());
            composition.Components().Append<EmbeddedResourceCompontent>();
            composition.Register<CacheInvalidationComponent>(
                Lifetime.Singleton);

            composition.Register<RelationMappingComponent>(Lifetime.Singleton);
            composition.Register(typeof(DataSourceApiController), Lifetime.Request);
            composition.Register(typeof(EnumDataSourceApiController), Lifetime.Request);

        }
    }
}