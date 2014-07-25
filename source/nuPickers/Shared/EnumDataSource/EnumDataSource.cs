namespace nuPickers.Shared.EnumDataSource
{
    using nuPickers.Shared.Editor;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class EnumDataSource
    {
        public string AssemblyName { get; set; }

        public string EnumName { get; set; }

        public IEnumerable<EditorDataItem> GetEditorDataItems()
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
