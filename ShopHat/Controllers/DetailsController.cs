using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ShopHat.Data;
using ShopHat.Models;
using ShopHat.Models.CokieUserViewModels;
using ShopHat.Models.ProductsViewModels;
using ShopHat.Services;
using System.Diagnostics;
using System.Drawing.Printing;

namespace ShopHat.Controllers
{
    public class DetailsController : Controller
    {
        private readonly ILogger<DetailsController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        public DetailsController(ILogger<DetailsController> logger, ApplicationDbContext context, IWebHostEnvironment env)
        {
            _logger = logger;
            _context = context;
            _env = env;

        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("Details/{slug}")]
        public IActionResult DetailsProducts(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            var productDetail = (from _pr in _context.Products
                                 join _cate in _context.Category on _pr.CateId equals _cate.ID
                                 join _sub in _context.SubCategory on _pr.SubId equals _sub.ID
                                 join _brand in _context.PrBrand on _pr.BrandId equals _brand.ID
                                 join _clb in _context.PrMainCLB on _pr.CLBID equals _clb.ID
                                 join _age in _context.PrAge on _pr.AgeID equals _age.ID
                                 join _rim in _context.PrRim on _pr.RimID equals _rim.ID
                                 join _color in _context.PrColor on _pr.ColorID equals _color.ID

                                 where _pr.Slug == slug
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
                                     RimName = _rim.RimName,
                                     ImgFullList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(_pr.ImgFull ?? "[]"),
                                     BrandIcon = _brand.ImgBrand,
                                     SizeId = _pr.SizeId,
                                     ColorName = _color.NameColor,
                                     Material = _pr.Material,
                                     SlugBrand = _brand.Slug,

                                 }).FirstOrDefault();

            if (productDetail == null)
            {
                return NotFound();
            }

            return View(productDetail);
        }

        [HttpPost]
        [Route("Details/DetailsProductsColor")]

        public IActionResult DetailsProductsColor(string num)
        {
            var productDetail = (from _pr in _context.Products
                                 join _color in _context.PrColor on _pr.ColorID equals _color.ID
                                 where _pr.ProductNumber == num
                                 select new ProductsCRUD
                                 {
                                     ID = _pr.ID,
                                     NameProduct = _pr.NameProduct,
                                     Slug = _pr.Slug,
                                     ColorName = _color.NameColor,
                                     ProductNumber = _pr.ProductNumber,
                                     ImgProducts = _pr.ImgProducts,
                                     SizeId = _pr.SizeId,

                                 }).ToList();

            if (productDetail == null || !productDetail.Any())
            {
                return new JsonResult(new { code = 200, status = "False" });
            }

            var productsWithNonNullProperties = productDetail
                .Select(ToDictionaryWithNonNullProperties)
                .ToList();

            return new JsonResult(new
            {
                code = 200,
                status = "Success",
                Products = productsWithNonNullProperties
            });
        }
        public static IDictionary<string, object> ToDictionaryWithNonNullProperties<T>(T obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var dictionary = new Dictionary<string, object>();
            foreach (var property in typeof(T).GetProperties())
            {
                var value = property.GetValue(obj);
                if (value != null)
                {
                    dictionary.Add(property.Name, value);
                }
            }
            return dictionary;
        }

        [Route("Details/brands/{brands}")]
        [HttpGet]
        public IActionResult GetAllProductRelated(int brands, int? excludeProductId = null, int pageNumber = 1, int pageSize = 10)
        {
            var pr = from _pr in _context.Products
                     join _cate in _context.Category on _pr.CateId equals _cate.ID
                     join _sub in _context.SubCategory on _pr.SubId equals _sub.ID
                     join _brand in _context.PrBrand on _pr.BrandId equals _brand.ID
                     join _clb in _context.PrMainCLB on _pr.CLBID equals _clb.ID
                     join _age in _context.PrAge on _pr.AgeID equals _age.ID
                     join _rim in _context.PrRim on _pr.RimID equals _rim.ID
                     where _pr.BrandId == brands
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
                         RimName = _rim.RimName,
                         ImgFull = _pr.ImgFull,
                         ImgFullList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(_pr.ImgFull ?? "[]")
                     };
            if (excludeProductId.HasValue)
            {
                pr = pr.Where(_pr => _pr.ID != excludeProductId.Value);
            }
            var sortedProducts = pr.OrderByDescending(x => x.ID);
            var pagedProducts = sortedProducts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var totalRecords = sortedProducts.Count();

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

