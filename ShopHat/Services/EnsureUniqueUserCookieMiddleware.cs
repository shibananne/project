using ShopHat.Data;
using ShopHat.Models;

namespace ShopHat.Services
{
    public class EnsureUniqueUserCookieMiddleware
    {
        private readonly RequestDelegate _next;

        public EnsureUniqueUserCookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext)
        {
            const string cookieName = "UniqueUserIdentifier";

            // Kiểm tra xem cookie đã tồn tại chưa
            if (!context.Request.Cookies.ContainsKey(cookieName))
            {
                var uniqueUserId = Guid.NewGuid().ToString();

                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddYears(1)
                };

                context.Response.Cookies.Append(cookieName, uniqueUserId, cookieOptions);

                var userCookie = new CokieUser
                {
                    CokieUserMain = uniqueUserId
                };
                dbContext.CokieUser.Add(userCookie);
                dbContext.SaveChanges();
            }

        
            await _next(context);
        }
    }
}
