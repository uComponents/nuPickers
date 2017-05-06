namespace nuPickers.Shared.DotNetDataSource
{
    /// <summary>
    /// Implementing this interface will enable a dot-net-data-source to support returning returning only those options matching specific keys
    /// </summary>
    public interface IDotNetDataSourceKeyed
    {
        /// <summary>
        /// (set by nuPickers before query)
        /// </summary>
        string[] Keys { set; }
    }
}