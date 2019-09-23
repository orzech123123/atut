namespace Atut.Sorting
{
    public class VueTablesSortRequest : ISortingInfo
    {
        public int Ascending { get; set; }
        public string OrderBy { get; set; }

        public string ColumnName => OrderBy != null ?
            char.ToUpper(OrderBy[0]) + OrderBy.Substring(1) :
            null;

        public bool IsAscending => Ascending != 0;
    }
}
