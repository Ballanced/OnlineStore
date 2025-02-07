using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using OnlineStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineStore.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ProductsController : Controller
    {
        private readonly StoreDbContext _context;

        public ProductsController(StoreDbContext context)
        {
            _context = context;
        }

        // Список всіх продуктів, доступний лише адміністраторам
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            // Логування вхідних даних
            Console.WriteLine($"Creating Product: Name={product.Name}, Price={product.Price}, Stock={product.Stock}, CategoryId={product.CategoryId}, Description={product.Description}, ImageUrl={product.ImageUrl}");

            if (ModelState.IsValid)
            {
                try
                {
                    // Ми не зберігаємо об'єкт категорії, тільки CategoryId
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Creating Product: {ex.Message}");
                }
            }
            else
            {
                foreach (var error in ModelState)
                {
                    foreach (var e in error.Value.Errors)
                    {
                        Console.WriteLine($"Key: {error.Key}, Error: {e.ErrorMessage}");
                    }
                }
            }

            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
            return View(product);
        }

        // Редагування продукту, доступне лише адміністраторам
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            // Логування вхідних даних
            Console.WriteLine($"Editing Product: Id={product.Id}, Name={product.Name}, Price={product.Price}, Stock={product.Stock}, CategoryId={product.CategoryId}, Description={product.Description}, ImageUrl={product.ImageUrl}");

            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Ми не зберігаємо об'єкт категорії, тільки CategoryId
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Editing Product: {ex.Message}");
                }
            }
            else
            {
                foreach (var error in ModelState)
                {
                    foreach (var e in error.Value.Errors)
                    {
                        Console.WriteLine($"Key: {error.Key}, Error: {e.ErrorMessage}");
                    }
                }
            }

            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name", product.CategoryId);
            return View(product);
        }



        // Видалення продукту, доступне лише адміністраторам
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

         [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Деталі продукту, доступні покупцям та адміністраторам
        [Authorize(Roles = "Customer,Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // Передаємо дані категорії через ViewBag
            ViewBag.CategoryName = _context.Categories
                .Where(c => c.Id == product.CategoryId)
                .Select(c => c.Name)
                .FirstOrDefault();

            return View(product);
        }



        // Список продуктів, доступний покупцям та адміністраторам
        [Authorize(Roles = "Customer,Administrator")]
        public IActionResult List()
        {
            return View(_context.Products.ToList());
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
