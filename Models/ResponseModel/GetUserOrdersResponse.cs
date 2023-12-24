using MenuWebapi.Models.DTO;

namespace MenuWebapi.Models.ResponseModel
{
    public class GetUserOrdersResponse
    {
        public int Count { get; set; }
        public ICollection<GetUserOrdersItemDTO>? Items { get; set; }
        public int Pages { get; set; }
    }
}
