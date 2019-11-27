using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop.Auth;
using Shop.Models;

namespace Shop.Model
    {
    public class AppDbContext : IdentityDbContext<ApplicationUser>
        {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            {



            }

        protected override void OnModelCreating(ModelBuilder builder)
            {
            base.OnModelCreating(builder);
            builder.Entity<Pie>()
                .HasOne(p => p.RecipeInformation)
                .WithOne(i => i.Pie)
                .HasForeignKey<RecipeInformation>(b => b.PieId);

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            }

        public DbSet<Pie> Pies { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<PieReview> PieReviews { get; set; }
        public DbSet<PieGiftOrder> PieGiftOrders { get; set; }
        }



    }
