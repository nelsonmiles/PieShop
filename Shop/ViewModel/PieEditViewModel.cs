using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Shop.ViewModels
{
    public class PieEditViewModel
    {
        public Pie Pie { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public int CategoryId { get; set; }
    }
}
