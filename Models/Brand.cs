﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Practice_03_07.Models
{
    public class Brand
    {
        public int Id { get; set; }

        [Required]
        public string BrandName { get; set; }

        public string? BrandImage { get; set; }

    }
}