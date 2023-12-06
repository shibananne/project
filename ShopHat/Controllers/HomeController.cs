using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopHat.Data;
using ShopHat.Models;
using ShopHat.Models.ProductsViewModels;
using ShopHat.Services;
using System.Diagnostics;
using System.Drawing.Printing;

namespace ShopHat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IWebHostEnvironment env)
        {
            _logger = logger;
            _context = context;
            _env = env;

        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [Route("/info/terms-and-conditions")]
        public IActionResult Terms()
        {
            return View();
        }
        [Route("/info/contact-us")]
        public IActionResult ContactUs()
        {
            return View();
        }
          [Route("/info/about-thebunder")]
        public IActionResult Thebunder()
        {
            return View();
        }
           [Route("/info/returns-exchanges")]
        public IActionResult ReturnsExchanges()
        {
            return View();
        }

        [Route("exclusives")]

        public IActionResult Exclusives()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetCountCart()
        {
            const string cookieName = "UniqueUserIdentifier";
            var userCookieValue = Request.Cookies[cookieName];

            if (string.IsNullOrEmpty(userCookieValue))
            {
                return NotFound("Cookie not found.");
            }
            else
            {
                var cart = _context.Order
                        .FirstOrDefault(o => o.UserID == userCookieValue && o.IsDone == false);

                if (cart != null)
                {
                    var count = _context.CookieCart
                                        .Where(x => x.OrderId == cart.ID)
                                        .Count();
                    return new JsonResult(new
                    {

                        Count = count

                    });
                }
                return new JsonResult(new
                {
                        
                    Count = 0

                });
            }
        }
        [HttpGet]

        public IActionResult GetAllProduct(int pageNumber = 1, int pageSize = 10)
        {
            var pr = from _pr in _context.Products
                     join _cate in _context.Category on _pr.CateId equals _cate.ID
                     join _sub in _context.SubCategory on _pr.SubId equals _sub.ID
                     join _brand in _context.PrBrand on _pr.BrandId equals _brand.ID
                     join _clb in _context.PrMainCLB on _pr.CLBID equals _clb.ID
                     join _age in _context.PrAge on _pr.AgeID equals _age.ID
                     join _rim in _context.PrRim on _pr.RimID equals _rim.ID
                     select new ProductsCRUD
                     {
                         ID = _pr.ID,
                         NameProduct = _pr.NameProduct,
                         Slug = _pr.Slug,
                         BeforePrice = _pr.BeforePrice,
                         DisPrice = _pr.DisPrice,
                         CateId = _pr.CateId,
                         SubId = _pr.SubId,
                         Quantity = _pr.Quantity,
                         CreateDate = _pr.CreateDate,
                         BrandId = _pr.BrandId,
                         CLBID = _pr.CLBID,
                         AgeID = _pr.AgeID,
                         RimID = _pr.RimID,
                         ProductNumber = _pr.ProductNumber,
                         ImgProducts = _pr.ImgProducts,
                         CateName = _cate.NameCate,
                         SubName = _sub.NameSub,
                         BrandsName = _brand.NameBrand,
                         ClbName = _clb.NameMainCLB,
                         AgeName = _age.NameAge,
                         RimName = _rim.RimName
                     };

            var sortedProducts = pr.OrderByDescending(x => x.ID);
            var pagedProducts = sortedProducts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var totalRecords = _context.Products.Count();

            return new JsonResult(new
            {
                code = 200,
                status = "Success",
                Products = pagedProducts,
                Count = totalRecords

            });

        }

        [HttpGet]

        public IActionResult GetAllBrands()
        {
            var totalRecords = _context.PrBrand.Count();

            var pr = _context.PrBrand.ToList().OrderByDescending(x => x.ID);
            return new JsonResult(new
            {
                code = 200,
                status = "Success",
                Brands = pr,
                Count = totalRecords

            });
        }

        [HttpGet]

        public IActionResult GetAllClb()
        {
            var totalRecords = _context.PrClb.Count();

            var pr = _context.PrClb.ToList().OrderByDescending(x => x.ID).Take(6);
            return new JsonResult(new
            {
                code = 200,
                status = "Success",
                CLB = pr,
                Count = totalRecords

            });
        }
        [HttpGet]
        public IActionResult GetAllClbWithMain()
        {
            var mainClbs = _context.PrMainCLB.OrderByDescending(x => x.ID).ToList();
            var result = new List<object>();

            foreach (var mainClb in mainClbs)
            {
                var prClbs = _context.PrClb.Where(x => x.IDMain == mainClb.ID).ToList();
                result.Add(new
                {
                    NameMainCLB = mainClb.NameMainCLB,
                    Slug = mainClb.Slug,
                    PrClbs = prClbs
                });
            }

            return new JsonResult(new
            {
                code = 200,
                status = "Success",
                CLBs = result,
                Count = mainClbs.Count
            });
        }


        [HttpGet]

        public IActionResult GetAllClbMain()
        {
            var totalRecords = _context.PrMainCLB.Count();

            var pr = _context.PrMainCLB.ToList().OrderByDescending(x => x.ID);
            return new JsonResult(new
            {
                code = 200,
                status = "Success",
                CLB = pr,
                Count = totalRecords

            });
        }


        [HttpGet]
        public IActionResult GetAllSubCategory()
        {

            var pr = from _pr in _context.SubCategory
                     join _cate in _context.Category on _pr.CateId equals _cate.ID

                     select new
                     {
                         ID = _pr.ID,
                         CateId = _cate.ID,
                         CategoryName = _cate.NameCate,
                         NameSub = _pr.NameSub,
                         IdCate = _pr.CateId,
                         ImgSub = _pr.ImgSubCate,
                         Slug = _pr.Slug
                     };
            var total = pr.ToList().OrderByDescending(x => x.ID).Take(6);
            return new JsonResult(new
            {
                code = 200,
                status = "Success",
                SubCategories = total

            });
        }
        [HttpGet]
        public IActionResult GetAllSubCategoryWithId(int id)
        {

            var pr = from _pr in _context.Category
                     join _cate in _context.SubCategory on _pr.ID equals _cate.CateId
                     where _pr.ID == id
                     select new
                     {
                         ID = _pr.ID,
                         CateId = _cate.ID,
                         CategoryName = _pr.NameCate,
                         NameSub = _cate.NameSub,
                         IdCate = _cate.CateId,
                         ImgSub = _cate.ImgSubCate,
                         Slug = _cate.Slug
                     };
            var total = pr.ToList().OrderByDescending(x => x.ID);
            return new JsonResult(new
            {
                code = 200,
                status = "Success",
                SubCategories = total

            });
        }

        [HttpGet]
        public IActionResult GetAllCart()
        {
            const string cookieName = "UniqueUserIdentifier";
            var userCookieValue = Request.Cookies[cookieName];

            if (string.IsNullOrEmpty(userCookieValue))
            {
                return NotFound("Cookie not found.");
            }
            else
            {
                var cart = _context.Order
                        .FirstOrDefault(o => o.UserID == userCookieValue && o.IsDone == false);

                if (cart != null)
                {
                    var orderDetails = _context.CookieCart
                                        .Where(x => x.OrderId == cart.ID);
                    if (orderDetails != null)
                    {
                        List<ProductsCRUD> productList = new List<ProductsCRUD>();
                        foreach (var order in orderDetails)
                        {
                            var products = _context.Products.FirstOrDefault(x => x.ID == order.ProductID);
                            if (products != null)
                            {
                                var brand = _context.PrBrand.FirstOrDefault(x => x.ID == products.BrandId);
                                var Size = _context.PrSize.Find(order.SizeID);

                                ProductsCRUD pr = new ProductsCRUD()
                                {
                                    ID = products.ID,
                                    NameProduct = products.NameProduct,
                                    Slug = products.Slug,
                                    BeforePrice = products.BeforePrice,
                                    DisPrice = products.DisPrice,
                                    CateId = products.CateId,
                                    SubId = products.SubId,
                                    SelectSizeId = Size?.NameSize,
                                    Quantity = products.Quantity,
                                    CreateDate = products.CreateDate,
                                    BrandId = products.BrandId,
                                    CLBID = products.CLBID,
                                    AgeID = products.AgeID,
                                    RimID = products.RimID,
                                    ProductNumber = products.ProductNumber,
                                    ImgProducts = products.ImgProducts,
                                    BrandsName = brand?.NameBrand,
                                    OrderId = order.OrderId,
                                    IdDetails = order.ID,
                                    QuantityCient = order.ProductQuantity,
                                    TotalPrices = order.TotalPay,


                                };
                                productList.Add(pr);

                            }


                        }
                        return new JsonResult(new
                        {
                            code = 200,
                            status = "Success",
                            Products = productList,
                            OrderId = cart.ID

                        });
                    }

                }
                return new JsonResult(new
                {

                    code = 203,
                    status = "False",

                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCart(int idOrder, int prId, int OrderDetails)
        {
            var order = _context.Order.FirstOrDefault(x => x.ID == idOrder && x.IsDone == false);

            if (order != null)
            {
                var orderDetails = _context.CookieCart.Where(x => x.OrderId == order.ID).FirstOrDefault(x => x.ID == OrderDetails && x.ProductID == prId);

                if (orderDetails != null)
                {
                    _context.CookieCart.Remove(orderDetails);
                    await _context.SaveChangesAsync();
                    return Ok(new { status = "Success", message = "Item removed from cart" });
                }
            }
            return NotFound(new { status = "Error", message = "Item not found in cart" });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCart(int OrderDetails, int prId, string action)
        {
            var orderDetails = _context.CookieCart.FirstOrDefault(x => x.ID == OrderDetails && x.ProductID == prId);

            if (orderDetails != null)
            {
                var product = _context.Products.FirstOrDefault(x => x.ID == prId);
                if (product != null)
                {
                    int? priceToUse = product.DisPrice.HasValue && product.DisPrice.Value > 0
                        ? product.DisPrice.Value
                        : product.BeforePrice;
                    if (action == "increase")
                    {

                        if (product.Quantity > 0)
                        {
                            product.Quantity -= 1;
                            orderDetails.ProductQuantity += 1;
                            orderDetails.TotalPay = orderDetails.ProductQuantity * priceToUse;

                        }

                    }
                    else if (action == "decrease" && orderDetails.ProductQuantity > 1)
                    {

                        orderDetails.ProductQuantity -= 1;
                        orderDetails.TotalPay = orderDetails.ProductQuantity * priceToUse;
                        product.Quantity += 1;

                    }
                    else
                    {
                        return BadRequest(new { status = "Error", message = "Invalid action or quantity" });
                    }
                    await _context.SaveChangesAsync();
                    return Ok(new { status = "Success", message = "Product quantity updated in cart" });

                }

            }
            return NotFound(new { status = "Error", message = "Item not found in cart" });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}