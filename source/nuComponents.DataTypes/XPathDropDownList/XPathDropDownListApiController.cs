namespace nuComponents.DataTypes.XPathDropDownList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    [PluginController("nuComponents")]
    public class XPathDropDownListApiController : UmbracoAuthorizedJsonController
    {
        public IEnumerable<Tuple<string, int>> GetDropDownListOptions()
        {
            List<Tuple<string, int>> dropDownListOptions = new List<Tuple<string, int>>();

            dropDownListOptions.Add(Tuple.Create<string, int>("hello", 1));

            return dropDownListOptions;
        }
    }
}
