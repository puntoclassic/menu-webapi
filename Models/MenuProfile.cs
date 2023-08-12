namespace MenuBackend.Models
{
    using AutoMapper;
    using MenuBackend.Models.Auth;
    using MenuBackend.Models.DTO;
    using MenuBackend.Models.Entities;

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


