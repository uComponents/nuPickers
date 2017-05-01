namespace nuPickers.Shared.EnumDataSource
{
    using DataSource;
    using nuPickers.Shared.Editor;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class EnumDataSource : IDataSource
    {
        public string AssemblyName { get; set; }

        public string EnumName { get; set; }

        public bool HandledTypeahead { get { return false; } }

        public IEnumerable<EditorDataItem> GetEditorDataItems(int currentId, int parentId, string typeahead) //TODO: change to explicit
        {
            return this.GetEditorDataItems();
        }

        public IEnumerable<EditorDataItem> GetEditorDataItems(int currentId, int parentId, string[] keys) //TODO: change to explicit
        {
            return this.GetEditorDataItems().Where(x => keys.Contains(x.Key));
        }

        IEnumerable<EditorDataItem> IDataSource.GetEditorDataItems(int currentId, int parentId, int skip, int take, out int count)
        {
            var editorDataItems = this.GetEditorDataItems();

            count = editorDataItems.Count();

            return editorDataItems.Skip(skip).Take(take);
        }

        [Obsolete("[v2.0.0]")]
        public  IEnumerable<EditorDataItem> GetEditorDataItems()
        {
            List<EditorDataItem> editorDataItems = new List<EditorDataItem>();

            Type enumType = Helper.GetAssembly(this.AssemblyName).GetType(this.EnumName);

            foreach(string enumItemName in Enum.GetNames(enumType))
            {
                FieldInfo fieldInfo = enumType.GetField(enumItemName);
                string key = enumItemName;
                string label = enumItemName;                

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
                                    //enabled = (bool)customAttributeNamedArguement.TypedValue.Value;
                                    break;
                            }
                        }
                    }
                }

                editorDataItems.Add(new EditorDataItem() { Key = key, Label = label });
            }

            return editorDataItems;
        }
    }
}