using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using Shop.Filters;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Shop.Utility;
using Shop.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Shop.Controllers
    {
    [PieNotFoundException]
    public class PieController : Controller
        {
        private readonly IPieRepository _pieRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPieReviewRepository _pieReviewRepository;
        private readonly HtmlEncoder _htmlEncoder;
        private readonly ILogger<PieController> _logger;

        public PieController(IPieRepository pieRepository, ICategoryRepository categoryRepository, ILogger<PieController> logger,
            IPieReviewRepository pieReviewRepository, HtmlEncoder htmlEncoder)
            {
            _pieRepository = pieRepository;
            _categoryRepository = categoryRepository;
            _pieReviewRepository = pieReviewRepository;
            _htmlEncoder = htmlEncoder;
            _logger = logger;
            }

        //[Route("AllPies")]
        //[Route("ListOfPies")]
        //public ViewResult List(string category)
        //{
        //    IEnumerable<Pie> pies;
        //    string currentCategory = string.Empty;

        //    if (string.IsNullOrEmpty(category))
        //    {
        //        pies = _pieRepository.Pies.OrderBy(p => p.PieId);
        //        currentCategory = "All pies";
        //    }
        //    else
        //    {
        //        pies = _pieRepository.Pies.Where(p => p.Category.CategoryName == category)
        //           .OrderBy(p => p.PieId);
        //        currentCategory = _categoryRepository.Categories.FirstOrDefault(c => c.CategoryName == category).CategoryName;
        //    }

        //    return View(new PiesListViewModel
        //    {
        //        Pies = pies,
        //        CurrentCategory = currentCategory
        //    });
        //}

        [Authorize]

        public ViewResult List(string category)
            {
            string _category = category;
            IEnumerable<Pie> pies;
            string currentCategory = string.Empty;

            if (string.IsNullOrEmpty(category))
                {
                pies = _pieRepository.Pies.OrderBy(p => p.PieId);
                currentCategory = "All pies";
                }

            else {

                if (string.Equals("Fruit pies", _category, StringComparison.OrdinalIgnoreCase))
                    {
                    pies = _pieRepository.Pies.Where(p => p.Category.CategoryId.Equals(1)).OrderBy(p => p.Name);
                    }

                else

                    pies = _pieRepository.Pies.Where(p => p.Category.CategoryId.Equals(4)).OrderBy(p => p.Name);

                currentCategory = _category;

                }

            var pieListViewModel = new PiesListViewModel
                {
                Pies = pies,
                CurrentCategory = currentCategory
                };


               return View(pieListViewModel);

                }
            

        

            

        [Route("[controller]/Details/{id}")]
        public IActionResult Details(int id)
        {
            var pie = _pieRepository.GetPieById(id);
            if (pie == null)
            {
                _logger.LogDebug(LogEventIds.GetPieIdNotFound, new Exception("Pie not found"), "Pie with id {0} not found", id);
                //return NotFound();
                //Catch this error using the exception filter
                throw new PieNotFoundException();
            }

            return View(new PieDetailViewModel() { Pie = pie });
        }

        [Route("[controller]/Details/{id}")]
        [HttpPost]
        public IActionResult Details(int id, string review)
        {
            var pie = _pieRepository.GetPieById(id);
            if (pie == null)
            {
                _logger.LogWarning(LogEventIds.GetPieIdNotFound, new Exception("Pie not found"), "Pie with id {0} not found", id);
                return NotFound();
            }

            string encodedReview = _htmlEncoder.Encode(review);

            _pieReviewRepository.AddPieReview(new PieReview() { Pie = pie, Review = encodedReview });

            return View(new PieDetailViewModel() { Pie = pie });
        }

    }
}
