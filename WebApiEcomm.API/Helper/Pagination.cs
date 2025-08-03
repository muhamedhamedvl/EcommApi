namespace WebApiEcomm.API.Helper
{
    public class Pagination<T>where T : class
    {
        public Pagination(int pageNumber, int page, int totalCount, IEnumerable<T> data)
        {
            PageNumber = pageNumber;
            Page = page;
            TotalCount = totalCount;
            Data = data;
        }

        public int PageNumber { get; set; }
        public int Page {  get; set; }
        public int TotalCount { get; set; }

        public IEnumerable<T> Data { get; set; }

    }
}
