using System.Collections;
using System.Web.UI;
using Umbraco.Core.PropertyEditors;

namespace nuPickers.Shared.EnumDataSource
{
    using DataSource;
    using nuPickers.Shared.Editor;
    using Paging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class EnumDataSource : IDataSource
    {
        public string AssemblyName { get; set; }

        public string EnumName { get; set; }

        bool IDataSource.HandledTypeahead { get { return false; } }

        IEnumerable<DataEditor> IDataSource.GetEditorDataItems(int currentId, int parentId, string typeahead)
        {
            return this.GetEditorDataItems();
        }

        IEnumerable<DataEditor> IDataSource.GetEditorDataItems(int currentId, int parentId, string[] keys)
        {
            return this.GetEditorDataItems().Where(x => keys.Contains(x.Key));
        }

        IEnumerable<DataEditor> IDataSource.GetEditorDataItems(int currentId, int parentId, PageMarker pageMarker, out int total)
        {
            var editorDataItems = this.GetEditorDataItems();

            total = editorDataItems.Count();

            return editorDataItems.Skip(pageMarker.Skip).Take(pageMarker.Take);
        }

        private IEnumerable<DataEditor> GetEditorDataItems()
        {
            List<DataEditor> editorDataItems = new List<DataEditor>();

            Type enumType = Helper.GetAssembly(this.AssemblyName).GetType(this.EnumName);

            foreach(string enumItemName in Enum.GetNames(enumType))
            {
                FieldInfo fieldInfo = enumType.GetField(enumItemName);
                string key = enumItemName;
                string label = enumItemName;
	            bool enabled = true;

                foreach(CustomAttributeData customAttributeData in CustomAttributeData.GetCustomAttributes(fieldInfo))
                {
                    if (customAttributeData.Constructor.DeclaringType != null
                        && customAttributeData.Constructor.DeclaringType.Name == "EnumDataSourceAttribute"
                        && customAttributeData.NamedArguments != null)
                    {
                        foreach(CustomAttributeNamedArgument customAttributeNamedArguement in customAttributeData.NamedArguments)
                        {
                            switch(customAttributeNamedArguement.MemberInfo.Name)
                            {
                                case "Key":
                                    key = customAttributeNamedArguement.TypedValue.Value.ToString();
                                    break;

                                case "Label":
                                    label = customAttributeNamedArguement.TypedValue.Value.ToString();
                                    break;

                                case "Enabled":
                                    enabled = (bool)customAttributeNamedArguement.TypedValue.Value;
                                    break;
                            }
                        }
                    }
                }

                if (enabled)
                {
                    editorDataItems.Add(new DataEditor( { Key = key, Label = label });
                }
            }

            return editorDataItems;
        }

        public DataSourceView GetView(string viewName)
        {
            throw new NotImplementedException();
        }

        public ICollection GetViewNames()
        {
            throw new NotImplementedException();
        }

        public event EventHandler DataSourceChanged;
    }
}