
namespace nuPickers.Shared.SqlDataSource
{
    using nuPickers.Shared.Editor;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Umbraco.Core.Persistence;
    using System.Linq;

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

        public IEnumerable<EditorDataItem> GetEditorDataItemsFilteredByIds(int contextId, string ids)
        {
            List<EditorDataItem> result = new List<EditorDataItem>();
            if (ids != null)
            {
                IEnumerable<string> collectionIds = ids.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).AsEnumerable<string>();
                result = GetEditorDataItems(contextId).Where(x => ids.Contains(x.Key)).ToList<EditorDataItem>();
            }
            return result;
        }
    }
}
