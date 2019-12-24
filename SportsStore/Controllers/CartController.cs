using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsStore.Infrastructure;

namespace SportsStore.Controllers
{




    public class CartController : Controller
    {
        private IProductRepository repository;
        private Cart cart;

        public CartController(IProductRepository repo, Cart cartService)
        {
            repository = repo;
            cart = cartService;
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {

                Сart = cart,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToActionResult AddToCart(int productId, string returnUrl)
        {
            Products products = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);

            if (products != null)
            {
                cart.AddItem(products, 1);
            }

            return RedirectToAction("Index", new { returnUrl });
        }
        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
        {
            Products products = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);
            if (products != null)
            {
                cart.RemoveLine(products);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

    }
}

