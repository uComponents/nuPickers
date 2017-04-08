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
        /// Title for the the config option 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description for the config option
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Initialize a new instance of <see cref="DotNetDataSourceAttribute"/>
        /// </summary>
        public DotNetDataSourceAttribute()
        {
        }
    }
}