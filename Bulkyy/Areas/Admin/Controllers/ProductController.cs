using Bulkyy.DataAccess.Data;
using Bulkyy.DataAccess.Repository.IRepository;
using Bulkyy.Models;
using Bulkyy.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bulkyy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> ObjproductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            
            return View(ObjproductList);

        }
        public IActionResult Upsert(int? id)
        {
			    
            ProductVM productVM = new ()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
					Text = i.Name,
					Value = i.Id.ToString()
				}),
				Product = new Product(),
				
			};
            if(id == null || id == 0)
            {
                //create
				return View(productVM);
			}
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
			
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM product, IFormFile? file)
        {
            

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"\images\product");

                    if (!string.IsNullOrEmpty(product.Product.ImageUrl))
                    {
                        //delete old image
                        var oldImagePath 
                            = Path.Combine(wwwRootPath, product.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    
                    }
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
						file.CopyTo(fileStream);
					}
                    product.Product.ImageUrl = @"\images\product\" + fileName;

                }
                if(product.Product.Id != 0)
                {
					_unitOfWork.Product.Update(product.Product);
				}
				else
                {
					_unitOfWork.Product.Add(product.Product);
				}
                
                _unitOfWork.Save();
                TempData["success"] = "product created succesfully";
                return RedirectToAction("Index", "Product");

            }
            else
            {
                product.CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
					Text = i.Name,
					Value = i.Id.ToString()
				});
				return View(product);
			}        
        }
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
        //    if (productFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(productFromDb);
        //}

        //[HttpPost]
        //public IActionResult Edit(Product product)
        //{


        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Update(product);
        //        _unitOfWork.Save();
        //        TempData["success"] = "product edited succesfully";
        //        return RedirectToAction("Index", "Product");
        //    }
        //    return View();

        //}
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "product deleted  succesfully";
            return RedirectToAction("Index", "Product");

        }

  //      #region API CALLS

  //      [HttpGet]
  //      public IActionResult GetAll(int id)
  //      {
		//  List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
  //          return Json(new { data = objProductList });
		//}
		//#endregion

	}
}

