using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shop.Controllers
    {
    public class PieGiftController : Controller
        {
         private readonly IPieRepository _pieRepository;
        private readonly IOrderRepository _orderRepository;

        public PieGiftController(IPieRepository pieRepository, IOrderRepository orderRepository)
            {
            _pieRepository = pieRepository;
            _orderRepository = orderRepository;
            }

        // GET: /<controller>/
        public IActionResult Index()
            {
            return View();
            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[IgnoreAntiforgeryToken]
        public IActionResult Index(PieGiftOrder pieGiftOrder)
            {
            var pieOfTheMonth = _pieRepository.PiesOfTheWeek.FirstOrDefault();

            if (pieOfTheMonth != null)
                {
                pieGiftOrder.Pie = pieOfTheMonth;
                _orderRepository.CreatePieGiftOrder(pieGiftOrder);
                return RedirectToAction("PieGiftOrderComplete");
                }

            return View();
            }

        public IActionResult PieGiftOrderComplete()
            {
            ViewBag.PieGiftOrderCompleteMessage = HttpContext.User.Identity.Name +
                                                  ", thanks for the order. Your friend will soon receive the pie!";
            return View();
            }
        }
    }