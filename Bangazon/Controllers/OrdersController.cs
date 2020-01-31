using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bangazon.Data;
using Bangazon.Models;
using Microsoft.AspNetCore.Identity;
using Bangazon.Models.OrderViewModels;

namespace Bangazon.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // Get user's cart
        public async Task<IActionResult> Cart()
        {
          
            var user = await GetCurrentUserAsync();

            var order = await _context.Order
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.UserId == user.Id && o.PaymentTypeId == null);

            if (order == null)
            {
                TempData["NoOrders"] = "Your Cart is Empty.";
                return RedirectToAction("Index", "Home");
            }

            var totalCost = order.OrderProducts.Sum(op => op.Product.Price);

            var paymentTypes = await _context.PaymentType.Where(pt => pt.UserId == user.Id).ToListAsync();

            var paymentOptions = paymentTypes.Select(pt => new SelectListItem
            {
                Value = pt.PaymentTypeId.ToString(),
                Text = pt.Description
            });

            var viewModel = new ShoppingCartViewModel
            {
                TotalCost = totalCost,
                User = user,
                PaymentOptions = paymentOptions.ToList(),
                SelectedPaymentId = paymentTypes.FirstOrDefault().PaymentTypeId,
                OrderDetails = new OrderDetailViewModel()
                {
                    Order = order,
                    LineItems = order.OrderProducts
                    .GroupBy(op => op.ProductId)
                    .Select(group => new OrderLineItem
                    {
                        Units = group.Count(),
                        Product = group.FirstOrDefault().Product,
                        Cost = group.Sum(op => op.Product.Price)

                    })
                    
                }
            };

            return View(viewModel);
        }

        //[HttpPost]
        //public async Task<IActionResult> Cart(ShoppingCartViewModel)
        //{

        //}




        //// GET: Orders
        //public async Task<IActionResult> Index()
        //{
        //    var user = await GetCurrentUserAsync();

        //    var orders = _context.Order.Include(o => o.PaymentType)
        //        .Where(o => o.UserId == user.Id);
        //    return View(await orders.ToListAsync());
        //}

        //// GET: Orders/Details/5

        //public async Task<IActionResult> Details(int? id)
        //{

        //    var user = await GetCurrentUserAsync();

        //    var order = await _context.Order
        //        .Include(o => o.PaymentType)
        //        .Include(o => o.User)
        //        .Include(o => o.OrderProducts)
        //             .ThenInclude(op => op.Product)
        //        .FirstOrDefaultAsync(m => m.UserId == user.Id && m.DateCompleted == null);
        //    if (order == null)
        //    {
        //        TempData["NoOrders"] = "Your Cart is Empty.";
        //        return RedirectToAction("Index", "Home");
        //    }

        //    return View(order);
        //}

        //// GET: Orders/Create
        //public IActionResult Create()
        //{
        //    ViewData["PaymentTypeId"] = new SelectList(_context.PaymentType, "PaymentTypeId", "AccountNumber");
        //    ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
        //    return View();
        //}

        //// POST: Orders/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("OrderId,DateCreated,DateCompleted,UserId,PaymentTypeId")] Order order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(order);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["PaymentTypeId"] = new SelectList(_context.PaymentType, "PaymentTypeId", "AccountNumber", order.PaymentTypeId);
        //    ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", order.UserId);
        //    return View(order);
        //}

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var user = await GetCurrentUserAsync();

            if (id == null)
            {
                return NotFound();
            }
            var order = await _context.Order
                .FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentType, "PaymentTypeId", "Description", order.PaymentTypeId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,DateCreated,DateCompleted,UserId,PaymentTypeId")] Order order)
        {
            ModelState.Remove("User");

            if (id != order.OrderId)
            {

                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    order.DateCompleted = DateTime.Now;
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["SuccessMessage"] = "Thanks for ordering!";
                return RedirectToAction("Index", "Home");
            }
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentType, "PaymentTypeId", "Description", order.PaymentTypeId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.PaymentType)
                .Include(o => o.User)
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.OrderId == id);
            foreach (var item in order.OrderProducts)
            {
                _context.OrderProduct.Remove(item);

            }
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

    }
}
