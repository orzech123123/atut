namespace Atut.Paging
{
    public class VueTablesPageRequest : IPagingInfo
    {
        public int Page { get; set; }
        public int Limit { get; set; }

        public int PageNumber => Page;
        public int PageSize => Limit;
    }
}
