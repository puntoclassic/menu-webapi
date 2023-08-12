using AutoMapper;
using MenuBackend.Models;
using MenuBackend.Models.Auth;
using MenuBackend.Models.Data;
using MenuBackend.Models.DTO;
using MenuBackend.Models.Entities;
using MenuBackend.Models.InputModel;
using MenuBackend.Models.Options;
using MenuBackend.Models.ResponseModel;
using MenuBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace MenuBackend.Controllers;
[ApiController]
[Route("[controller]/[action]")]

public class OrderController : BaseController
{
    private readonly EmailService emailService;
    private readonly StripeOptions stripeOptions;
    private readonly FrontendOptions frontendOptions;
    private readonly IMapper mapper;

    public OrderController(UserManager<ApplicationUser> _userManager, ApplicationDbContext _dbContext, EmailService _emailService, IOptions<StripeOptions> _stripeOptions, IOptions<FrontendOptions> _frontendOptions, IMapper _mapper) : base(_userManager, _dbContext)
    {
        this.emailService = _emailService;
        this.stripeOptions = _stripeOptions.Value;
        this.frontendOptions = _frontendOptions.Value;
        this.mapper = _mapper;
    }

    [HttpPost]
    [Authorize]
    public async Task<PostOrderCreatedResponse> PostCreate([FromBody] Cart cart)
    {
        var user = await GetUserFromRequest();
        if (user != null)
        {

            var settings = dbContext.Settings?.Include(i => i.OrderCreatedState).First();

            var order = new Order
            {
                IsPaid = false,
                DeliveryAddress = cart.Indirizzo,
                DeliveryTime = cart.Orario,
                IsShippingRequired = cart.TipologiaConsegna == "DOMICILIO",
                Notes = cart.Note,
                ShippingCosts = (float)(settings != null ? settings.ShippingCosts! : 2),
                User = user,
                OrderState = settings?.OrderCreatedState
            };

            dbContext.Add(order);

            await dbContext.SaveChangesAsync();


            if (order.Id > 0)
            {
                foreach (var item in cart.Items!.Values)
                {
                    var orderDetail = new OrderDetail
                    {
                        Order = order,
                        Name = item.Item!.Name,
                        UnitPrice = item.Item!.Price,
                        Quantity = item.Quantity
                    };
                    dbContext.Add(orderDetail);
                }

            }

            await dbContext.SaveChangesAsync();

            emailService.SendOrderCreatedEmail(order);


            return new PostOrderCreatedResponse
            {
                Status = "Order created",
                OrderId = order.Id
            };
        }
        else
        {
            return new PostOrderCreatedResponse
            {
                Status = "Order creation failed"
            };
        }
    }


    [HttpPost]
    [Authorize(Policy = "OrderBelongsToUser")]
    public async Task<PostGenericResponse> PostUpdate([FromBody] PostUpdateOrderRequest inputModel)
    {

        try
        {
            var order = dbContext.Orders?.Find(inputModel.Id);

            if (order != null)
            {

                order.IsPaid = inputModel.IsPaid;
                order.DeliveryAddress = inputModel.DeliveryAddress;
                order.DeliveryTime = inputModel.DeliveryTime;
                order.IsShippingRequired = inputModel.IsShippingRequired;
                order.ShippingCosts = inputModel.ShippingCosts;
                order.Notes = inputModel.Notes;
                order.OrderStateId = inputModel.OrderStateId;

                dbContext.Orders?.Update(order);

                await dbContext.SaveChangesAsync();

                return new PostGenericResponse
                {
                    Status = "Update completed"
                };

            }
            else
            {
                return new PostGenericResponse
                {
                    ErrorMessage = "Order not founded"
                };
            }

        }
        catch (InvalidOperationException)
        {
            return new PostGenericResponse
            {
                ErrorMessage = "Invalid operation"
            };
        }

    }

    [HttpGet]
    [Authorize]
    public async Task<GetUserOrdersResponse> GetUserOrders([FromQuery] GetAllPaginatedSortedRequest inputModel)
    {
        var orders = dbContext.Orders?.AsQueryable();
        var user = await GetUserFromRequest();

        if (orders != null && user != null)
        {
            orders = orders.Include(i => i.OrderDetails).Include(i => i.OrderState).Where(w => w.User == user);
            if (inputModel.Paginated)
            {

                int count = orders.Count();
                int pages = Convert.ToInt32(Math.Ceiling((double)count / inputModel.PerPage));
                switch (inputModel.OrderBy)
                {
                    default:
                        orders = inputModel.Ascending ? orders.OrderBy(o => o.Id) : orders.OrderByDescending(o => o.Id);
                        break;
                }

                var items = orders.Skip(inputModel.PerPage * (inputModel.Page - 1)).Take(inputModel.PerPage).ToList();

                return new GetUserOrdersResponse
                {
                    Count = count,
                    Items = mapper.Map<GetUserOrdersItemDTO[]>(items.ToList()),
                    Pages = pages
                };
            }
            else
            {
                int count = orders.Count();
                return new GetUserOrdersResponse
                {
                    Count = count,
                    Items = mapper.Map<GetUserOrdersItemDTO[]>(orders.ToList()),
                    Pages = 1
                };
            }
        }
        return new GetUserOrdersResponse
        {
            Count = 0,
            Items = null,
            Pages = 0
        };

    }

