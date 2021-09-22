﻿using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using nuPickers.Shared.DataSource;
using nuPickers.Shared.Editor;
using nuPickers.Shared.Paging;
using Umbraco.Core.Models;
using Umbraco.Web.Composing;

namespace nuPickers.Shared.XmlDataSource
{
    public class XmlDataSource : IDataSource
    {
        public string XmlData { get; set; }

        public string Url { get; set; }

        public string XPath { get; set; }

        public string KeyXPath { get; set; }

        public string LabelXPath { get; set; }

        bool IDataSource.HandledTypeahead { get { return false; } }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentId"></param>
        /// <param name="parentId"></param>
        /// <param name="typeahead">ignored</param>
        /// <returns></returns>
        IEnumerable<EditorDataItem> IDataSource.GetEditorDataItems(int currentId, int parentId, string typeahead)
        {
            return GetEditorDataItems(currentId, parentId);
        }

        IEnumerable<EditorDataItem> IDataSource.GetEditorDataItems(int currentId, int parentId, string[] keys)
        {
            return GetEditorDataItems(currentId, parentId).Where(x => keys.Contains(x.Key));
        }

        IEnumerable<EditorDataItem> IDataSource.GetEditorDataItems(int currentId, int parentId, PageMarker pageMarker, out int total)
        {
            var editorDataItems = GetEditorDataItems(currentId, parentId);

            total = editorDataItems.Count();

            return editorDataItems.Skip(pageMarker.Skip).Take(pageMarker.Take);
        }

        private IEnumerable<EditorDataItem> GetEditorDataItems(int currentId, int parentId)
        {
            XmlDocument xmlDocument;
            List<EditorDataItem> editorDataItems = new List<EditorDataItem>();

            switch (XmlData)
            {

                case "content":
                    xmlDocument = Current.UmbracoHelper.ContentAtXPath(UmbracoObjectTypes.Document.GetName());
                    break;

                case "media":
                    xmlDocument =Current.UmbracoHelper.ContentAtXPath(uQuery.UmbracoObjectType.Media);
                    break;

                case "members":
                    xmlDocument =Current.UmbracoHelper.ContentAtXPath();
                    break;

                case "url":
                    xmlDocument = new XmlDocument();

                    var url = Url.Replace("@contextId", currentId.ToString());

                    xmlDocument.LoadXml(Helper.GetDataFromUrl(url));
                    break;

                default:
                    xmlDocument = null;
                    break;
            }

            if (xmlDocument != null)
            {
                string xPath = XPath;

                if (xPath.Contains("$ancestorOrSelf"))
                {
                    // default to 'self-or-parent-or-root'
                    int ancestorOrSelfId = currentId > 0 ? currentId : parentId > 0 ? parentId : -1;

                    // if we have a content id, but it's not published in the xml
                    if (XmlData == "content" && ancestorOrSelfId > 0 && xmlDocument.SelectSingleNode("/descendant::*[@id='" + ancestorOrSelfId + "']") == null)
                    {
                        // use Umbraco API to get path of all ids above ancestorOrSelfId to root
                        Queue<int> path = new Queue<int>(Current.Services.ContentService.GetById(ancestorOrSelfId).Path.Split(',').Select(x => int.Parse(x)).Reverse().Skip(1));

                        // find the nearest id in the xml
                        do { ancestorOrSelfId = path.Dequeue(); }
                        while (xmlDocument.SelectSingleNode("/descendant::*[@id='" + ancestorOrSelfId + "']") == null);
                    }

                    xPath = XPath.Replace("$ancestorOrSelf", string.Concat("/descendant::*[@id='", ancestorOrSelfId, "']"));
                }

                XPathNavigator xPathNavigator = xmlDocument.CreateNavigator();
                XPathNodeIterator xPathNodeIterator = xPathNavigator.Select(xPath);
                List<string> keys = new List<string>(); // used to keep track of keys, so that duplicates aren't added

                string key;
                string label;

                while (xPathNodeIterator.MoveNext())
                {
                    // media xml is wrapped in a <Media id="-1" /> node to be valid, exclude this from any results
                    // member xml is wrapped in <Members id="-1" /> node to be valid, exclude this from any results
                    if (xPathNodeIterator.CurrentPosition > 1 ||
                        !(xPathNodeIterator.Current.GetAttribute("id", string.Empty) == "-1" &&
                         (xPathNodeIterator.Current.Name == "Media" || xPathNodeIterator.Current.Name == "Members")))
                    {
                        key = xPathNodeIterator.Current.SelectSingleNode(KeyXPath).Value;

                        // only add item if it has a unique key - failsafe
                        if (!string.IsNullOrWhiteSpace(key) && !keys.Any(x => x == key))
                        {
                            // TODO: ensure key doens't contain any commas (keys are converted saved as csv)
                            keys.Add(key); // add key so that it's not reused

                            // set default markup to use the configured label XPath
                            label = xPathNodeIterator.Current.SelectSingleNode(LabelXPath).Value;

                            editorDataItems.Add(new EditorDataItem
                            {
                                Key = key,
                                Label = label
                            });
                        }
                    }
                }
            }

            return editorDataItems;
        }
    }
}