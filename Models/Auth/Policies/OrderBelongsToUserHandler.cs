using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using MenuBackend.Models.Data;
using MenuBackend.Models.InputModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MenuBackend.Models.Auth.Policies
{

    public class OrderBelongsToUserHandler : AuthorizationHandler<OrderBelongsToUserRequirement>
    {

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDbContext dbContext;

        public OrderBelongsToUserHandler(ApplicationDbContext _dbContext, IHttpContextAccessor _httpContextAccessor)
        {
            this.httpContextAccessor = _httpContextAccessor;
            this.dbContext = _dbContext;
        }

        private ApplicationUser? GetUserFromRequest(HttpContext httpContext)
        {

            var userIdClaim = httpContext.User.FindFirst("UserId");

            if (userIdClaim != null)
            {
                return dbContext.Users?.Find(userIdClaim.Value);
            }

            return null;
        }

        protected override async Task<Task> HandleRequirementAsync(
        AuthorizationHandlerContext context, OrderBelongsToUserRequirement requirement)
        {

            var request = httpContextAccessor.HttpContext!.Request;

            int? orderId = null;

            var user = GetUserFromRequest(httpContext: httpContextAccessor.HttpContext!);

            var role = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role);

            if (role != null && role.Value == "admin")
            {
                context.Succeed(requirement);
            }
            else
            {
                if (request.Method == "GET")
                {
                    orderId = Convert.ToInt32(request.Query.First(w => w.Key == "OrderId").Value);
                }
                else if (request.Method == "POST")
                {
                    request.EnableBuffering();

                    string bodyContent = await new StreamReader(request.Body).ReadToEndAsync(); ;

                    OrderGenericRequest requestBodyObject = JsonConvert.DeserializeObject<OrderGenericRequest>(bodyContent);

                    orderId = requestBodyObject.OrderId;

                    request.Body.Position = 0;

                }

                if (orderId != null)
                {
                    try
                    {
                        var order = dbContext.Orders?.Include(i => i.User).Where(w => w.Id == orderId).First();

                        if (order!.User == user)
                        {
                            context.Succeed(requirement);
                        }

                    }
                    catch (InvalidOperationException)
                    {

                    }
                }
            }

            return Task.FromResult(Task.CompletedTask);
        }

    }
}
