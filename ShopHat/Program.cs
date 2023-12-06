using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ShopHat.Data;
using ShopHat.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddTransient<ICommon, Common>();
builder.Services.AddControllers();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Forbidden/";
        options.LoginPath = "/Products/Login";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.SlidingExpiration = true;
    });
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMvc(options =>
{
    options.EnableEndpointRouting = false;
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});
var connectionString = builder.Configuration.GetConnectionString("connMSSQL") ?? throw new InvalidCastException(nameof(args));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null);
    }));
var app = builder.Build();



app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowAll");
app.UseRouting();
app.UseSession();
app.UseMiddleware<EnsureUniqueUserCookieMiddleware>();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapAreaControllerRoute(
      name: "Areas",
      areaName: "AdminShopHat",
      pattern: "{controller=Products}/{action=Index}/{id?}");
app.Run();
