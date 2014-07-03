
namespace nuPickers.Shared.DotNetDataSource
{
    using System;

    /// <summary>
    /// When this attribute is used, the pickers will render a corresponding config field
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DotNetDataSourceAttribute : Attribute
    {
        /// <summary>
        /// Field description rendered
        /// </summary>
        public string Description { get; set; }

        public DotNetDataSourceAttribute()
        {
        }
    }
}
