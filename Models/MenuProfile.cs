namespace MenuWebapi.Models
{
    using AutoMapper;
    using MenuWebapi.Models.Auth;
    using MenuWebapi.Models.DTO;
    using MenuWebapi.Models.Entities;

    public class MenuProfile : Profile
    {
        public MenuProfile()
        {
            CreateMap<ApplicationUser, UserDTO>();
            CreateMap<Order, OrderDTO>();
            CreateMap<Order, OrderGetAllDTO>();
            CreateMap<Order, GetUserOrdersItemDTO>();
            CreateMap<OrderDetail, OrderDetailDTO>();
            CreateMap<OrderState, OrderStateDTO>();
        }

    }
}


