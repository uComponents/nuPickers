namespace nuPickers.Shared.Paging
{
    /// <summary>
    /// represents itemsPerPage and a page number - used to caluclate skip/take values
    /// </summary>
    internal class PageMarker
    {
        internal int ItemsPerPage { get; private set; }

        internal int Page { get; private set; }

        internal int Skip { get; private set; }

        internal int Take { get; private set; }

        internal PageMarker(int itemsPerPage, int page)
        {
            this.ItemsPerPage = itemsPerPage;
            this.Page = page;
            this.Skip = itemsPerPage * (page - 1);
            this.Take = itemsPerPage;
        }
    }
}