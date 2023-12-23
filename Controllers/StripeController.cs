using MenuBackend.Models.Data;
using MenuBackend.Models.Options;
using MenuBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace MenuBackend.Controllers;
[ApiController]
[Route("[controller]/[action]")]
public class StripeController : BaseController
{
    private readonly StripeOptions stripeOptions;
    private readonly EmailService emailService;


    public StripeController(ApplicationDbContext _dbContext, EmailService _emailService, IOptions<StripeOptions> _stripeOptions, IOptions<FrontendOptions> _frontendOptions) : base(null, _dbContext)
    {
        this.emailService = _emailService;
        this.stripeOptions = _stripeOptions.Value;
    }


    [HttpPost]
    public async Task<IActionResult> WebHook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        try
        {
            var stripeEvent = EventUtility.ParseEvent(json);

            if (stripeEvent.Type == Events.CheckoutSessionCompleted)
            {
                StripeConfiguration.ApiKey = stripeOptions.SecretKey;
                var data = stripeEvent.Data;

                var data_object = (Session)stripeEvent.Data.Object;

                var metadata = data_object.Metadata;

                var order_id = Int32.Parse(metadata["OrderId"]);

                var order = dbContext.Orders?.Include(i => i.User).Include(i => i.OrderDetails).Where(o => o.Id == order_id).First();

                var settings = dbContext.Settings?.Include(o => o.OrderPaidState).First();

                if (order != null)
                {
                    order.IsPaid = true;
                    order.OrderState = settings != null ? settings.OrderPaidState : order.OrderState;

                    dbContext.Orders?.Update(order);

                    await dbContext.SaveChangesAsync();

                    emailService.SendOrderPaidEmail(order);

                }

            }
            // ... handle other event types
            else
            {
                // Unexpected event type
                Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
            }
            return Ok();
        }
        catch (StripeException)
        {
            return BadRequest();
        }
    }
}

