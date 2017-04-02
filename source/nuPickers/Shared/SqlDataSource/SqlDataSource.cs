
namespace nuPickers.Shared.SqlDataSource
{
    using DataSource;
    using nuPickers.Shared.Editor;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text.RegularExpressions;
    using Umbraco.Core.Persistence;

    public class SqlDataSource : IDataSource
    {
        public string SqlExpression { get; set; }

        public string ConnectionString { get; set; }

        public string Typeahead { get; set; }

        [DefaultValue(false)]
        public bool HandledTypeahead { get; private set; }

        public IEnumerable<EditorDataItem> GetEditorDataItems(int currentId, int parentId)
        {
            return this.GetEditorDataItems(currentId);
        }

        /// <summary>
        /// NOTE: this method remains for legacy purposes (although nothing should be using it)
        /// </summary>
        /// <param name="contextId"></param>
        /// <returns></returns>
        [Obsolete]
        public IEnumerable<EditorDataItem> GetEditorDataItems(int contextId)
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
                    this.HandledTypeahead = true;
                }

                editorDataItems = database.Fetch<EditorDataItem>(sql, contextId, this.Typeahead);
            }

            return editorDataItems;
        }
    }
}
