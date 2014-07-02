
namespace nuPickers.Shared.DotNetDataSource
{
    using System;

    /// <summary>
    /// When this attribute is used, the pickers will render a corresponding config field
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class DotNetDataSourceAttribute : Attribute
    {
        public DotNetDataSourceAttribute()
        {
        }
    }
}
