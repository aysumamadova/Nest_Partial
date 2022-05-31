using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest_Backend.DAL;
using Nest_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nest_Backend.Controllers
{
    public class ProductController : Controller
    {
        private AppDbContext _context { get; }
        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.ProductCount = _context.Products.Where(p => p.IsDeleted == false).Count();
            ViewBag.Categories = _context.Categories.Where(p => p.IsDeleted == false).Include(c => c.Products);
            return View(_context.Products.Where(p => p.IsDeleted == false).OrderByDescending(p => p.Id).Take(10).Include(p => p.ProductImgs).Include(p => p.Categories));
        }
        public IActionResult LoadMore(int skip)
        {
            IQueryable<Product> p = _context.Products.Where(p => p.IsDeleted == false);
            int productCount = p.Count();
            return PartialView("_ProductPartial", p
                                    .OrderByDescending(p => p.Id)
                                    .Skip(skip)
                                    .Take(10)
                                    .Include(p => p.ProductImgs)
                                    .Include(p => p.Categories));
        }
        public IActionResult CategoryFilter(int CategoriesId)
        {
            if (_context.Categories.Find(CategoriesId) == null) return NotFound();
            return PartialView("_ProductPartial", _context.Products.Where(p => p.IsDeleted == false && p.CategoriesId == CategoriesId)
                                .OrderByDescending(p => p.Id)
                                .Include(p => p.ProductImgs)
                                .Include(p => p.Categories));
        }
    }
}
    

