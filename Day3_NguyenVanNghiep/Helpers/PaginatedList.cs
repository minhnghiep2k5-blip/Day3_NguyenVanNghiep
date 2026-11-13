namespace Day3_NguyenVanNghiep.Helpers
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; set; }

        public int PageIndex { get; private set; }

        public int TotalPages { get; private set; }

        public int TotalCount { get; private set; }

        public int PageSize { get; private set; }

        public int PreviousPageIndex => PageIndex - 1;

        public int NextPageIndex => PageIndex + 1;

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public PaginatedList(
            List<T> items,
            int totalCount,
            int pageIndex,
            int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageIndex = pageIndex;
            PageSize = pageSize;

            TotalPages = (int)Math.Ceiling(
                (double)totalCount / pageSize
            );
        }
    }
}