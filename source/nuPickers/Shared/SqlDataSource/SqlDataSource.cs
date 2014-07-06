
namespace nuPickers.Shared.SqlDataSource
{
    using nuPickers.Shared.Editor;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Umbraco.Core.Persistence;

    public class SqlDataSource
    {
        public string SqlExpression { get; set; }

        public string ConnectionString { get; set; }

        public string Typeahead { get; set; } // the value supplied by the user - the current typeahead text

        public IEnumerable<EditorDataItem> GetEditorDataItems(int contextId) // supply option typeahead param
        {
            List<EditorDataItem> editorDataItems = new List<EditorDataItem>();

            Database database = new Database(this.ConnectionString);

            if (database != null)
            {
                string sql = Regex.Replace(this.SqlExpression, "\n|\r", " ")
                             .Replace("@contextId", "@0")
                             .Replace("@typeahead", "@1");


                editorDataItems = database.Fetch<EditorDataItem>(sql, contextId, this.Typeahead);
            }

            return editorDataItems;
        }
    }
}
