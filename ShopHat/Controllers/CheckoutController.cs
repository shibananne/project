using Microsoft.AspNetCore.Mvc;
using ShopHat.Data;
using ShopHat.Models.CokieUserViewModels;

namespace ShopHat.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ILogger<CheckoutController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        public CheckoutController(ILogger<CheckoutController> logger, ApplicationDbContext context, IWebHostEnvironment env)
        {
            _logger = logger;
            _context = context;
            _env = env;

        }
        public IActionResult Index()
        {
            return View();
        } 
        public IActionResult ThankYou()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ConfirmCoupon(string Cop)
        {
            var pr = _context.CouponChild.FirstOrDefault(x=> x.CouponMain == Cop);
            if (pr == null)
            {
                return new JsonResult(new
                {
                    code = 203,
                    status = "Không tìm thấy"
                });
            }
            else
            {
                return new JsonResult(new
                {
                    code = 200,
                    status = "Thành công",
                    Coupon = pr
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmCart(int OrderId, CokieUserCRUD model)
        {
            var order = _context.Order.FirstOrDefault(x => x.ID == OrderId);

            const string cookieName = "UniqueUserIdentifier";
            var userCookieValue = Request.Cookies[cookieName];

            if (string.IsNullOrEmpty(userCookieValue))
            {
                return NotFound("Cookie not found.");
            }
            var user = _context.CokieUser.FirstOrDefault(x=>x.CokieUserMain == userCookieValue);
            if (order != null)
            {
                if(user != null)
                {
                    user.FullName = model.FullName;
                    user.Email = model.Email;
                    user.FullAddress = model.FullAddress;
                    user.City = model.City;
                    user.District = model.District;
                    user.Ward = model.Ward;
                    user.DetailsAddress = model.DetailsAddress;
                    await _context.SaveChangesAsync();

                }
                order.IsDone = true;
                order.Address = model.FullAddress;
                order.CustomerName = model.FullName;
                order.Address = model.FullAddress;
                order.Phone_Number = model.Phone;
                
                await _context.SaveChangesAsync();
                return new JsonResult(new
                {
                    code = 200,
                    status = "Success",
                });
            }
            return new JsonResult(new
            {
                code = 203,
                status = "False",
            });

        }
    }
}
