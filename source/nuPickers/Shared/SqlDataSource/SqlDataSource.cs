using NPoco;

namespace nuPickers.Shared.SqlDataSource
{
    using DataSource;
    using nuPickers.Shared.Editor;
    using Paging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Umbraco.Core.Persistence;

    public class SqlDataSource : IDataSource
    {
        private bool handledTypeahead = false;

        public string SqlExpression { get; set; }

        public string ConnectionString { get; set; }

        bool IDataSource.HandledTypeahead { get { return this.handledTypeahead; } }

        IEnumerable<EditorDataItem> IDataSource.GetEditorDataItems(int currentId, int parentId, string typeahead)
        {
            return this.GetEditorDataItems(currentId == 0 ? parentId : currentId, typeahead);
        }

        IEnumerable<EditorDataItem> IDataSource.GetEditorDataItems(int currentId, int parentId, string[] keys)
        {
            return this.GetEditorDataItems(currentId == 0 ? parentId : currentId).Where(x => keys.Contains(x.Key));
        }

        IEnumerable<EditorDataItem> IDataSource.GetEditorDataItems(int currentId, int parentId, PageMarker pageMarker, out int total)
        {
            var editorDataItems = this.GetEditorDataItems(currentId == 0 ? parentId : currentId);

            total = editorDataItems.Count();

            return editorDataItems.Skip(pageMarker.Skip).Take(pageMarker.Take);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="contextId"></param>
        /// <returns></returns>
        private IEnumerable<EditorDataItem> GetEditorDataItems(int contextId, string typeahead = null)
        {
            List<EditorDataItem> editorDataItems = new List<EditorDataItem>();

            Database database = new Database(this.ConnectionString);

            if (database != null)
            {
                string sql = Regex.Replace(this.SqlExpression, "\n|\r", " ")
                             .Replace("@contextId", "@0")
                             .Replace("@typeahead", "@1");

                if (this.SqlExpression.Contains("@typeahead")) // WARNING: not a perfect check !
                {
                    this.handledTypeahead = true;
                }

                editorDataItems = database.Fetch<EditorDataItem>(sql, contextId, typeahead);
            }

            return editorDataItems;
        }
    }
}