using Microsoft.AspNetCore.Mvc.Rendering;
using Practice_03_07.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Practice_03_07.ViewModel
{
    public class SubCategoryVM
    {
        public SubCategory SubCategory { get; set; }

        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
