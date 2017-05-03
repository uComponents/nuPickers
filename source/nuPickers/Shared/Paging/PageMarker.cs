namespace nuPickers.Shared.Paging
{
    /// <summary>
    /// represents itemsPerPage and a page number - used to caluclate skip/take values
    /// </summary>
    internal class PageMarker
    {
        private int itemsPerPage;

        private int page;

        private int skip;

        private int take;

        internal int ItemsPerPage
        {
            get
            {
                return this.itemsPerPage;
            }
        }

        internal int Page
        {
            get
            {
                return this.page;
            }
        }

        internal int Skip
        {
            get
            {
                return this.skip;
            }
        }

        internal int Take
        {
            get
            {
                return this.take;
            }
        }

        internal PageMarker(int itemsPerPage, int page)
        {
            this.itemsPerPage = itemsPerPage;
            this.page = page;
            this.skip = itemsPerPage * (page - 1);
            this.take = itemsPerPage;
        }
    }
}