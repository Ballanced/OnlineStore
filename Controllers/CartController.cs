﻿using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Extensions;
using System.Collections.Generic;
using System.Linq;
using OnlineStore.Extensions;

namespace OnlineStore.Controllers
{
    public class CartController : Controller
    {
        private readonly StoreDbContext _context;

        public CartController(StoreDbContext context) 
        { 
            _context = context; 
        }


        public IActionResult Index()
        {
            {
                var cart = HttpContext.Session.GetObjectFromJson<List<Product>>("Cart") ?? new List<Product>();
                return View(cart);
            }
        }


        public IActionResult AddToCart(int id) 
        { 
            var product = _context.Products.FirstOrDefault(p => p.Id == id); 
            if (product != null) 
            { 
                var cart = HttpContext.Session.GetObjectFromJson<List<Product>>("Cart") ?? new List<Product>(); 
                cart.Add(product); 
                HttpContext.Session.SetObjectAsJson("Cart", cart); 
            } 
            return RedirectToAction("Index"); 
        }

        public IActionResult RemoveFromCart(int id) 
        { 
            var cart = HttpContext.Session.GetObjectFromJson<List<Product>>("Cart") ?? new List<Product>(); 
            var productToRemove = cart.FirstOrDefault(p => p.Id == id); 
            if (productToRemove != null) 
            { 
                cart.Remove(productToRemove); 
                HttpContext.Session.SetObjectAsJson("Cart", cart); 
            } 
            return RedirectToAction("Index"); 
        }


    }
}

