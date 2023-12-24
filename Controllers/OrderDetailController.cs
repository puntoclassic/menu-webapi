using AutoMapper;
using MenuWebapi.Models.Auth;
using MenuWebapi.Models.Data;
using MenuWebapi.Models.Entities;
using MenuWebapi.Models.InputModel;
using MenuWebapi.Models.Options;
using MenuWebapi.Models.ResponseModel;
using MenuWebapi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MenuWebapi.Controllers
{

    [ApiController]
    [Route("[controller]/[action]")]
    public class OrderDetailController : BaseController
    {

        private readonly FrontendOptions frontendOptions;
        private readonly IMapper mapper;

        public OrderDetailController(UserManager<ApplicationUser> _userManager, ApplicationDbContext _dbContext, IOptions<FrontendOptions> _frontendOptions, IMapper _mapper) : base(_userManager, _dbContext)
        {
            this.frontendOptions = _frontendOptions.Value;
            this.mapper = _mapper;
        }

        //Update order detail quantity
        [HttpPost]
        [Authorize(Policy = "OrderBelongsToUser")]
        public async Task<PostGenericResponse> PostUpdateOrderDetailQuantity([FromBody] PostUpdateOrderDetailQuantityRequest inputModel)
        {
            try
            {
                var orderDetail = dbContext.OrderDetails?.Find(inputModel.OrderDetailId);

                if (orderDetail != null)
                {
                    orderDetail.Quantity = inputModel.Quantity;
                    dbContext.OrderDetails?.Update(orderDetail);
                    await dbContext.SaveChangesAsync();

                    return new PostGenericResponse
                    {
                        Status = "Ok"

                    };
                }
                else
                {
                    return new PostGenericResponse
                    {
                        ErrorMessage = "Operation failed",
                        Status = "Error"
                    };
                }

            }
            catch (InvalidOperationException)
            {
                return new PostGenericResponse
                {
                    ErrorMessage = "Operation failed"

                };
            }
        }

        [HttpPost]
        [Authorize(Policy = "OrderBelongsToUser")]
        public async Task<PostGenericResponse> PostRemoveOrderDetail([FromBody] PostRemoveOrderDetail inputModel)
        {
            try
            {
                var orderDetail = dbContext.OrderDetails?.Find(inputModel.OrderDetailId);

                if (orderDetail != null)
                {
                    dbContext.OrderDetails?.Update(orderDetail);
                    await dbContext.SaveChangesAsync();

                    return new PostGenericResponse
                    {
                        Status = "Ok"
                    };
                }
                else
                {
                    return new PostGenericResponse
                    {
                        ErrorMessage = "Operation failed",
                        Status = "Error"
                    };
                }

            }
            catch (InvalidOperationException)
            {
                return new PostGenericResponse
                {
                    ErrorMessage = "Operation failed"

                };
            }
        }

        [HttpPost]
        [Authorize(Policy = "OrderBelongsToUser")]
        public async Task<PostGenericResponse> PostCreateOrderDetail([FromBody] PostCreateOrderDetail inputModel)
        {
            try
            {

                var order = dbContext.Orders?.Find(inputModel.OrderId);

                if (order != null)
                {
                    var orderDetail = new OrderDetail
                    {
                        Name = inputModel.Name,
                        OrderId = inputModel.OrderId,
                        Quantity = inputModel.Quantity,
                        UnitPrice = inputModel.UnitPrice
                    };

                    dbContext.OrderDetails?.Add(orderDetail);

                    await dbContext.SaveChangesAsync();

                    return new PostGenericResponse
                    {
                        Status = "Ok"
                    };
                }
                else
                {
                    return new PostGenericResponse
                    {
                        ErrorMessage = "Operation failed",
                        Status = "Error"
                    };
                }


            }
            catch (InvalidOperationException)
            {
                return new PostGenericResponse
                {
                    ErrorMessage = "Operation failed"

                };
            }
        }


        //Create order detail (add item to order)

    }
}