    [HttpGet]
    [Authorize(Policy = "OrderBelongsToUser")]
    public async Task<GetPaymentUrlResponse> GetPaymentUrl([FromQuery] int OrderId)
    {
        var user = await GetUserFromRequest();
        if (user != null)
        {
            try
            {
                var order = dbContext.Orders?.Include(i => i.OrderDetails).Where(w => w.Id == OrderId).First();

                StripeConfiguration.ApiKey = stripeOptions.SecretKey;

                var lineItems = new List<SessionLineItemOptions>();

                foreach (var item in order!.OrderDetails!)
                {
                    lineItems.Add(new SessionLineItemOptions
                    {
                        Quantity = item.Quantity,
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "eur",
                            UnitAmount = (long)(Math.Round(item.UnitPrice, 2) * 100),
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Name
                            }
                        }
                    });
                }

                if (order.IsShippingRequired)
                {
                    lineItems.Add(new SessionLineItemOptions
                    {
                        Quantity = 1,
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "eur",
                            UnitAmount = (long)(Math.Round(order.ShippingCosts, 2) * 100),
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Spese di consegna"
                            }
                        }
                    });
                }

                var options = new SessionCreateOptions
                {
                    LineItems = lineItems,
                    Mode = "payment",
                    SuccessUrl = $"{frontendOptions.BaseUrl}{frontendOptions.PaymentSuccessUrl}",
                    CancelUrl = $"{frontendOptions.BaseUrl}{frontendOptions.PaymentFailedUrl}",
                    Metadata = new Dictionary<string, string>
                        {
                            {"OrderId",$"{order.Id}"}
                        }
                };

                var service = new SessionService();
                Session session = service.Create(options);

                return new GetPaymentUrlResponse
                {
                    PaymentUrl = session.Url
                };

            }
            catch (ArgumentNullException)
            {
                HttpContext.Response.StatusCode = 500;
                return new GetPaymentUrlResponse
                {
                    ErrorMessage = "Bad order",
                    Status = "Request failed"
                };
            }
        }
        else
        {
            HttpContext.Response.StatusCode = 500;
            return new GetPaymentUrlResponse
            {
                ErrorMessage = "Bad user",
                Status = "Request failed"
            };
        }
    }

    [HttpGet]
    [Authorize(Policy = "OrderBelongsToUser")]
    public async Task<OrderDTO?> GetOrderDetail([FromQuery] int OrderId)
    {
        var user = await GetUserFromRequest();
        if (user != null)
        {
            try
            {
                var order = dbContext.Orders?.Include(i => i.OrderDetails).Include(i => i.OrderState).Where(w => w.Id == OrderId).First();
                return mapper.Map<OrderDTO>(order);
            }
            catch (ArgumentNullException)
            {
                HttpContext.Response.StatusCode = 500;
                return null;
            }
        }
        else
        {
            HttpContext.Response.StatusCode = 500;
            return null;
        }
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public GetAllPaginatedSortedResponse<OrderGetAllDTO> GetAll([FromQuery] GetAllPaginatedSortedRequest inputModel)
    {
        var orders = dbContext.Orders?
        .Include(i => i.OrderDetails)
        .Include(i => i.User)
        .Include(i => i.OrderState)
        .AsQueryable();

        if (orders != null)
        {
            if (inputModel.Paginated)
            {
                if (inputModel.SearchKey != null)
                {
                    Func<Order, bool> filter = order =>
                    {
                        bool orderDetailsContainSearchKey = order.OrderDetails!.Any(c => c.Name!.ToLower().Contains(inputModel.SearchKey.ToLower()));
                        bool orderIdMatchesSearchKey = order.Id.ToString() == inputModel.SearchKey;

                        if (order.User != null)
                        {
                            bool userContainsSearchKey =
                                order.User.Firstname.ToLower().Contains(inputModel.SearchKey.ToLower()) ||
                                order.User.Lastname.ToLower().Contains(inputModel.SearchKey.ToLower());

                            return userContainsSearchKey || orderDetailsContainSearchKey || orderIdMatchesSearchKey;
                        }
                        else
                        {
                            return orderDetailsContainSearchKey || orderIdMatchesSearchKey;
                        }

                    };

                    orders = orders.Where(filter).AsQueryable();
                }


                int count = orders.Count();
                int pages = Convert.ToInt32(Math.Ceiling((double)count / inputModel.PerPage));
                switch (inputModel.OrderBy)
                {
                    case "total":
                        orders = inputModel.Ascending ? orders.OrderBy(o => o.Total) : orders.OrderByDescending(o => o.Total);
                        break;
                    case "user":
                        if (inputModel.Ascending)
                            orders = orders.OrderBy(o => o.User!.Firstname).ThenBy(o => o.User!.Lastname);
                        else
                            orders = orders.OrderByDescending(o => o.User!.Firstname).ThenByDescending(o => o.User!.Lastname);
                        break;
                    default:
                        if (inputModel.Ascending)
                            orders = orders.OrderBy(o => o.Id);
                        else
                            orders = orders.OrderByDescending(o => o.Id);
                        break;
                }


                var items = orders.Skip(inputModel.PerPage * (inputModel.Page - 1)).Take(inputModel.PerPage).ToList();
                return new GetAllPaginatedSortedResponse<OrderGetAllDTO>
                {
                    Count = count,
                    Items = mapper.Map<OrderGetAllDTO[]>(items),
                    Pages = pages
                };
            }
            else
            {
                int count = orders.Count();
                return new GetAllPaginatedSortedResponse<OrderGetAllDTO>
                {
                    Count = count,
                    Items = mapper.Map<OrderGetAllDTO[]>(orders.ToList()),
                    Pages = 1
                };
            }
        }
        return new GetAllPaginatedSortedResponse<OrderGetAllDTO>
        {
            Count = 0,
            Items = null,
            Pages = 0
        };
    }


}
