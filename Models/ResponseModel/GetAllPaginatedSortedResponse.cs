namespace MenuWebapi.Models.ResponseModel
{
    public class GetAllPaginatedSortedResponse<T>
    {
        public int Count { get; set; }
        public ICollection<T>? Items { get; set; }
        public int Pages { get; set; }
    }
}
