using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;
using ShopHat.Data;
using ShopHat.Models;
using ShopHat.Models.PrBrandViewModels;
using ShopHat.Models.PrClbViewModels;
using ShopHat.Models.ProductsViewModels;
using ShopHat.Models.SubCategoryViewModels;
using ShopHat.Services;

namespace ShopHat.Areas.AdminShopHat.Controllers
{
    [Area("AdminShopHat")]

    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly IWebHostEnvironment _env;

        public ProductsController(ApplicationDbContext context, ICommon common, IWebHostEnvironment env)
        {
            _context = context;
            _iCommon = common;
            _env = env;
        }


        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Category()
        {
            return View();
        }
        public IActionResult SubCategory()
        {
            return View();
        }
        public IActionResult CLB()
        {
            return View();
        }
        public IActionResult SubCLB()
        {
            return View();
        }
        public IActionResult Brands()
        {
            return View();
        }
        public IActionResult UserOrder()
        {
            return View();
        } 
        public IActionResult ColorMain()
        {
            return View();
        }

        [AllowAnonymous]
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

            return Ok(pr.ToList().OrderByDescending(x => x.ID));
        }
        [HttpGet]
        public async Task<IActionResult> getIdSubMenu(int id)
        {
            try
            {
                SubCategory vm = new SubCategory();
                if (id > 0)
                {
                    try
                    {
                        vm = await _context.SubCategory.FirstOrDefaultAsync(x => x.ID == id);

                        if (vm == null)
                        {
                            return BadRequest("Không tìm thấy đối tượng với ID tương ứng");
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }

                }
                else
                {
                    return NotFound();
                }


                return Ok(vm);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddSubMenu(SubCategoryCRUD model)
        {
            try
            {
                SubCategory sub = new SubCategory();
                if (model.PrPath != null)
                {
                    var PrPath = await _iCommon.UploadedFile(model.PrPath);
                    sub.ImgSubCate = "/upload/" + PrPath;
                }
                sub.NameSub = model.NameSub;
                sub.CateId = model.CateId;
                sub.Slug = model.Slug;
                _context.SubCategory.Add(sub);
                await _context.SaveChangesAsync();
                return Ok(sub);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpPost]
        public async Task<IActionResult> UpdateSubMenu(SubCategoryCRUD model)
        {
            try
            {
                var existingProduct = await _context.SubCategory.FindAsync(model.ID);

                if (existingProduct == null)
                {
                    return NotFound("Không tìm thấy");
                }
                if (model.PrPath != null)
                {
                    var PrPath = await _iCommon.UploadedFile(model.PrPath);
                    existingProduct.ImgSubCate = "/upload/" + PrPath;
                }
                else
                {
                    existingProduct.ImgSubCate = existingProduct.ImgSubCate;
                }
                existingProduct.CateId = model.CateId;
                existingProduct.NameSub = model.NameSub;
                existingProduct.Slug = model.Slug;
                await _context.SaveChangesAsync();

                return Ok(existingProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSubMenu(SubCategory model)
        {
            try
            {
                var existingProduct = await _context.SubCategory.FirstOrDefaultAsync(x => x.ID == model.ID);

                if (existingProduct == null)
                {
                    return NotFound();
                }
                _context.SubCategory.Remove(existingProduct);
                await _context.SaveChangesAsync();

                return Ok(existingProduct);


            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [AllowAnonymous]
        [HttpGet]

        public IActionResult GetAllCategory()
        {
            var pr = _context.Category.ToList().OrderByDescending(x => x.ID);
            return Ok(pr);
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(Category model)
        {
            try
            {
                _context.Category.Add(model);
                await _context.SaveChangesAsync();
                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet]
        public async Task<IActionResult> getIdCategory(int id)
        {
            try
            {
                Category vm = new Category();
                if (id > 0)
                {
                    try
                    {
                        vm = await _context.Category.FirstOrDefaultAsync(x => x.ID == id);

                        if (vm == null)
                        {
                            return BadRequest("Không tìm thấy đối tượng với ID tương ứng");
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }

                }
                else
                {
                    return NotFound();
                }


                return Ok(vm);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(Category model)
        {
            try
            {
                var existingProduct = await _context.Category.FindAsync(model.ID);

                if (existingProduct == null)
                {
                    return NotFound("Không tìm thấy");
                }
                existingProduct.NameCate = model.NameCate;
                existingProduct.Slug = model.Slug;
                await _context.SaveChangesAsync();

                return Ok(existingProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(Category model)
        {
            try
            {
                var existingProduct = await _context.Category.FirstOrDefaultAsync(x => x.ID == model.ID);
                if (existingProduct == null)
                {
                    return NotFound();

                }
                _context.Category.Remove(existingProduct);
                await _context.SaveChangesAsync();

                return Ok(existingProduct);

            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [AllowAnonymous]
        [HttpGet]

        public IActionResult GetAllCLB()
        {
            var pr = _context.PrMainCLB.ToList().OrderByDescending(x => x.ID);
            return Ok(pr);
        }
        [HttpPost]
        public async Task<IActionResult> AddCLB(PrMainCLB model)
        {
            try
            {
                _context.PrMainCLB.Add(model);
                await _context.SaveChangesAsync();
                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet]
        public async Task<IActionResult> getIdCLB(int id)
        {
            try
            {
                PrMainCLB vm = new PrMainCLB();
                if (id > 0)
                {
                    try
                    {
                        vm = await _context.PrMainCLB.FirstOrDefaultAsync(x => x.ID == id);

                        if (vm == null)
                        {
                            return BadRequest("Không tìm thấy đối tượng với ID tương ứng");
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }

                }
                else
                {
                    return NotFound();
                }


                return Ok(vm);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCLB(PrMainCLB model)
        {
            try
            {
                var existingProduct = await _context.PrMainCLB.FindAsync(model.ID);

                if (existingProduct == null)
                {
                    return NotFound("Không tìm thấy");
                }
                existingProduct.NameMainCLB = model.NameMainCLB;
                existingProduct.Slug = model.Slug;
                await _context.SaveChangesAsync();

                return Ok(existingProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCLB(PrMainCLB model)
        {
            try
            {
                var existingProduct = await _context.PrMainCLB.FirstOrDefaultAsync(x => x.ID == model.ID);
                if (existingProduct == null)
                {
                    return NotFound();

                }
                _context.PrMainCLB.Remove(existingProduct);
                await _context.SaveChangesAsync();

                return Ok(existingProduct);

            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        [AllowAnonymous]
        [HttpGet]

        public IActionResult GetAllSubCLB()
        {

            var pr = from _pr in _context.PrClb
                     join _cate in _context.PrMainCLB on _pr.IDMain equals _cate.ID
                     select new
                     {
                         ID = _pr.ID,
                         PrMainCLBId = _cate.ID,
                         NameMainCLB = _cate.NameMainCLB,
                         NameCLB = _pr.NameCLB,
                         IDMain = _pr.IDMain,
                         ImgCLB = _pr.ImgCLB,
                         Slug = _pr.Slug
                     };

            return Ok(pr.ToList().OrderByDescending(x => x.ID));
        }
        [HttpGet]
        public async Task<IActionResult> getIdSubCLB(int id)
        {
            try
            {
                PrClb vm = new PrClb();
                if (id > 0)
                {
                    try
                    {
                        vm = await _context.PrClb.FirstOrDefaultAsync(x => x.ID == id);

                        if (vm == null)
                        {
                            return BadRequest("Không tìm thấy đối tượng với ID tương ứng");
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }

                }
                else
                {
                    return NotFound();
                }


                return Ok(vm);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddSubPrClb(PrClbCRUD model)
        {
            try
            {
                PrClb sub = new PrClb();
                if (model.PrPath != null)
                {
                    var PrPath = await _iCommon.UploadedFile(model.PrPath);
                    sub.ImgCLB = "/upload/" + PrPath;
                }
                sub.NameCLB = model.NameCLB;
                sub.IDMain = model.IDMain;
                sub.Slug = model.Slug;
                _context.PrClb.Add(sub);
                await _context.SaveChangesAsync();
                return Ok(sub);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpPost]
        public async Task<IActionResult> UpdateSubClb(PrClbCRUD model)
        {
            try
            {
                var existingProduct = await _context.PrClb.FindAsync(model.ID);

                if (existingProduct == null)
                {
                    return NotFound("Không tìm thấy");
                }
                if (model.PrPath != null)
                {
                    var PrPath = await _iCommon.UploadedFile(model.PrPath);
                    existingProduct.ImgCLB = "/upload/" + PrPath;
                }
                else
                {
                    existingProduct.ImgCLB = existingProduct.ImgCLB;
                }
                existingProduct.IDMain = model.IDMain;
                existingProduct.NameCLB = model.NameCLB;
                existingProduct.Slug = model.Slug;
                await _context.SaveChangesAsync();

                return Ok(existingProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSubClb(PrClb model)
        {
            try
            {
                var existingProduct = await _context.PrClb.FirstOrDefaultAsync(x => x.ID == model.ID);

                if (existingProduct == null)
                {
                    return NotFound();
                }
                _context.PrClb.Remove(existingProduct);
                await _context.SaveChangesAsync();

                return Ok(existingProduct);


            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [AllowAnonymous]
        [HttpGet]

        public IActionResult GetAllBrands()
        {
            var pr = _context.PrBrand.ToList().OrderByDescending(x => x.ID);
            return Ok(pr);
        }
        [HttpPost]
        public async Task<IActionResult> AddPrBrand(PrBrandCRUD model)
        {
            try
            {
                PrBrand sub = new PrBrand();
                if (model.PrPath != null)
                {
                    var PrPath = await _iCommon.UploadedFile(model.PrPath);
                    sub.ImgBrand = "/upload/" + PrPath;
                }
                sub.NameBrand = model.NameBrand;
                sub.Slug = model.Slug;
                _context.PrBrand.Add(sub);
                await _context.SaveChangesAsync();
                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet]
        public async Task<IActionResult> getIdPrBrand(int id)
        {
            try
            {
                PrBrand vm = new PrBrand();
                if (id > 0)
                {
                    try
                    {
                        vm = await _context.PrBrand.FirstOrDefaultAsync(x => x.ID == id);

                        if (vm == null)
                        {
                            return BadRequest("Không tìm thấy đối tượng với ID tương ứng");
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }

                }
                else
                {
                    return NotFound();
                }


                return Ok(vm);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePrBrand(PrBrandCRUD model)
        {
            try
            {
                var existingProduct = await _context.PrBrand.FindAsync(model.ID);

                if (existingProduct == null)
                {
                    return NotFound("Không tìm thấy");
                }
                if (model.PrPath != null)
                {
                    var PrPath = await _iCommon.UploadedFile(model.PrPath);
                    existingProduct.ImgBrand = "/upload/" + PrPath;
                }

                else
                {
                    existingProduct.ImgBrand = existingProduct.ImgBrand;
                }
                existingProduct.NameBrand = model.NameBrand;
                existingProduct.Slug = model.Slug;
                await _context.SaveChangesAsync();

                return Ok(existingProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletePrBrand(PrBrand model)
        {
            try
            {
                var existingProduct = await _context.PrBrand.FirstOrDefaultAsync(x => x.ID == model.ID);
                if (existingProduct == null)
                {
                    return NotFound();

                }
                _context.PrBrand.Remove(existingProduct);
                await _context.SaveChangesAsync();

                return Ok(existingProduct);

            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [AllowAnonymous]
        [HttpGet]

        public IActionResult GetAllProduct()
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
                         RimName = _rim.RimName,
                         ColorID = _pr.ColorID,
                         Material = _pr.Material
                     };

            return Ok(pr.ToList().OrderByDescending(x => x.ID));
        }
        [HttpPost]
        public async Task<IActionResult> AddProducts(ProductsCRUD model)
        {
            try
            {
                Products vm = new Products();
                if (model.PrPath != null)
                {
                    var PrPath = await _iCommon.UploadedFile(model.PrPath);
                    model.ImgProducts = "/upload/" + PrPath;
                }
                if (model.PrPathFull != null)
                {
                    List<string> uploadedPaths = new List<string>();
                    foreach (var prImg in model.PrPathFull)
                    {
                        var PrPath = await _iCommon.UploadedFile(prImg);
                        uploadedPaths.Add("/upload/" + PrPath);
                    }
                    model.ImgFull = Newtonsoft.Json.JsonConvert.SerializeObject(uploadedPaths);
                }
                if (model.SizeFull != null)
                {
                    List<string> uploadedId = new List<string>();
                    foreach (var prImg in model.SizeFull)
                    {
                        uploadedId.Add(prImg);
                    }
                    model.SizeId = Newtonsoft.Json.JsonConvert.SerializeObject(uploadedId);
                }

                vm = model;
                vm.ImgFull = model.ImgFull;
                vm.SizeId = model.SizeId;
                vm.CreateDate = DateTime.Now;
                _context.Products.Add(vm);
                await _context.SaveChangesAsync();
                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet]
        public async Task<IActionResult> getIdProducts(int id)
        {
            try
            {
                Products vm = new Products();
                if (id > 0)
                {
                    try
                    {
                        vm = await _context.Products.FirstOrDefaultAsync(x => x.ID == id);

                        if (vm == null)
                        {
                            return BadRequest("Không tìm thấy đối tượng với ID tương ứng");
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }

                }
                else
                {
                    return NotFound();
                }


                return Ok(vm);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProducts(ProductsCRUD model)
        {
            try
            {
                var existingProduct = await _context.Products.FindAsync(model.ID);
                if (existingProduct == null)
                {
                    return NotFound("Không tìm thấy");
                }
                if (model.PrPath != null)
                {
                    var PrPath = await _iCommon.UploadedFile(model.PrPath);
                    model.ImgProducts = "/upload/" + PrPath;
                    existingProduct.ImgProducts = model.ImgProducts;
                }
                else
                {
                    existingProduct.ImgProducts = existingProduct.ImgProducts;

                }
                if (model.PrPathFullEmpty == "true")
                {
                    existingProduct.ImgFull = "[]";
                }
                if (model.PrPathFull != null)
                {
                    List<string> uploadedPaths = new List<string>();
                    foreach (var prImg in model.PrPathFull)
                    {
                        var PrPath = await _iCommon.UploadedFile(prImg);
                        uploadedPaths.Add("/upload/" + PrPath);
                    }
                    model.ImgFull = Newtonsoft.Json.JsonConvert.SerializeObject(uploadedPaths);
                    existingProduct.ImgFull = model.ImgFull;

                }
                else
                {
                    existingProduct.ImgFull = existingProduct.ImgFull;

                }
                if (model.SizeFull != null)
                {
                    List<string> uploadedId = new List<string>();
                    foreach (var prImg in model.SizeFull)
                    {
                        uploadedId.Add(prImg);
                    }
                    model.SizeId = Newtonsoft.Json.JsonConvert.SerializeObject(uploadedId);
                    existingProduct.SizeId = model.SizeId;
                }
                else
                {
                    existingProduct.SizeId = existingProduct.SizeId;

                }
                existingProduct.NameProduct = model.NameProduct;
                existingProduct.ProductNumber = model.ProductNumber;
                existingProduct.BeforePrice = model.BeforePrice;
                existingProduct.DisPrice = model.DisPrice;
                existingProduct.CateId = model.CateId;
                existingProduct.SubId = model.SubId;
                existingProduct.Quantity = model.Quantity;
                existingProduct.BrandId = model.BrandId;
                existingProduct.CLBID = model.CLBID;
                existingProduct.AgeID = model.AgeID;
                existingProduct.RimID = model.RimID;
                existingProduct.Slug = model.Slug;
                existingProduct.ColorID = model.ColorID;
                existingProduct.Material = model.Material;
                existingProduct.CreateDate = DateTime.Now;
                await _context.SaveChangesAsync();

                return Ok(existingProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProducts(Products model)
        {
            try
            {
                var existingProduct = await _context.Products.FirstOrDefaultAsync(x => x.ID == model.ID);
                if (existingProduct == null)
                {
                    return NotFound();

                }
                _context.Products.Remove(existingProduct);
                await _context.SaveChangesAsync();

                return Ok(existingProduct);

            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet]
        public async Task<IActionResult> GetAllCartAdmin()
        {
            var carts = await _context.Order.Where(o => o.IsDone == true).ToListAsync();

            if (carts == null || !carts.Any())
                return Json(new { code = 203, status = "No orders found" });

            var result = new List<object>();


            foreach (var cart in carts)
            {
                var orderDetailsList = await _context.CookieCart.Where(x => x.OrderId == cart.ID).ToListAsync();

                var productList = new List<ProductsCRUD>();


                foreach (var orderDetail in orderDetailsList)
                {
                    var products = await _context.Products.FirstOrDefaultAsync(x => x.ID == orderDetail.ProductID);

                    if (products != null)
                    {
                        var brand = await _context.PrBrand.FirstOrDefaultAsync(x => x.ID == products.BrandId);
                        var size = await _context.PrSize.FindAsync(orderDetail.SizeID);

                        var pr = new ProductsCRUD()
                        {
                            ID = products.ID,
                            NameProduct = products.NameProduct,
                            Slug = products.Slug,
                            BeforePrice = products.BeforePrice,
                            DisPrice = products.DisPrice,
                            CateId = products.CateId,
                            SubId = products.SubId,
                            SelectSizeId = size?.NameSize,
                            Quantity = products.Quantity,
                            CreateDate = products.CreateDate,
                            BrandId = products.BrandId,
                            CLBID = products.CLBID,
                            AgeID = products.AgeID,
                            RimID = products.RimID,
                            ProductNumber = products.ProductNumber,
                            ImgProducts = products.ImgProducts,
                            BrandsName = brand?.NameBrand,
                            OrderId = orderDetail.OrderId,
                            IdDetails = orderDetail.ID,
                            QuantityCient = orderDetail.ProductQuantity,
                            TotalPrices = orderDetail.TotalPay,
                            CustomerName = cart?.CustomerName,
                            Phone_Number = cart?.Phone_Number,
                            Address = cart?.Address,
                            CreateOrder = cart?.CreateDate
                        };

                        productList.Add(pr);
                    }
                }

                result.Add(new
                {
                    OrderId = cart.ID,
                    Products = productList
                });
            }

            return Json(new { code = 200, status = "Success", Orders = result });
        }

    }
}
