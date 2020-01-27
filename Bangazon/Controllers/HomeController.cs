using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bangazon.Models;
using Bangazon.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Bangazon.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: Products
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.Product.Include(p => p.ProductType).Include(p => p.User);
        //    return View(await applicationDbContext.ToListAsync());
        //}
        public async Task<IActionResult> Index(string searchQuery)
        {
            if (searchQuery == null)
            {
                var emptyList = new List<Product>();
                return View(emptyList);
            }
            else
            {
                //searchQuery = searchQuery;
                return View(await _context.Product
              .Where(p => p.City.Equals(searchQuery))
                    .ToListAsync());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}
