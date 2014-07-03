
namespace nuPickers.Shared.DotNetDataSource
{
    using nuPickers.Shared.Editor;
    using System;
    using System.Reflection;
    using System.Collections.Generic;
    using System.Linq;

    public class DotNetDataSource
    {
        public string AssemblyName { get; set; }

        public string ClassName { get; set; }

        public IEnumerable<object> Properties { get; set; }

        public IEnumerable<EditorDataItem> GetEditorDataItems()
        {
            List<EditorDataItem> editorDataItems = new List<EditorDataItem>();

            object dotNetDataSource = Activator.CreateInstance(this.AssemblyName, this.ClassName).Unwrap();
                        
            // all properties with the DotNetDataSourceAttribute
            foreach(PropertyInfo propertyInfo in dotNetDataSource.GetType().GetProperties().Where(x => x.GetCustomAttributes(typeof(DotNetDataSourceAttribute), false).Any()))
            {                
                //propertyInfo.SetValue(dotNetDataSource, "");
            }


            return ((IDotNetDataSource)dotNetDataSource)
                        .GetEditorDataItems()
                        .Select(x => new EditorDataItem() { Key = x.Key, Label = x.Value });
        }
    }
}
