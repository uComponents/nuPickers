namespace nuComponents.DataTypes.XPathTemplatableList
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using System.Xml;
    using System.Xml.XPath;
    using umbraco;
    //using umbraco.cms.businesslogic.macro;
    using umbraco.presentation.templateControls;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    [PluginController("nuComponents")]
    public class XPathTemplatableListApiController : UmbracoAuthorizedJsonController
    {
        public IEnumerable<object> GetMacros()
        {
            //using legacy api as no method on Umbraco.Core.Services.MacroSerivce to get all macros
            return umbraco.cms.businesslogic.macro.Macro.GetAll().Select(x => new
                                                                            { 
                                                                                name = x.Name, 
                                                                                alias = x.Alias,
                                                                                valid = x.Properties.Any(y => y.Alias == "id") // TODO: check type ?
                                                                            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        ///     [{"path":""}, {"path":""}...]
        /// </returns>
        public IEnumerable<object> GetScriptFiles()
        {
            return this.Services
                        .FileService
                        .GetScripts()
                        .Where(x => x.Path.EndsWith(".js"))
                        .Select(x => new {
                            path = x.Path
                        });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">
        ///   expects the value from $scope.model.config (Umbraco automatically serializes the PreValueEditor fields into this)
        /// </param>
        /// <returns>
        ///  [{"key":"","markup":""},{"key":"","markup":""}...]
        /// </returns>
        [HttpPost]
        public IEnumerable<object> GetEditorOptions([FromBody] XPathTemplatableListPreValueEditor config)
        {
            XmlDocument xmlDocument;
            List<object> editorOptions = new List<object>();

            switch (config.XmlSchema)
            {
                //TODO: handle caching (elsewhere) of source xml
                case "content":
                    xmlDocument = uQuery.GetPublishedXml(uQuery.UmbracoObjectType.Document);
                    break;

                case "media":
                    xmlDocument = uQuery.GetPublishedXml(uQuery.UmbracoObjectType.Media);
                    //xmlDocument.FirstChild.Attributes.Append(xmlDocument.CreateAttribute("exclude"));
                    break;

                case "members":
                    xmlDocument = uQuery.GetPublishedXml(uQuery.UmbracoObjectType.Member);
                    //xmlDocument.FirstChild.Attributes.Append(xmlDocument.CreateAttribute("exclude"));
                    break;

                default:
                    // fallback to expecting path to an xml file ?
                    xmlDocument = null;
                    break;
            }

            if (xmlDocument != null)
            {
                XPathNavigator xPathNavigator = xmlDocument.CreateNavigator();
                XPathNodeIterator xPathNodeIterator = xPathNavigator.Select(uQuery.ResolveXPath(config.OptionsXPath));
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
                        key = xPathNodeIterator.Current.GetAttribute(config.KeyAttribute, string.Empty);

                        // only add item if it has a unique key - failsafe
                        if (!string.IsNullOrWhiteSpace(key) && !keys.Any(x => x == key))
                        {
                            // TODO: ensure key doens't contain any commas (keys are converted saved as csv)
                            keys.Add(key); // add key so that it's not reused

                            // set default markup to use the configured label attribute
                            markup = xPathNodeIterator.Current.GetAttribute(config.LabelAttribute, string.Empty);

                            //// if macro configured, replace the markup with it's output
                            //if (umbraco.cms.businesslogic.macro.Macro.GetByAlias(config.LabelMacro) != null)
                            //{
                            //    Macro macro = new Macro() { Alias = config.LabelMacro };
                            //    macro.MacroAttributes["key"] = key;
                            //    markup = macro.RenderToString();
                            //}

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

        ///// <param name="configuration">
        /////   expects the value from $scope.model.config.configruation
        ///// </param>
        //[HttpPost]
        //public IEnumerable<object> GetEditorOptions([FromBody] XPathTemplatableListConfiguration configuration)
        //{
        //}
    }
}
