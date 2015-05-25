
namespace nuPickers.Shared.DotNetDataSource
{
    /// <summary>
    /// enables a dotnet datasource to supply data to a tree picker
    /// </summary>
    public interface IDotNetDataSourceTree
    {
        /// <summary>
        /// the parent key of the flat set of items to return as per the IDotNetDataSource.GetEditorDataItems method
        /// if null, indicates root items to be returned
        /// </summary>
        string ParentKey { get; set; }
    }
}
