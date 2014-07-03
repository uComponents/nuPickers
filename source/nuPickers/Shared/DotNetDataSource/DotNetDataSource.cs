
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

        public IEnumerable<DotNetDataSourceProperty> Properties { get; set; }

        public IEnumerable<EditorDataItem> GetEditorDataItems()
        {
            List<EditorDataItem> editorDataItems = new List<EditorDataItem>();

            object dotNetDataSource = Activator.CreateInstance(this.AssemblyName, this.ClassName).Unwrap();

            foreach (PropertyInfo propertyInfo in dotNetDataSource.GetType().GetProperties().Where(x => this.Properties.Select(y => y.Name).Contains(x.Name)))
            {
                if (propertyInfo.PropertyType == typeof(string))
                {
                    propertyInfo.SetValue(dotNetDataSource, this.Properties.Where(x => x.Name == propertyInfo.Name).Single().Value);
                }
                else
                {
                    // TODO: log unexpected property type
                }
            }

            return ((IDotNetDataSource)dotNetDataSource)
                        .GetEditorDataItems()
                        .Select(x => new EditorDataItem() { Key = x.Key, Label = x.Value });
        }
    }
}
