
namespace nuPickers.Shared.EnumDataSource
{
    using System;
    using System.Linq;

    /// <summary>
    /// Extension methods on Enum, used to get at configured EnumDataSource Attribute values
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the nuPicker Key that would be used for this enum item
        /// </summary>
        /// <param name="enumItem"></param>
        /// <returns>the nuPicker Key</returns>
        public static string GetKey(this Enum enumItem)
        {
            string key = enumItem.ToString();

            EnumDataSourceAttribute enumDataSourceAttribute = enumItem.GetType()
                                                                        .GetField(enumItem.ToString())
                                                                        .GetCustomAttributes(typeof(EnumDataSourceAttribute), false)
                                                                        .LastOrDefault() as EnumDataSourceAttribute;

            if (enumDataSourceAttribute != null && enumDataSourceAttribute.Key != null)
            {
                key = enumDataSourceAttribute.Key;
            }

            return key;
        }

        /// <summary>
        /// Gets the nuPicker Label that would be used for this enum item
        /// </summary>
        /// <param name="enumItem"></param>
        /// <returns></returns>
        public static string GetLabel(this Enum enumItem)
        {
            string label = enumItem.ToString();

            EnumDataSourceAttribute enumDataSourceAttribute = enumItem.GetType()
                                                                        .GetField(enumItem.ToString())
                                                                        .GetCustomAttributes(typeof(EnumDataSourceAttribute), false)
                                                                        .LastOrDefault() as EnumDataSourceAttribute;

            if (enumDataSourceAttribute != null && enumDataSourceAttribute.Label != null)
            {
                label = enumDataSourceAttribute.Label;
            }

            return label;
        }
    }
}
