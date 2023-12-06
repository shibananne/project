using ShopHat.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using ShopHat.Models.ProductsViewModels;

namespace ShopHat.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Products> Products { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<PrAge> PrAge { get; set; }
        public DbSet<PrBackside> PrBackside { get; set; }
        public DbSet<PrBrand> PrBrand { get; set; }
        public DbSet<PrClb> PrClb { get; set; }
        public DbSet<PrLeague> PrLeague { get; set; }
        public DbSet<PrModel> PrModel { get; set; }
        public DbSet<PrRim> PrRim { get; set; }
        public DbSet<PrSize> PrSize { get; set; }
        public DbSet<PrSizeDetails> PrSizeDetails { get; set; }
        public DbSet<SubCategory> SubCategory { get; set; }
        public DbSet<PrMainCLB> PrMainCLB { get; set; }
        public DbSet<CokieUser> CokieUser { get; set; }
        public DbSet<CookieCart> CookieCart { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<PrColor> PrColor { get; set; }
        public DbSet<TermsPr> TermsPr { get; set; }
        public DbSet<CouponMain> CouponMain { get; set; }
        public DbSet<CouponChild> CouponChild { get; set; }




    }
}
