using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PlaceOrder(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("OrderConfirmation");
            }
            return View("Checkout", model);
        }

            public IActionResult OrderConfirmation()
            {
                return View();
            }
    }
}
