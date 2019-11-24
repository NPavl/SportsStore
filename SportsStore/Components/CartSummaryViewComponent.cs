using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SportsStore.Components
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private Cart cart;

        public CartSummaryViewComponent(Cart cartService)
        {
            cart = cartService;
        }

        public IViewComponentResult Invoke()
        {
            return View(cart);
        }

        // Этот компонент представления способен задействовать в своих интересах службу,
        // созданную ранее в главе для получения объекта Cart, принимая ее как аргумент конс ­
        // труктора. Результатом оказывается простой компонент представления, который пере ­
        // дает объект Cart методу View() ,чтобы сгенерировать фрагмент НТМL-разметки для
        // включения в компоновку.
    }
}
