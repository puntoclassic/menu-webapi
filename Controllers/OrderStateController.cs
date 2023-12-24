using MenuWebapi.Models.Data;
using MenuWebapi.Models.Entities;
using MenuWebapi.Models.InputModel;
using MenuWebapi.Models.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace MenuWebapi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class OrderStateController : BaseController
    {
        public OrderStateController(ApplicationDbContext _dbContext) : base(_userManager: null, _dbContext: _dbContext)
        {
        }

        [HttpGet("{id}")]
        public OrderState? GetById([FromRoute] int Id)
        {
            try
            {
                var orderState = dbContext.OrderStates?.Find(Id);
                if (orderState != null)
                {
                    return orderState;
                }
                else
                {
                    HttpContext.Response.StatusCode = 500;
                    return null;
                }
            }
            catch (InvalidOperationException)
            {
                HttpContext.Response.StatusCode = 500;
                return null;
            }
        }
        [HttpGet]
        public GetAllPaginatedSortedResponse<OrderState> GetAll([FromQuery] GetAllPaginatedSortedRequest inputModel)
        {
            var orderStates = dbContext.OrderStates?.AsQueryable();
            if (orderStates != null)
            {
                if (inputModel.Paginated)
                {
                    if (inputModel.SearchKey != null)
                    {
                        orderStates = orderStates.Where(w => w.Name.ToLower().Contains(inputModel.SearchKey.ToLower())).AsQueryable();
                    }
                    int count = orderStates.Count();
                    int pages = Convert.ToInt32(Math.Ceiling((double)count / inputModel.PerPage));
                    switch (inputModel.OrderBy)
                    {
                        case "Id":
                            orderStates = inputModel.Ascending ? orderStates.OrderBy(o => o.Id) : orderStates.OrderByDescending(o => o.Id);
                            break;
                        case "Name":
                            orderStates = inputModel.Ascending ? orderStates.OrderBy(o => o.Name) : orderStates.OrderByDescending(o => o.Name);
                            break;
                        default:
                            orderStates = inputModel.Ascending ? orderStates.OrderBy(o => o.Id) : orderStates.OrderByDescending(o => o.Id);
                            break;
                    }
                    return new GetAllPaginatedSortedResponse<OrderState>
                    {
                        Count = count,
                        Items = orderStates.Skip(inputModel.PerPage * (inputModel.Page - 1)).Take(inputModel.PerPage).ToList(),
                        Pages = pages
                    };
                }
                else
                {
                    int count = orderStates.Count();
                    return new GetAllPaginatedSortedResponse<OrderState>
                    {
                        Count = count,
                        Items = orderStates.ToList(),
                        Pages = 1
                    };
                }
            }
            return new GetAllPaginatedSortedResponse<OrderState>
            {
                Count = 0,
                Items = null,
                Pages = 0
            };
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<PostGenericResponse> PostCreateOrderState([FromBody] PostCreateOrderState inputModel)
        {
            try
            {
                dbContext.OrderStates?.Add(new OrderState
                {
                    Name = inputModel.Name!,
                    CssBadgeClass = inputModel.CssBadgeClass
                });
                await dbContext.SaveChangesAsync();
                HttpContext.Response.StatusCode = 201;
                return new PostGenericResponse
                {
                    Status = "Order state created"
                };
            }
            catch (InvalidOperationException)
            {
                HttpContext.Response.StatusCode = 500;
                return new PostGenericResponse
                {
                    Status = "Failed",
                    ErrorMessage = "Order state creation failed"
                };
            }
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<PostGenericResponse> PostUpdateOrderState([FromBody] PostUpdateOrderState inputModel)
        {
            try
            {
                var orderState = dbContext.OrderStates?.Find(inputModel.Id);
                if (orderState != null)
                {
                    orderState.Name = inputModel.Name!;
                    orderState.CssBadgeClass = inputModel.CssBadgeClass;
                    dbContext.Update(orderState);
                    await dbContext.SaveChangesAsync();
                    HttpContext.Response.StatusCode = 201;
                    return new PostGenericResponse
                    {
                        Status = "Order state updated"
                    };
                }
                else
                {
                    return new PostGenericResponse
                    {
                        Status = "Failed",
                        ErrorMessage = "Order state update failed"
                    };
                }
            }
            catch (InvalidOperationException)
            {
                HttpContext.Response.StatusCode = 500;
                return new PostGenericResponse
                {
                    Status = "Failed",
                    ErrorMessage = "Order state update failed"
                };
            }
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<PostGenericResponse> PostDeleteOrderState([FromBody] PostDeleteOrderState inputModel)
        {
            try
            {
                var orderState = dbContext.OrderStates?.Find(inputModel.Id);
                if (orderState != null)
                {
                    dbContext.Remove(orderState);
                    await dbContext.SaveChangesAsync();
                    HttpContext.Response.StatusCode = 201;
                    return new PostGenericResponse
                    {
                        Status = "Order state deleted"
                    };
                }
                else
                {
                    return new PostGenericResponse
                    {
                        Status = "Failed",
                        ErrorMessage = "Order state delete failed"
                    };
                }
            }
            catch (InvalidOperationException)
            {
                HttpContext.Response.StatusCode = 500;
                return new PostGenericResponse
                {
                    Status = "Failed",
                    ErrorMessage = "Order state delete failed"
                };
            }
        }
    }
}
