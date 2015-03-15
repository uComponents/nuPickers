
namespace nuPickers.Shared.DotNetDataSource
{
    using nuPickers.Shared.Editor;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    public class DotNetDataSource
    {
        public string AssemblyName { get; set; }

        public string ClassName { get; set; }
     
        public IEnumerable<DotNetDataSourceProperty> Properties { get; set; }

        public string Typeahead { get; set; }

        [DefaultValue(false)]
        internal bool HandledTypeahead { get; set; }

        public IEnumerable<EditorDataItem> GetEditorDataItems(int contextId)
        {
            IEnumerable<EditorDataItem> editorDataItems = Enumerable.Empty<EditorDataItem>();

            object dotNetDataSource = AppDomain.CurrentDomain.CreateInstanceAndUnwrap(Helper.GetAssembly(this.AssemblyName).FullName, this.ClassName);

            if (dotNetDataSource != null)
            {
                if (dotNetDataSource is IDotNetDataSourceTypeahead)
                {
                    ((IDotNetDataSourceTypeahead)dotNetDataSource).Typeahead = this.Typeahead;
                    this.HandledTypeahead = true;
                }

                foreach (PropertyInfo propertyInfo in dotNetDataSource.GetType().GetProperties().Where(x => this.Properties.Select(y => y.Name).Contains(x.Name)))
                {
                    if (propertyInfo.PropertyType == typeof(string))
                    {
                        string propertyValue = this.Properties.Where(x => x.Name == propertyInfo.Name).Single().Value;

                        if (propertyValue != null)
                        {
                            // process any tokens                        
                            propertyValue = propertyValue.Replace("$(ContextId)", contextId.ToString());

                            propertyInfo.SetValue(dotNetDataSource, propertyValue);
                        }
                    }
                    else
                    {
                        // TODO: log unexpected property type
                    }
                }

                editorDataItems = ((IDotNetDataSource)dotNetDataSource)
                                            .GetEditorDataItems(contextId)
                                            .Select(x => new EditorDataItem() { Key = x.Key, Label = x.Value });
            }

            return editorDataItems;
        }
    }
}
