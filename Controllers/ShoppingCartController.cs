using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebsiteBanHang.Data;
using WebsiteBanHang.Extensions;
using WebsiteBanHang.Models;
using WebsiteBanHang.Repositories;

namespace WebsiteBanHang.Controllers
{
    public class ShoppingCartController : Controller
    {
        private const string CartKey = "Cart";
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShoppingCartController(
            IProductRepository productRepository,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _productRepository = productRepository;
            _context = context;
            _userManager = userManager;
        }

        // Yêu cầu đăng nhập (Nhân viên/Admin/Khách hàng đều được)
        [Authorize] 
        public async Task<IActionResult> AddToCart(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>(CartKey) ?? new ShoppingCart();
            cart.AddItem(product, 1);
            HttpContext.Session.SetObjectAsJson(CartKey, cart);

            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>(CartKey) ?? new ShoppingCart();
            return View(cart);
        }

        [Authorize]
        public IActionResult RemoveFromCart(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>(CartKey);
            if (cart != null)
            {
                cart.RemoveItem(id);
                HttpContext.Session.SetObjectAsJson(CartKey, cart);
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Checkout()
        {
            return View(new Order());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>(CartKey);

            if (cart == null || !cart.Items.Any())
            {
                ModelState.AddModelError("", "Giỏ hàng đang trống.");
                return View(order);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            order.UserId = user.Id;
            order.OrderDate = DateTime.Now;
            order.TotalPrice = cart.GetTotal();
            order.OrderDetails = cart.Items.Select(i => new OrderDetail
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList();

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove(CartKey);

            return View("OrderCompleted", order.Id);
        }
    }
}