            var pr = _context.PrBrand.ToList().OrderByDescending(x => x.ID).Take(7);
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
        [Route("Details/AddCart")]
        [HttpPost]
        public async Task<IActionResult> AddCart(string UserCookie, int QuantityClient, int ProductID, int sizeId)
        {
            var userCart = await _context.CokieUser
                                         .FirstOrDefaultAsync(u => u.CokieUserMain == UserCookie);

            var orderUser = await _context.Order
     .Where(x => x.UserID == UserCookie)
     .OrderByDescending(x => x.ID)
     .FirstOrDefaultAsync();
            if (userCart != null)
            {
                if (orderUser == null)
                {

                    Order order = new Order
                    {
                        UserID = userCart.CokieUserMain,
                        ProductId = ProductID,
                        IsDone = false,
                        CreateDate = DateTime.Now,

                    };

                    await _context.Order.AddAsync(order);
                    await _context.SaveChangesAsync();

                    int OrderIdMain = order.ID;
                    var product = _context.Products.FirstOrDefault(p => p.ID == ProductID);
                    int? priceToUse = product.DisPrice.HasValue && product.DisPrice.Value > 0
                         ? product.DisPrice.Value
                         : product.BeforePrice;
                    CookieCart orderDetails = new CookieCart
                    {
                        OrderId = OrderIdMain,
                        ProductID = product.ID,
                        ProductQuantity = QuantityClient,
                        TotalPay = QuantityClient * priceToUse,
                        SizeID = sizeId,

                    };
                    await _context.CookieCart.AddAsync(orderDetails);
                    await _context.SaveChangesAsync();
                    return new JsonResult(new
                    {
                        code = 200,
                        status = "Success"
                    });

                }
                else if (orderUser != null && orderUser.IsDone == true)
                {
                    Order order = new Order
                    {
                        UserID = userCart.CokieUserMain,
                        ProductId = ProductID,
                        IsDone = false,
                        CreateDate = DateTime.Now,
                    };


                    await _context.Order.AddAsync(order);
                    await _context.SaveChangesAsync();

                    int OrderIdMain = order.ID;
                    var product = _context.Products.FirstOrDefault(p => p.ID == ProductID);
                    int? priceToUse = product.DisPrice.HasValue && product.DisPrice.Value > 0
                         ? product.DisPrice.Value
                         : product.BeforePrice;
                    CookieCart orderDetails = new CookieCart
                    {
                        OrderId = OrderIdMain,
                        ProductID = product.ID,
                        ProductQuantity = QuantityClient,
                        TotalPay = QuantityClient * priceToUse,
                        SizeID = sizeId,

                    };
                    await _context.CookieCart.AddAsync(orderDetails);
                    await _context.SaveChangesAsync();
                    return new JsonResult(new
                    {
                        code = 200,
                        status = "Success"
                    });
                }
                else
                {
                    int OrderIdMain = orderUser.ID;
                    var product = _context.Products.FirstOrDefault(p => p.ID == ProductID);
                    int? priceToUse = product.DisPrice.HasValue && product.DisPrice.Value > 0
                         ? product.DisPrice.Value
                         : product.BeforePrice;
                    var detailsList = _context.CookieCart.Where(p => p.OrderId == OrderIdMain).ToList();
                    var existingDetail = detailsList.FirstOrDefault(p => p.ProductID == ProductID);
                    if (existingDetail != null)
                    {
                        existingDetail.ProductQuantity += QuantityClient;
                        existingDetail.TotalPay = existingDetail.ProductQuantity * priceToUse;
                    }
                    else
                    {
                        CookieCart newDetail = new CookieCart
                        {
                            OrderId = OrderIdMain,
                            ProductID = ProductID,
                            ProductQuantity = QuantityClient,
                            TotalPay = QuantityClient * priceToUse,
                            SizeID = sizeId,

                        };
                        _context.CookieCart.Add(newDetail);
                    }
                    await _context.SaveChangesAsync();
                    return new JsonResult(new
                    {
                        code = 200,
                        status = "Success"
                    });
                }
            }


            return new JsonResult(new
            {
                code = 203,
                status = "False"
            });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}