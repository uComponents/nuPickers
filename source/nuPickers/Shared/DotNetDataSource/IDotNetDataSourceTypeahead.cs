
namespace nuPickers.Shared.DotNetDataSource
{
    /// <summary>
    /// enables a dotnet datasource to supply data to a typeahead list picker
    /// </summary>
    public interface IDotNetDataSourceTypeahead
    {
        /// <summary>
        /// the current typeahead input text
        /// </summary>
        string Typeahead { get; set; }
    }
}
