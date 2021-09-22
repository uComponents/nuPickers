namespace nuPickers.Shared.DotNetDataSource
{
    using DataSource;
    using nuPickers.Shared.Editor;
    using Paging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Umbraco.Core.Logging;

    public class DotNetDataSource : IDataSource
    {
        private readonly IProfilingLogger _logger;
        public DotNetDataSource(  IProfilingLogger profilingLogger)
        {
            _logger = profilingLogger;
        }
        private bool handledTypeahead = false;

        public string AssemblyName { get; set; }

        public string ClassName { get; set; }

        public IEnumerable<DotNetDataSourceProperty> Properties { get; set; }

        bool IDataSource.HandledTypeahead { get { return this.handledTypeahead; } }

        IEnumerable<EditorDataItem> IDataSource.GetEditorDataItems(int currentId, int parentId, string typeahead)
        {
            var contextId = currentId == 0 ? parentId : currentId; // HACK: workarround to avoid breaking the IDotNetDataSource interface

            var editorDataItems = Enumerable.Empty<EditorDataItem>();

            IDotNetDataSource dotNetDataSource = AppDomain.CurrentDomain.CreateInstanceAndUnwrap(Helper.GetAssembly(this.AssemblyName).FullName, this.ClassName) as IDotNetDataSource;

            if (dotNetDataSource != null)
            {
                if (dotNetDataSource is IDotNetDataSourceTypeahead)
                {
                    ((IDotNetDataSourceTypeahead)dotNetDataSource).Typeahead = typeahead;
                    this.handledTypeahead = true;
                }

                this.SetProperties(ref dotNetDataSource, contextId);

                editorDataItems = dotNetDataSource
                                    .GetEditorDataItems(contextId)
                                    .Select(x => new EditorDataItem() { Key = x.Key, Label = x.Value })
                                    .ToList();
            }

            return editorDataItems;
        }

        IEnumerable<EditorDataItem> IDataSource.GetEditorDataItems(int currentId, int parentId, string[] keys)
        {
            var contextId = currentId == 0 ? parentId : currentId; // HACK: workarround to avoid breaking the IDotNetDataSource interface

            var editorDataItems = Enumerable.Empty<EditorDataItem>();

            IDotNetDataSource dotNetDataSource = AppDomain.CurrentDomain.CreateInstanceAndUnwrap(Helper.GetAssembly(this.AssemblyName).FullName, this.ClassName) as IDotNetDataSource;

            if (dotNetDataSource != null)
            {
                if (dotNetDataSource is IDotNetDataSourceKeyed)
                {
                    ((IDotNetDataSourceKeyed)dotNetDataSource).Keys = keys;
                }

                this.SetProperties(ref dotNetDataSource, contextId);

                editorDataItems = dotNetDataSource
                                    .GetEditorDataItems(contextId)
                                    .Select(x => new EditorDataItem() { Key = x.Key, Label = x.Value })
                                    .ToList();
            }

            return editorDataItems.Where(x => keys.Contains(x.Key));
        }

        IEnumerable<EditorDataItem> IDataSource.GetEditorDataItems(int currentId, int parentId, PageMarker pageMarker, out int total)
        {
            var contextId = currentId == 0 ? parentId : currentId; // HACK: workarround to avoid breaking the IDotNetDataSource interface

            var editorDataItems = Enumerable.Empty<EditorDataItem>();

            total = -1;

            IDotNetDataSource dotNetDataSource = AppDomain.CurrentDomain.CreateInstanceAndUnwrap(Helper.GetAssembly(this.AssemblyName).FullName, this.ClassName) as IDotNetDataSource;

            if (dotNetDataSource != null)
            {
                this.SetProperties(ref dotNetDataSource, contextId);

                if (dotNetDataSource is IDotNetDataSourcePaged)
                {
                    ((IDotNetDataSourcePaged)dotNetDataSource).ItemsPerPage = pageMarker.ItemsPerPage;
                    ((IDotNetDataSourcePaged)dotNetDataSource).Page = pageMarker.Page;

                    editorDataItems = dotNetDataSource
                                        .GetEditorDataItems(contextId)
                                        .Select(x => new EditorDataItem() { Key = x.Key, Label = x.Value })
                                        .ToList();

                    total = ((IDotNetDataSourcePaged)dotNetDataSource).Total;
                }
                else
                {
                    editorDataItems = dotNetDataSource
                                        .GetEditorDataItems(contextId)
                                        .Select(x => new EditorDataItem() { Key = x.Key, Label = x.Value })
                                        .ToList();

                    total = editorDataItems.Count();

                    editorDataItems = editorDataItems
                                        .Skip(pageMarker.Skip)
                                        .Take(pageMarker.Take);
                }
            }

            return editorDataItems;
        }

        private void SetProperties(ref IDotNetDataSource dotNetDataSource, int contextId)
        {
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
                    _logger.Info<DotNetDataSource>(  "Unexpected PropertyType, {PropertyName} is not a string)",propertyInfo.Name );

                }
            }
        }
    }
}