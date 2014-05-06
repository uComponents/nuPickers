namespace nuComponents.DataTypes.PropertyEditors.XmlDropDownList
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using System.Xml;
    using System.Xml.XPath;
    using umbraco;
    using umbraco.NodeFactory;
    using umbraco.presentation.templateControls;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;
    using LegacyMacroService = umbraco.cms.businesslogic.macro.Macro; // aliasing as Macro classes exist in two namespaces

    [PluginController("nuComponents")]
    public class XmlDropDownListApiController : UmbracoAuthorizedJsonController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">
        ///   expects the value from $scope.model.config (Umbraco automatically serializes the PreValueEditor fields into this)
        /// </param>
        /// <returns>
        ///  [{"key":"","markup":""}, ...]
        /// </returns>
        [HttpPost]
        public IEnumerable<object> GetEditorOptions([FromBody] dynamic config)
        {
            // convert dynamic to typed values
            string xmlSchema = config.xmlDataSource.xmlSchema;
            string optionsXPath = config.xmlDataSource.optionsXPath;
            string keyAttribute = config.xmlDataSource.keyAttribute;
            string labelAttribute = config.xmlDataSource.labelAttribute;
            //string labelMacro = config.xmlDataSource.labelMacro;
            //string cssFile = config.listPicker.cssFile;
            //string sriptFile = config.listPicker.scriptFile;
            //int listHeight = config.listPicker.listHeight;
            //int minItems = config.listPicker.minItems;
            //int maxItems = config.listPicker.maxItems;
            //bool allowDuplicates = config.listPicker.allowDuplicates;
            //bool hideUsed = config.listPicker.hideUsed;
            //bool enableFiltering = config.listPicker.enableFiltering;


            XmlDocument xmlDocument;
            List<object> editorOptions = new List<object>();

            switch (xmlSchema)
            {
                //TODO: handle caching (elsewhere) of source xml
                case "content":
                    xmlDocument = uQuery.GetPublishedXml(uQuery.UmbracoObjectType.Document);
                    break;

                case "media":
                    xmlDocument = uQuery.GetPublishedXml(uQuery.UmbracoObjectType.Media);
                    break;

                case "members":
                    xmlDocument = uQuery.GetPublishedXml(uQuery.UmbracoObjectType.Member);
                    break;

                default:
                    // fallback to expecting path to an xml file ?
                    xmlDocument = null;
                    break;
            }

            if (xmlDocument != null)
            {
                XPathNavigator xPathNavigator = xmlDocument.CreateNavigator();
                XPathNodeIterator xPathNodeIterator = xPathNavigator.Select(uQuery.ResolveXPath(optionsXPath));
                List<string> keys = new List<string>(); // used to keep track of keys, so that duplicates aren't added

                string key;
                string markup;

                while (xPathNodeIterator.MoveNext())
                {
                    // media xml is wrapped in a <Media id="-1" /> node to be valid, exclude this from any results
                    // member xml is wrapped in <Members id="-1" /> node to be valid, exclude this from any results
                    // TODO: nuQuery should append something unique to this root wrapper to simplify check here
                    if (xPathNodeIterator.CurrentPosition > 1 ||                        
                        !(xPathNodeIterator.Current.GetAttribute("id", string.Empty) == "-1" &&
                         (xPathNodeIterator.Current.Name == "Media" || xPathNodeIterator.Current.Name == "Members")))                       
                    {
                        // check for existance of a key attribute
                        key = xPathNodeIterator.Current.GetAttribute(keyAttribute, string.Empty);

                        // only add item if it has a unique key - failsafe
                        if (!string.IsNullOrWhiteSpace(key) && !keys.Any(x => x == key))
                        {
                            // TODO: ensure key doens't contain any commas (keys are converted saved as csv)
                            keys.Add(key); // add key so that it's not reused

                            // set default markup to use the configured label attribute
                            markup = xPathNodeIterator.Current.GetAttribute(labelAttribute, string.Empty);

                            editorOptions.Add(new
                            {
                                key = key,
                                markup = markup
                            });
                        }
                    }
                }
            }

            return editorOptions;
        }
    }
}
