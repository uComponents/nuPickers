
namespace nuComponents.DataTypes.Shared.SaveFormat
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    internal static class SaveFormat
    {
        internal static IEnumerable<string> GetSavedKeys(string savedValue)
        {
            if (!string.IsNullOrWhiteSpace(savedValue))
            {
                switch (savedValue[0])
                {
                    //case '[':
                    //    break;

                    case '<':
                        // TODO: check xml is valid
                        return XDocument.Parse(savedValue).Descendants("PickedOption").Select(x => x.Attribute("Key").Value);

                    default: // csv
                        return savedValue.Split(',');                          
                }
            }

            return Enumerable.Empty<string>();
        }
    }
}
