﻿using BookShop.DataAccess.Repository.IRepository;
using BookShop.Models;
using BookShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShopWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> objCoverTypeList = _unitOfWork.Product.GetAll();
            return View(objCoverTypeList);
        }

        //GET
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };

            if (id == null || id == 0)
            {
                //Create Product
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfWork.Product.GetFirsrOrDefault(u => u.Id == id);
                return View(productVM);
            }
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if(file!=null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extenstion = Path.GetExtension(file.FileName);

                    if(obj.Product.ImageURL != null)
                    {
                        var oldImage = Path.Combine(wwwRootPath, obj.Product.ImageURL.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extenstion), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageURL = @"\images\products\" + fileName + extenstion;
                }
                if(obj.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.Product);
                } else
                {
                    _unitOfWork.Product.Update(obj.Product);
                }
                
                _unitOfWork.Save();
                TempData["success"] = "Product Created Successfully";
                return RedirectToAction("Index");
            }
            return View(obj);

        }


        #region API CALLS
        //Return Data as JSON to use it in the Data Table
        [HttpGet]
        public IActionResult GetAll()
        {
            var ProductList = _unitOfWork.Product.GetAll(includeProperties:"Category");
            return Json(new {data = ProductList});
        }

        //POST
        [HttpDelete, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {

            var obj = _unitOfWork.Product.GetFirsrOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new {success = false, message = "Error while deleting"});
            }

            var oldImage = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageURL.TrimStart('\\'));
            if (System.IO.File.Exists(oldImage))
            {
                System.IO.File.Delete(oldImage);
            }

            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion

    }
}
