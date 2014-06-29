using System;

namespace nuPickers.Shared.EnumDataSource
{
    /// <summary>
    /// Attribute that can be applied to enum fields, to configure how the EnumDataSource generates it's data
    /// NOTE: this is virtually the same as Shared/Editor/EditorDataItem.cs
    /// </summary>
    public class EnumDataSourceAttribute : Attribute
    {
        /// <summary>
        /// Sets the unique identifier (the picker value) - else this defaults to the enum value
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Sets the visiable label for each item in the picker
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this enum field should be enabled
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDataSourceAttribute"/> class.
        /// </summary>
        public EnumDataSourceAttribute()
        {
            this.Enabled = true;
        }
    }
}
