using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using ShopHat.Data;
using ShopHat.Models;
using ShopHat.Models.PrColorViewModels;
using ShopHat.Models.PrModelViewModels;
using ShopHat.Models.PrRimViewModels;
using ShopHat.Models.PrSizeViewModels;
using ShopHat.Models.TermsPrViewModels;
using ShopHat.Services;

namespace ShopHat.Areas.AdminShopHat.Controllers
{
    [Area("AdminShopHat")]

    public class ConfigController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly IWebHostEnvironment _env;

        public ConfigController(ApplicationDbContext context, ICommon common, IWebHostEnvironment env)
        {
            _context = context;
            _iCommon = common;
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SizeConfig()
        {
            return View();
        }
        public IActionResult MainPage()
        {
            return View();
        }
        public IActionResult Terms()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult ReturnProducts()
        {
            return View();
        }
        public IActionResult Coupon()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult UploadLocalMain(List<IFormFile> files, [FromServices] IUrlHelperFactory urlHelperFactory)
        {
            var filePaths = new List<string>();

            foreach (IFormFile photo in Request.Form.Files)
            {
                string sv = Path.Combine(_env.WebRootPath, "upload", photo.FileName);
                using (var stream = new FileStream(sv, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }
                string relativePath = $"~/upload/{photo.FileName}";
                string absolutePath = Url.Content(relativePath);

                filePaths.Add(absolutePath);
            }

            return Json(new { urls = filePaths });
        }

        [AllowAnonymous]
        [HttpGet]

        public IActionResult GetAllAge()
        {
            var pr = _context.PrAge.ToList().OrderByDescending(x => x.ID);
            return Ok(pr);
        }
        [HttpPost]
        public async Task<IActionResult> AddAge(PrAge model)
        {
            try
            {
                _context.PrAge.Add(model);
                await _context.SaveChangesAsync();
                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet]
        public async Task<IActionResult> getIdAge(int id)
        {
            try
            {
                PrAge vm = new PrAge();
                if (id > 0)
                {
                    try
                    {
                        vm = await _context.PrAge.FirstOrDefaultAsync(x => x.ID == id);

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
        public async Task<IActionResult> UpdateAge(PrAge model)
        {
            try
            {
                var existingProduct = await _context.PrAge.FindAsync(model.ID);

                if (existingProduct == null)
                {
                    return NotFound("Không tìm thấy");
                }
                existingProduct.NameAge = model.NameAge;
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

        public IActionResult GetAllRim()
        {
            var pr = _context.PrRim.ToList().OrderByDescending(x => x.ID);
            return Ok(pr);
        }
        [HttpPost]
        public async Task<IActionResult> AddPrRim(PrRimCRUD model)
        {
            try
            {
                _context.PrRim.Add(model);
                await _context.SaveChangesAsync();
                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet]
        public async Task<IActionResult> getIdPrRim(int id)
        {
            try
            {
                PrRim vm = new PrRim();
                if (id > 0)
                {
                    try
                    {
                        vm = await _context.PrRim.FirstOrDefaultAsync(x => x.ID == id);

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
        public async Task<IActionResult> UpdatePrRim(PrRim model)
        {
            try
            {
                var existingProduct = await _context.PrRim.FindAsync(model.ID);

                if (existingProduct == null)
                {
                    return NotFound("Không tìm thấy");
                }
                existingProduct.RimName = model.RimName;
                await _context.SaveChangesAsync();

                return Ok(existingProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletePrRim(PrRim model)
        {
            try
            {
                var existingProduct = await _context.PrRim.FirstOrDefaultAsync(x => x.ID == model.ID);
                if (existingProduct == null)
                {
                    return NotFound();

                }
                _context.PrRim.Remove(existingProduct);
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

        public IActionResult GetAllSize()
        {
            var pr = _context.PrSize.ToList().OrderByDescending(x => x.ID);
            return Ok(pr);
        }
        [HttpPost]
        public async Task<IActionResult> AddPrSize(PrSizeCRUD model)
        {
            try
            {
                _context.PrSize.Add(model);
                await _context.SaveChangesAsync();
                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet]
        public async Task<IActionResult> getIdPrSize(int id)
        {
            try
            {
                PrSize vm = new PrSize();
                if (id > 0)
                {
                    try
                    {
                        vm = await _context.PrSize.FirstOrDefaultAsync(x => x.ID == id);

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
        public async Task<IActionResult> UpdatePrSize(PrSize model)
        {
            try
            {
                var existingProduct = await _context.PrSize.FindAsync(model.ID);

                if (existingProduct == null)
                {
                    return NotFound("Không tìm thấy");
                }
                existingProduct.NameSize = model.NameSize;
                await _context.SaveChangesAsync();

                return Ok(existingProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletePrSize(PrSize model)
        {
            try
            {
                var existingProduct = await _context.PrSize.FirstOrDefaultAsync(x => x.ID == model.ID);
                if (existingProduct == null)
                {
                    return NotFound();

                }
                _context.PrSize.Remove(existingProduct);
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

        public IActionResult GetAllColor()
        {
            var pr = _context.PrColor.ToList().OrderByDescending(x => x.ID);
            return Ok(pr);
        }
        [HttpPost]
        public async Task<IActionResult> AddPrColor(PrColorCRUD model)
        {
            try
            {
                if (model.PrPath != null)
                {
                    var PrPath = await _iCommon.UploadedFile(model.PrPath);
                    model.ImgColor = "/upload/" + PrPath;
                }
                _context.PrColor.Add(model);
                await _context.SaveChangesAsync();
                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet]
        public async Task<IActionResult> getIdPrColor(int id)
        {
            try
            {
                PrColor vm = new PrColor();
                if (id > 0)
                {
                    try
                    {
                        vm = await _context.PrColor.FirstOrDefaultAsync(x => x.ID == id);

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
        public async Task<IActionResult> UpdatePrColor(PrColorCRUD model)
        {
            try
            {
                var existingProduct = await _context.PrColor.FindAsync(model.ID);

                if (existingProduct == null)
                {
                    return NotFound("Không tìm thấy");
                }
                if (model.PrPath != null)
                {
                    var PrPath = await _iCommon.UploadedFile(model.PrPath);
                    existingProduct.ImgColor = "/upload/" + PrPath;
                }
                else
                {
                    existingProduct.ImgColor = existingProduct.ImgColor;
                }
                existingProduct.NameColor = model.NameColor;
                await _context.SaveChangesAsync();

                return Ok(existingProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletePrColor(PrColor model)
        {
            try
            {
                var existingProduct = await _context.PrColor.FirstOrDefaultAsync(x => x.ID == model.ID);
                if (existingProduct == null)
                {
                    return NotFound();

                }
                _context.PrColor.Remove(existingProduct);
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

        public IActionResult GetTermsPr()
        {
            var pr = _context.TermsPr.FirstOrDefault(x => x.ID == 1);
            return Ok(pr);
        }
        [HttpPost]
        public async Task<IActionResult> AddPageMain(TermsPrCRUD model, int idMain)
        {
            try
            {
                var pr = _context.TermsPr.FirstOrDefault(x => x.ID == idMain);
                if (pr == null)
                {
                    return new JsonResult(new
                    {
                        status = 500,
                        msg = "False"

                    });
                }
                else
                {
                    pr.AboutMain = model.AboutMain;
                    await _context.SaveChangesAsync();
                    return new JsonResult(new
                    {
                        status = 200,
                        msg = "Success",
                        Product = pr

                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddTerms(TermsPrCRUD model, int idMain)
        {
            try
            {
                var pr = _context.TermsPr.FirstOrDefault(x => x.ID == idMain);
                if (pr == null)
                {
                    return new JsonResult(new
                    {
                        status = 500,
                        msg = "False"

                    });
                }
                else
                {
                    pr.Terms = model.Terms;
                    await _context.SaveChangesAsync();
                    return new JsonResult(new
                    {
                        status = 200,
                        msg = "Success",
                        Product = pr

                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddAbout(TermsPrCRUD model, int idMain)
        {
            try
            {
                var pr = _context.TermsPr.FirstOrDefault(x => x.ID == idMain);
                if (pr == null)
                {
                    return new JsonResult(new
                    {
                        status = 500,
                        msg = "False"

                    });
                }
                else
                {
                    pr.AboutPage = model.AboutPage;
                    await _context.SaveChangesAsync();
                    return new JsonResult(new
                    {
                        status = 200,
                        msg = "Success",
                        Product = pr

                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddReturn(TermsPrCRUD model, int idMain)
        {
            try
            {
                var pr = _context.TermsPr.FirstOrDefault(x => x.ID == idMain);
                if (pr == null)
                {
                    return new JsonResult(new
                    {
                        status = 500,
                        msg = "False"

                    });
                }
                else
                {
                    pr.RefunPr = model.RefunPr;
                    await _context.SaveChangesAsync();
                    return new JsonResult(new
                    {
                        status = 200,
                        msg = "Success",
                        Product = pr

                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [AllowAnonymous]
        [HttpGet]

        public IActionResult GetAllACouponChild()
        {
            var pr = _context.CouponChild;

            return Ok(pr.ToList().OrderByDescending(x => x.ID));
        }
        [HttpPost]
        public async Task<IActionResult> AddCouponChild(CouponChild model)
        {
            try
            {

                _context.CouponChild.Add(model);
                await _context.SaveChangesAsync();

                return Ok("Thành công");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet]
        public async Task<IActionResult> getIdCouponChild(int id)
        {
            try
            {
                CouponChild vm = new CouponChild();
                if (id > 0)
                {
                    try
                    {
                        vm = await _context.CouponChild.FirstOrDefaultAsync(x => x.ID == id);

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
        public async Task<IActionResult> UpdateCouponChild(CouponChild model)
        {
            try
            {
                var existingProduct = await _context.CouponChild.FindAsync(model.ID);

                if (existingProduct == null)
                {
                    return NotFound("Không tìm thấy");
                }
                existingProduct.CouponMain = model.CouponMain;
                existingProduct.ContentCop = model.ContentCop;
                await _context.SaveChangesAsync();

                return Ok(existingProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCouponChild(CouponChild model)
        {
            try
            {
                var existingProduct = await _context.CouponChild.FirstOrDefaultAsync(x => x.ID == model.ID);
                if (existingProduct == null)
                {
                    return NotFound();

                }
                _context.CouponChild.Remove(existingProduct);
                await _context.SaveChangesAsync();

                return Ok(existingProduct);

            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

    }
}
