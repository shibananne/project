using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopHat.Data;
using ShopHat.Models;
using ShopHat.Models.ProductsViewModels;
using ShopHat.Services;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq.Expressions;
using static ShopHat.Models.ProductsViewModels.ProductsCRUD;

namespace ShopHat.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        public CategoryController(ILogger<CategoryController> logger, ApplicationDbContext context, IWebHostEnvironment env)
        {
            _logger = logger;
            _context = context;
            _env = env;

        }
        [Route("Teams/{slug}")]
        [HttpGet]
        public IActionResult Team(string slug, int pageNumber = 1, int pageSize = 10)
        {
            ViewBag.Slug = slug;
            var clbP = _context.PrClb.FirstOrDefault(x => x.Slug == slug);
            var clbMainTeam = _context.PrMainCLB.FirstOrDefault(x => x.Slug == slug);
            if(clbP != null)
            {
                ViewBag.CateName = clbP.NameCLB;
                var pr = from _pr in _context.Products
                         join _cate in _context.Category on _pr.CateId equals _cate.ID
                         join _sub in _context.SubCategory on _pr.SubId equals _sub.ID
                         join _brand in _context.PrBrand on _pr.BrandId equals _brand.ID
                         join _clb in _context.PrMainCLB on _pr.CLBID equals _clb.ID
                         join _clbMain in _context.PrClb on _clb.ID equals _clbMain.IDMain
                         join _age in _context.PrAge on _pr.AgeID equals _age.ID
                         join _rim in _context.PrRim on _pr.RimID equals _rim.ID
                         where _clbMain.Slug == slug
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
                var CountProducts = sortedProducts.Count();
                var pagedProducts = sortedProducts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                ViewBag.Count = CountProducts;

                return View(pagedProducts);
            }
            if(clbMainTeam != null)
            {
                ViewBag.CateName = clbMainTeam.NameMainCLB;
                var pr = from _pr in _context.Products
                         join _cate in _context.Category on _pr.CateId equals _cate.ID
                         join _sub in _context.SubCategory on _pr.SubId equals _sub.ID
                         join _brand in _context.PrBrand on _pr.BrandId equals _brand.ID
                         join _clb in _context.PrMainCLB on _pr.CLBID equals _clb.ID
                         join _clbMain in _context.PrClb on _clb.ID equals _clbMain.IDMain
                         join _age in _context.PrAge on _pr.AgeID equals _age.ID
                         join _rim in _context.PrRim on _pr.RimID equals _rim.ID
                         where _clb.Slug == slug
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
                var CountProducts = sortedProducts.Count();
                var pagedProducts = sortedProducts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                ViewBag.Count = CountProducts;

                return View(pagedProducts);
            }
            return View();



         

        }


        [Route("Category/{slug}")]
        [HttpGet]

        public IActionResult Index(string slug, int pageNumber = 1, int pageSize = 10)
        {
            ViewBag.Slug = slug;
            var cate = _context.SubCategory.FirstOrDefault(c => c.Slug == slug);
            var cateMain = _context.Category.FirstOrDefault(c => c.Slug == slug);
            var brands = _context.PrBrand.FirstOrDefault(c => c.Slug == slug);


            if (cate != null)
            {
                ViewBag.CateName = cate.NameSub;

                var pr = from _pr in _context.Products
                         join _cate in _context.Category on _pr.CateId equals _cate.ID
                         join _sub in _context.SubCategory on _pr.SubId equals _sub.ID
                         join _brand in _context.PrBrand on _pr.BrandId equals _brand.ID
                         join _clb in _context.PrMainCLB on _pr.CLBID equals _clb.ID
                         join _age in _context.PrAge on _pr.AgeID equals _age.ID
                         join _rim in _context.PrRim on _pr.RimID equals _rim.ID
                         where _pr.SubId == cate.ID
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
                var CountProducts = sortedProducts.Count();
                var pagedProducts = sortedProducts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                ViewBag.Count = CountProducts;

                return View(pagedProducts);

            }
            else if (cateMain != null)
            {
                ViewBag.CateName = cateMain.NameCate;

                var pr = from _pr in _context.Products
                         join _cate in _context.Category on _pr.CateId equals _cate.ID
                         join _sub in _context.SubCategory on _pr.SubId equals _sub.ID
                         join _brand in _context.PrBrand on _pr.BrandId equals _brand.ID
                         join _clb in _context.PrMainCLB on _pr.CLBID equals _clb.ID
                         join _age in _context.PrAge on _pr.AgeID equals _age.ID
                         join _rim in _context.PrRim on _pr.RimID equals _rim.ID
                         where _pr.CateId == cateMain.ID
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
                var CountProducts = sortedProducts.Count();
                var pagedProducts = sortedProducts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                ViewBag.Count = CountProducts;

                return View(pagedProducts);
            }
            else if (slug == "top-list")
            {
                ViewBag.CateName = "Top List";

                ViewBag.Count = _context.Products.Where(x => x.ProductsCount > 100).Count();

            }
            else if (slug == "new-in-stock")
            {
                ViewBag.CateName = "Mới";

                DateTime threeMonthsAgo = DateTime.Now.AddMonths(-3);

                var pr = from _pr in _context.Products
                         join _cate in _context.Category on _pr.CateId equals _cate.ID
                         join _sub in _context.SubCategory on _pr.SubId equals _sub.ID
                         join _brand in _context.PrBrand on _pr.BrandId equals _brand.ID
                         join _clb in _context.PrMainCLB on _pr.CLBID equals _clb.ID
                         join _age in _context.PrAge on _pr.AgeID equals _age.ID
                         join _rim in _context.PrRim on _pr.RimID equals _rim.ID
                         where _pr.CreateDate >= threeMonthsAgo
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
                var CountProducts = sortedProducts.Count();
                var pagedProducts = sortedProducts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                ViewBag.Count = CountProducts;
                return View(pagedProducts);

            }
            else if (slug == "sale")
            {
                ViewBag.CateName = "Giảm giá";


                var pr = from _pr in _context.Products
                         join _cate in _context.Category on _pr.CateId equals _cate.ID
                         join _sub in _context.SubCategory on _pr.SubId equals _sub.ID
                         join _brand in _context.PrBrand on _pr.BrandId equals _brand.ID
                         join _clb in _context.PrMainCLB on _pr.CLBID equals _clb.ID
                         join _age in _context.PrAge on _pr.AgeID equals _age.ID
                         join _rim in _context.PrRim on _pr.RimID equals _rim.ID
                         where _pr.DisPrice != null
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
                var CountProducts = sortedProducts.Count();
                var pagedProducts = sortedProducts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                ViewBag.Count = CountProducts;
                return View(pagedProducts);

            }
            else if (brands != null)
            {
                ViewBag.CateName = brands.NameBrand;


                var pr = from _pr in _context.Products
                         join _cate in _context.Category on _pr.CateId equals _cate.ID
                         join _sub in _context.SubCategory on _pr.SubId equals _sub.ID
                         join _brand in _context.PrBrand on _pr.BrandId equals _brand.ID
                         join _clb in _context.PrMainCLB on _pr.CLBID equals _clb.ID
                         join _age in _context.PrAge on _pr.AgeID equals _age.ID
                         join _rim in _context.PrRim on _pr.RimID equals _rim.ID
                         where _pr.BrandId == brands.ID

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
                var CountProducts = sortedProducts.Count();
                var pagedProducts = sortedProducts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                ViewBag.Count = CountProducts;
                return View(pagedProducts);
            }
            else if (slug == "teams")
            {
                ViewBag.CateName = "Đội bóng";


                var pr = from _pr in _context.Products
                         join _cate in _context.Category on _pr.CateId equals _cate.ID
                         join _sub in _context.SubCategory on _pr.SubId equals _sub.ID
                         join _brand in _context.PrBrand on _pr.BrandId equals _brand.ID
                         join _clb in _context.PrMainCLB on _pr.CLBID equals _clb.ID
                         join _age in _context.PrAge on _pr.AgeID equals _age.ID
                         join _rim in _context.PrRim on _pr.RimID equals _rim.ID
                         where _pr.CLBID != null

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
                var CountProducts = sortedProducts.Count();
                var pagedProducts = sortedProducts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                ViewBag.Count = CountProducts;
                return View(pagedProducts);

            }

            return View();

        }
        //[HttpGet]
        //[Route("CategoryAll/{slug}")]

        //public IActionResult GetAllWithSub(string slug)
        //{
        //    var cate = _context.SubCategory.FirstOrDefault(c => c.Slug == slug);
        //    if (cate != null)
        //    {
        //        var pr = from _pr in _context.Products
        //                 join _cate in _context.Category on _pr.CateId equals _cate.ID
        //                 join _sub in _context.SubCategory on _pr.SubId equals _sub.ID
        //                 join _brand in _context.PrBrand on _pr.BrandId equals _brand.ID
        //                 join _clb in _context.PrMainCLB on _pr.CLBID equals _clb.ID
        //                 join _age in _context.PrAge on _pr.AgeID equals _age.ID
        //                 join _rim in _context.PrRim on _pr.RimID equals _rim.ID
        //                 where _pr.SubId == cate.ID
        //                 select new ProductsCRUD
        //                 {
        //                     ID = _pr.ID,
        //                     NameProduct = _pr.NameProduct,
        //                     Slug = _pr.Slug,
        //                     BeforePrice = _pr.BeforePrice,
        //                     DisPrice = _pr.DisPrice,
        //                     CateId = _pr.CateId,
        //                     SubId = _pr.SubId,
        //                     Quantity = _pr.Quantity,
        //                     CreateDate = _pr.CreateDate,
        //                     BrandId = _pr.BrandId,
        //                     CLBID = _pr.CLBID,
        //                     AgeID = _pr.AgeID,
        //                     RimID = _pr.RimID,
        //                     ProductNumber = _pr.ProductNumber,
        //                     ImgProducts = _pr.ImgProducts,
        //                     CateName = _cate.NameCate,
        //                     SubName = _sub.NameSub,
        //                     BrandsName = _brand.NameBrand,
        //                     ClbName = _clb.NameMainCLB,
        //                     AgeName = _age.NameAge,
        //                     RimName = _rim.RimName
        //                 };

        //        var sortedProducts = pr.OrderByDescending(x => x.ID).ToList();
        //        var brandIds = sortedProducts.Select(p => p.BrandId).Distinct().ToList();
        //        var ageIds = sortedProducts.Select(p => p.AgeID).Distinct().ToList();
        //        var clbIds = sortedProducts.Select(p => p.CLBID).Distinct().ToList();
        //        var rimIds = sortedProducts.Select(p => p.RimID).Distinct().ToList();

        //        var brandData = _context.PrBrand.Where(x => brandIds.Contains(x.ID)).ToList();
        //        var AgeData = _context.PrAge.Where(x => ageIds.Contains(x.ID)).ToList();
        //        var clbData = _context.PrMainCLB.Where(x => clbIds.Contains(x.ID)).ToList();
        //        var rimData = _context.PrRim.Where(x => rimIds.Contains(x.ID)).ToList();
        //        return new JsonResult(new
        //        {
        //            code = 200,
        //            status = "Success",
        //            Products = sortedProducts,
        //            Brands = new
        //            {
        //                BrandName = "Thương Hiệu",
        //                data = brandData
        //            },
        //            Age = new
        //            {
        //                AgeName = "Độ tuổi",
        //                data = AgeData
        //            },
        //            CLB = new
        //            {
        //                ClbName = "Đội bóng",
        //                data = clbData
        //            },
        //            Rim = new
        //            {
        //                RimName = "Vành",
        //                data = rimData
        //            }

        //        });

        //    }

        //    return new JsonResult(new
        //    {
        //        code = 203,
        //        status = "False"

        //    });
        //}
        [HttpGet]
        [Route("Clb/{slug}")]
        public IActionResult GetAllTeam(string slug)
        {
           
            var clbP = _context.PrClb.FirstOrDefault(x => x.Slug == slug);
            var clbMain = _context.PrMainCLB.FirstOrDefault(x => x.Slug == slug);

            if (clbP != null)
            {
                var sortedProducts = GetProductsByTeam(slug);
                var filtersData = GetFiltersData(sortedProducts);

                return new JsonResult(new
                {
                    code = 200,
                    status = "Success",
                    Products = sortedProducts,
                    Brands = filtersData.Brands,
                    Age = filtersData.Age,
                    CLB = filtersData.CLB,
                    Rim = filtersData.Rim
                });
            }
            else if(clbMain != null)
            {
                var sortedProducts = GetProductsByTeamMain(slug);
                var filtersData = GetFiltersData(sortedProducts);

                return new JsonResult(new
                {
                    code = 200,
                    status = "Success",
                    Products = sortedProducts,
                    Brands = filtersData.Brands,
                    Age = filtersData.Age,
                    CLB = filtersData.CLB,
                    Rim = filtersData.Rim
                });
            }
            return new JsonResult(new
            {
                code = 203,
                status = "False"
            });
        }

        [HttpGet]
        [Route("CategoryAll/{slug}")]
        public IActionResult GetAllWithSub(string slug)
        {
            var cate = _context.SubCategory.FirstOrDefault(c => c.Slug == slug);
            var cateMain = _context.Category.FirstOrDefault(c => c.Slug == slug);
            var brands = _context.PrBrand.FirstOrDefault(c => c.Slug == slug);

            if (cate != null)
            {
                var sortedProducts = GetProductsByCategory(cate);
                var filtersData = GetFiltersData(sortedProducts);

                return new JsonResult(new
                {
                    code = 200,
                    status = "Success",
                    Products = sortedProducts,
                    Brands = filtersData.Brands,
                    Age = filtersData.Age,
                    CLB = filtersData.CLB,
                    Rim = filtersData.Rim
                });
            }
            else if (cateMain != null)
            {
                var sortedProducts = GetProductsByCategoryMain(cateMain);
                var filtersData = GetFiltersData(sortedProducts);

                return new JsonResult(new
                {
                    code = 200,
                    status = "Success",
                    Products = sortedProducts,
                    Brands = filtersData.Brands,
                    Age = filtersData.Age,
                    CLB = filtersData.CLB,
                    Rim = filtersData.Rim
                });
            }
            else if (slug == "top-list")
            {
                var sortedProducts = GetProductsByTopList();
                var filtersData = GetFiltersData(sortedProducts);

                return new JsonResult(new
                {
                    code = 200,
                    status = "Success",
                    Products = sortedProducts,
                    Brands = filtersData.Brands,
                    Age = filtersData.Age,
                    CLB = filtersData.CLB,
                    Rim = filtersData.Rim
                });
            }
            else if (slug == "new-in-stock")
            {
                var sortedProducts = GetProductsByNewList();
                var filtersData = GetFiltersData(sortedProducts);

                return new JsonResult(new
                {
                    code = 200,
                    status = "Success",
                    Products = sortedProducts,
                    Brands = filtersData.Brands,
                    Age = filtersData.Age,
                    CLB = filtersData.CLB,
                    Rim = filtersData.Rim
                });
            }
            else if (slug == "sale")
            {
                var sortedProducts = GetProductsBySale();
                var filtersData = GetFiltersData(sortedProducts);

                return new JsonResult(new
                {
                    code = 200,
                    status = "Success",
                    Products = sortedProducts,
                    Brands = filtersData.Brands,
                    Age = filtersData.Age,
                    CLB = filtersData.CLB,
                    Rim = filtersData.Rim
                });
            }
            else if (brands != null)
            {
                var sortedProducts = GetProductsByBrands(brands);
                var filtersData = GetFiltersData(sortedProducts);

                return new JsonResult(new
                {
                    code = 200,
                    status = "Success",
                    Products = sortedProducts,
                    Brands = filtersData.Brands,
                    Age = filtersData.Age,
                    CLB = filtersData.CLB,
                    Rim = filtersData.Rim
                });
            }
            else if (slug == "teams")
            {
                var sortedProducts = GetProductsByCLB();
                var filtersData = GetFiltersData(sortedProducts);

                return new JsonResult(new
                {
                    code = 200,
                    status = "Success",
                    Products = sortedProducts,
                    Brands = filtersData.Brands,
                    Age = filtersData.Age,
                    CLB = filtersData.CLB,
                    Rim = filtersData.Rim
                });
            }

                return new JsonResult(new
            {
                code = 203,
                status = "False"
            });
        }

        [HttpGet]
        [Route("Brands/{slug}")]
        public IActionResult GetAllWithBrands(string slug)
        {
            var cate = _context.PrBrand.FirstOrDefault(c => c.Slug == slug);
            if (cate != null)
            {
                var sortedProducts = GetProductsByBrands(cate);
                var filtersData = GetFiltersData(sortedProducts);

                return new JsonResult(new
                {
                    code = 200,
                    status = "Success",
                    Products = sortedProducts,
                    Brands = filtersData.Brands,
                    Age = filtersData.Age,
                    CLB = filtersData.CLB,
                    Rim = filtersData.Rim
                });
            }
            return new JsonResult(new
            {
                code = 203,
                status = "False"
            });

        }
        private List<ProductsCRUD> GetProductsByCategory(SubCategory cate)
        {
            return (from _pr in _context.Products
                    join _cate in _context.Category on _pr.CateId equals _cate.ID
                    join _sub in _context.SubCategory on _pr.SubId equals _sub.ID
                    join _brand in _context.PrBrand on _pr.BrandId equals _brand.ID
                    join _clb in _context.PrMainCLB on _pr.CLBID equals _clb.ID
                    join _age in _context.PrAge on _pr.AgeID equals _age.ID
                    join _rim in _context.PrRim on _pr.RimID equals _rim.ID
                    where _pr.SubId == cate.ID
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
                        Brand = new BrandObj
                        {
                            Id = _pr.BrandId,
                            Name = _brand.NameBrand
                        },
                        Clb = new ClbObj
                        {
                            Id = _pr.CLBID,
                            Name = _clb.NameMainCLB
                        },
                        Age = new AgeObj
                        {
                            Id = _pr.AgeID,
                            Name = _age.NameAge
                        },
                        Rim = new RimObj
                        {
                            Id = _pr.RimID,
                            Name = _rim.RimName
                        },
                        ProductNumber = _pr.ProductNumber,
                        ImgProducts = _pr.ImgProducts,
                        CateName = _cate.NameCate,
                        SubName = _sub.NameSub,
                        BrandsName = _brand.NameBrand,
                        ClbName = _clb.NameMainCLB,
                        AgeName = _age.NameAge,
                        RimName = _rim.RimName
                    }).OrderByDescending(x => x.ID).ToList();
        } 
        private List<ProductsCRUD> GetProductsByCLB()
        {
            return (from _pr in _context.Products
                    join _cate in _context.Category on _pr.CateId equals _cate.ID
                    join _sub in _context.SubCategory on _pr.SubId equals _sub.ID
                    join _brand in _context.PrBrand on _pr.BrandId equals _brand.ID
                    join _clb in _context.PrMainCLB on _pr.CLBID equals _clb.ID
                    join _age in _context.PrAge on _pr.AgeID equals _age.ID
                    join _rim in _context.PrRim on _pr.RimID equals _rim.ID
                     where _pr.CLBID != null
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
                        Brand = new BrandObj
                        {
                            Id = _pr.BrandId,
                            Name = _brand.NameBrand
                        },
                        Clb = new ClbObj
                        {
                            Id = _pr.CLBID,
                            Name = _clb.NameMainCLB
                        },
                        Age = new AgeObj
                        {
                            Id = _pr.AgeID,
                            Name = _age.NameAge
                        },
                        Rim = new RimObj
                        {
                            Id = _pr.RimID,
                            Name = _rim.RimName
                        },
                        ProductNumber = _pr.ProductNumber,
                        ImgProducts = _pr.ImgProducts,
                        CateName = _cate.NameCate,
                        SubName = _sub.NameSub,
                        BrandsName = _brand.NameBrand,
                        ClbName = _clb.NameMainCLB,
                        AgeName = _age.NameAge,
                        RimName = _rim.RimName
                    }).OrderByDescending(x => x.ID).ToList();
        }
        private List<ProductsCRUD> GetProductsByCategoryMain(Category cate)
        {
            return (from _pr in _context.Products
                    join _cate in _context.Category on _pr.CateId equals _cate.ID
                    join _sub in _context.SubCategory on _pr.SubId equals _sub.ID
                    join _brand in _context.PrBrand on _pr.BrandId equals _brand.ID
                    join _clb in _context.PrMainCLB on _pr.CLBID equals _clb.ID
                    join _age in _context.PrAge on _pr.AgeID equals _age.ID
                    join _rim in _context.PrRim on _pr.RimID equals _rim.ID
                    where _pr.CateId == cate.ID
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
                        Brand = new BrandObj
                        {
                            Id = _pr.BrandId,
                            Name = _brand.NameBrand
                        },
                        Clb = new ClbObj
                        {
                            Id = _pr.CLBID,
                            Name = _clb.NameMainCLB
                        },
                        Age = new AgeObj
                        {
                            Id = _pr.AgeID,
                            Name = _age.NameAge
                        },
                        Rim = new RimObj
                        {
                            Id = _pr.RimID,
                            Name = _rim.RimName
                        },
                        ProductNumber = _pr.ProductNumber,
                        ImgProducts = _pr.ImgProducts,
                        CateName = _cate.NameCate,
                        SubName = _sub.NameSub,
                        BrandsName = _brand.NameBrand,
                        ClbName = _clb.NameMainCLB,
                        AgeName = _age.NameAge,
                        RimName = _rim.RimName
                    }).OrderByDescending(x => x.ID).ToList();
        }
        private List<ProductsCRUD> GetProductsByTeam(string slug)
        {
            return (from _pr in _context.Products
                    join _cate in _context.Category on _pr.CateId equals _cate.ID
                    join _sub in _context.SubCategory on _pr.SubId equals _sub.ID
                    join _brand in _context.PrBrand on _pr.BrandId equals _brand.ID
                    join _clb in _context.PrMainCLB on _pr.CLBID equals _clb.ID
                    join _clbMain in _context.PrClb on _clb.ID equals _clbMain.IDMain

                    join _age in _context.PrAge on _pr.AgeID equals _age.ID
                    join _rim in _context.PrRim on _pr.RimID equals _rim.ID
                    where _clbMain.Slug == slug

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
                        Brand = new BrandObj
                        {
                            Id = _pr.BrandId,
                            Name = _brand.NameBrand
                        },
                        Clb = new ClbObj
                        {
                            Id = _pr.CLBID,
                            Name = _clb.NameMainCLB
                        },
                        Age = new AgeObj
                        {
                            Id = _pr.AgeID,
                            Name = _age.NameAge
                        },
                        Rim = new RimObj
                        {
                            Id = _pr.RimID,
                            Name = _rim.RimName
                        },
                        ProductNumber = _pr.ProductNumber,
                        ImgProducts = _pr.ImgProducts,
                        CateName = _cate.NameCate,
                        SubName = _sub.NameSub,
                        BrandsName = _brand.NameBrand,
                        ClbName = _clb.NameMainCLB,
                        AgeName = _age.NameAge,
                        RimName = _rim.RimName
                    }).OrderByDescending(x => x.ID).ToList();
        } 
        private List<ProductsCRUD> GetProductsByTeamMain(string slug)
        {
            return (from _pr in _context.Products
                    join _cate in _context.Category on _pr.CateId equals _cate.ID
                    join _sub in _context.SubCategory on _pr.SubId equals _sub.ID
                    join _brand in _context.PrBrand on _pr.BrandId equals _brand.ID
                    join _clb in _context.PrMainCLB on _pr.CLBID equals _clb.ID
                    join _clbMain in _context.PrClb on _clb.ID equals _clbMain.IDMain
                    join _age in _context.PrAge on _pr.AgeID equals _age.ID
                    join _rim in _context.PrRim on _pr.RimID equals _rim.ID
                    where _clb.Slug == slug

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
                        Brand = new BrandObj
                        {
                            Id = _pr.BrandId,
                            Name = _brand.NameBrand
                        },
                        Clb = new ClbObj
                        {
                            Id = _pr.CLBID,
                            Name = _clb.NameMainCLB
                        },
                        Age = new AgeObj
                        {
                            Id = _pr.AgeID,
                            Name = _age.NameAge
                        },
                        Rim = new RimObj
                        {
                            Id = _pr.RimID,
                            Name = _rim.RimName
                        },
                        ProductNumber = _pr.ProductNumber,
                        ImgProducts = _pr.ImgProducts,
                        CateName = _cate.NameCate,
                        SubName = _sub.NameSub,
                        BrandsName = _brand.NameBrand,
                        ClbName = _clb.NameMainCLB,
                        AgeName = _age.NameAge,
                        RimName = _rim.RimName
                    }).OrderByDescending(x => x.ID).ToList();
        }
        private List<ProductsCRUD> GetProductsByNewList()
        {
            DateTime threeMonthsAgo = DateTime.Now.AddMonths(-3);
            return (from _pr in _context.Products
                    join _cate in _context.Category on _pr.CateId equals _cate.ID
                    join _sub in _context.SubCategory on _pr.SubId equals _sub.ID
                    join _brand in _context.PrBrand on _pr.BrandId equals _brand.ID
                    join _clb in _context.PrMainCLB on _pr.CLBID equals _clb.ID
                    join _age in _context.PrAge on _pr.AgeID equals _age.ID
                    join _rim in _context.PrRim on _pr.RimID equals _rim.ID
                    where _pr.CreateDate >= threeMonthsAgo
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
                        Brand = new BrandObj
                        {
                            Id = _pr.BrandId,
                            Name = _brand.NameBrand
                        },
                        Clb = new ClbObj
                        {
                            Id = _pr.CLBID,
                            Name = _clb.NameMainCLB
                        },
                        Age = new AgeObj
                        {
                            Id = _pr.AgeID,
                            Name = _age.NameAge
                        },
                        Rim = new RimObj
                        {
                            Id = _pr.RimID,
                            Name = _rim.RimName
                        },
                        ProductNumber = _pr.ProductNumber,
                        ImgProducts = _pr.ImgProducts,
                        CateName = _cate.NameCate,
                        SubName = _sub.NameSub,
                        BrandsName = _brand.NameBrand,
                        ClbName = _clb.NameMainCLB,
                        AgeName = _age.NameAge,
                        RimName = _rim.RimName
                    }).OrderByDescending(x => x.ID).ToList();
        }
        private List<ProductsCRUD> GetProductsByTopList()
        {
            return (from _pr in _context.Products
                    join _cate in _context.Category on _pr.CateId equals _cate.ID
                    join _sub in _context.SubCategory on _pr.SubId equals _sub.ID
                    join _brand in _context.PrBrand on _pr.BrandId equals _brand.ID
                    join _clb in _context.PrMainCLB on _pr.CLBID equals _clb.ID
                    join _age in _context.PrAge on _pr.AgeID equals _age.ID
                    join _rim in _context.PrRim on _pr.RimID equals _rim.ID
                    where _pr.ProductsCount > 100
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
                        Brand = new BrandObj
                        {
                            Id = _pr.BrandId,
                            Name = _brand.NameBrand
                        },
                        Clb = new ClbObj
                        {
                            Id = _pr.CLBID,
                            Name = _clb.NameMainCLB
                        },
                        Age = new AgeObj
                        {
                            Id = _pr.AgeID,
                            Name = _age.NameAge
                        },
                        Rim = new RimObj
                        {
                            Id = _pr.RimID,
                            Name = _rim.RimName
                        },
                        ProductNumber = _pr.ProductNumber,
                        ImgProducts = _pr.ImgProducts,
                        CateName = _cate.NameCate,
                        SubName = _sub.NameSub,
                        BrandsName = _brand.NameBrand,
                        ClbName = _clb.NameMainCLB,
                        AgeName = _age.NameAge,
                        RimName = _rim.RimName
                    }).OrderByDescending(x => x.ID).ToList();
        }
        private List<ProductsCRUD> GetProductsBySale()
        {
            return (from _pr in _context.Products
                    join _cate in _context.Category on _pr.CateId equals _cate.ID
                    join _sub in _context.SubCategory on _pr.SubId equals _sub.ID
                    join _brand in _context.PrBrand on _pr.BrandId equals _brand.ID
                    join _clb in _context.PrMainCLB on _pr.CLBID equals _clb.ID
                    join _age in _context.PrAge on _pr.AgeID equals _age.ID
                    join _rim in _context.PrRim on _pr.RimID equals _rim.ID
                    where _pr.DisPrice != null

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
                        Brand = new BrandObj
                        {
                            Id = _pr.BrandId,
                            Name = _brand.NameBrand
                        },
                        Clb = new ClbObj
                        {
                            Id = _pr.CLBID,
                            Name = _clb.NameMainCLB
                        },
                        Age = new AgeObj
                        {
                            Id = _pr.AgeID,
                            Name = _age.NameAge
                        },
                        Rim = new RimObj
                        {
                            Id = _pr.RimID,
                            Name = _rim.RimName
                        },
                        ProductNumber = _pr.ProductNumber,
                        ImgProducts = _pr.ImgProducts,
                        CateName = _cate.NameCate,
                        SubName = _sub.NameSub,
                        BrandsName = _brand.NameBrand,
                        ClbName = _clb.NameMainCLB,
                        AgeName = _age.NameAge,
                        RimName = _rim.RimName
                    }).OrderByDescending(x => x.ID).ToList();
        }

        private List<ProductsCRUD> GetProductsByBrands(PrBrand brand)
        {
            return (from _pr in _context.Products
                    join _cate in _context.Category on _pr.CateId equals _cate.ID
                    join _sub in _context.SubCategory on _pr.SubId equals _sub.ID
                    join _brand in _context.PrBrand on _pr.BrandId equals _brand.ID
                    join _clb in _context.PrMainCLB on _pr.CLBID equals _clb.ID
                    join _age in _context.PrAge on _pr.AgeID equals _age.ID
                    join _rim in _context.PrRim on _pr.RimID equals _rim.ID
                    where _pr.BrandId == brand.ID
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
                        Brand = new BrandObj
                        {
                            Id = _pr.BrandId,
                            Name = _brand.NameBrand
                        },
                        Clb = new ClbObj
                        {
                            Id = _pr.CLBID,
                            Name = _clb.NameMainCLB
                        },
                        Age = new AgeObj
                        {
                            Id = _pr.AgeID,
                            Name = _age.NameAge
                        },
                        Rim = new RimObj
                        {
                            Id = _pr.RimID,
                            Name = _rim.RimName
                        },
                        ProductNumber = _pr.ProductNumber,
                        ImgProducts = _pr.ImgProducts,
                        CateName = _cate.NameCate,
                        SubName = _sub.NameSub,
                        BrandsName = _brand.NameBrand,
                        ClbName = _clb.NameMainCLB,
                        AgeName = _age.NameAge,
                        RimName = _rim.RimName
                    }).OrderByDescending(x => x.ID).ToList();
        }
        private (dynamic Brands, dynamic Age, dynamic CLB, dynamic Rim) GetFiltersData(List<ProductsCRUD> products)
        {
            var brandData = GetDataFromTable<PrBrand>(products, p => p.BrandId, "ID");
            var ageData = GetDataFromTable<PrAge>(products, p => p.AgeID, "ID");
            var clbData = GetDataFromTable<PrMainCLB>(products, p => p.CLBID, "ID");
            var rimData = GetDataFromTable<PrRim>(products, p => p.RimID, "ID");

            return (
                Brands: new { BrandName = "Thương Hiệu", data = brandData },
                Age: new { AgeName = "Độ tuổi", data = ageData },
                CLB: new { ClbName = "Đội bóng", data = clbData },
                Rim: new { RimName = "Vành", data = rimData }
            );
        }

        private List<T> GetDataFromTable<T>(List<ProductsCRUD> products, Func<ProductsCRUD, int?> selector, string idPropertyName) where T : class
        {
            var ids = products.Select(selector).Where(id => id.HasValue).Distinct().Select(id => id.Value).ToList();

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, idPropertyName);
            var containsMethod = typeof(List<int>).GetMethod("Contains");
            var containsExpression = Expression.Call(Expression.Constant(ids), containsMethod, property);

            var lambda = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);

            return _context.Set<T>().Where(lambda).ToList();
        }


        [Route("Menu/{slug}")]
        [HttpGet]

        public IActionResult ProductWithSubMenu(string slug, int pageNumber = 1, int pageSize = 10)
        {
            var cate = _context.SubCategory.FirstOrDefault(c => c.Slug == slug);
            if (cate != null)
            {
                var pr = from _pr in _context.Products
                         join _cate in _context.Category on _pr.CateId equals _cate.ID
                         join _sub in _context.SubCategory on _pr.SubId equals _sub.ID
                         join _brand in _context.PrBrand on _pr.BrandId equals _brand.ID
                         join _clb in _context.PrMainCLB on _pr.CLBID equals _clb.ID
                         join _age in _context.PrAge on _pr.AgeID equals _age.ID
                         join _rim in _context.PrRim on _pr.RimID equals _rim.ID
                         where _pr.SubId == cate.ID
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
                return new JsonResult(new
                {
                    code = 200,
                    status = "Success",
                    Products = pagedProducts,

                });
            }

            return new JsonResult(new
            {
                code = 400,
                status = "False",

            });
        }
        public IActionResult Privacy()
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}