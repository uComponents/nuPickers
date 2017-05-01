namespace nuPickers.Shared.DotNetDataSource
{
    public interface IDotNetDataSourcePaged
    {
        /// <summary>
        /// Number of items to skip (set by nuPickers before query)
        /// </summary>
        int Skip { set; }

        /// <summary>
        /// Number of items to take (set by nuPickers before query)
        /// </summary>
        int Take { set; }

        /// <summary>
        /// The total number of items available (regardless of skip and take values - read by nuPickers after query)
        /// </summary>
        int Count { get; }
    }
}