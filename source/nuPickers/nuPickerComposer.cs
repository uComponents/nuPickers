using nuPickers.Compontents;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace nuPickers
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]

    public class nuPickerComposer : ComponentComposer<nuPickerCompontent>, ICoreComposer
    {

    }
}