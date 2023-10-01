using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Practice_03_07.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Practice_03_07.ViewModel
{
    public class CityVM
    {
        public City City { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CountryList { get; set; }

       
    }
}
