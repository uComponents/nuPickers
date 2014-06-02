
namespace nuComponents.DataTypes.Shared.EnumDataSource
{
    using nuComponents.DataTypes.Shared.Editor;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web;
    using System.Web.Hosting;
    using umbraco;

    public class EnumDataSource
    {
        public string AssemblyName { get; set; }

        public string EnumName { get; set; }

        public IEnumerable<EditorDataItem> GetEditorDataItems()
        {
            List<EditorDataItem> editorDataItems = new List<EditorDataItem>();

            Type enumType = EnumDataSource.GetAssembly(this.AssemblyName).GetType(this.EnumName);

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


        /// <summary>
        /// Gets the <see cref="Assembly"/> with the specified name.
        /// </summary>
        /// <remarks>Works in Medium Trust.</remarks>
        /// <param name="assemblyName">The <see cref="Assembly"/> name.</param>
        /// <returns>The <see cref="Assembly"/>.</returns>
        internal static Assembly GetAssembly(string assemblyName)
        {
            AspNetHostingPermissionLevel appTrustLevel = GlobalSettings.ApplicationTrustLevel;
            if (appTrustLevel == AspNetHostingPermissionLevel.Unrestricted)
            {
                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += (sender, args) =>
                {
                    return Assembly.ReflectionOnlyLoad(args.Name);
                };
            }

            if (string.Equals(assemblyName, "App_Code", StringComparison.InvariantCultureIgnoreCase))
            {
                return Assembly.Load(assemblyName);
            }

            string assemblyFilePath = HostingEnvironment.MapPath(string.Concat("~/bin/", assemblyName));
            if (!string.IsNullOrEmpty(assemblyFilePath))
            {
                if (appTrustLevel == AspNetHostingPermissionLevel.Unrestricted)
                {
                    return Assembly.ReflectionOnlyLoadFrom(assemblyFilePath);
                }
                else
                {
                    // Medium Trust support
                    return Assembly.LoadFile(assemblyFilePath);
                }
            }

            return null;
        }

    }
}
