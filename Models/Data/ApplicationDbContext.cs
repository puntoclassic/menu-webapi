using MenuWebapi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MenuWebapi.Models.Auth;
namespace MenuWebapi.Models.Data
{
    public class ApplicationDbContext : IdentityUserContext<ApplicationUser>
    {
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Food>? Foods { get; set; }
        public DbSet<Setting>? Settings { get; set; }
        public DbSet<OrderState>? OrderStates { get; set; }
        public DbSet<Order>? Orders { get; set; }
        public DbSet<OrderDetail>? OrderDetails { get; set; }
        public DbSet<Carrier> Carriers { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public override int SaveChanges()
        {
            UpdateOrderTotals();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateOrderTotals();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateOrderTotals()
        {
            foreach (var entry in ChangeTracker.Entries<Order>())
            {
                if (entry.State == EntityState.Modified || entry.State == EntityState.Added)
                {
                    var order = entry.Entity;

                    var orderDetails = this.OrderDetails?.Where(od => od.OrderId == order.Id);

                    if (orderDetails != null)
                    {
                        float newTotal = orderDetails.Sum(od => od.UnitPrice * od.Quantity) + order.ShippingCosts;
                        order.Total = newTotal;

                    }
                }
            }
        }



    }
}
