using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Practice_03_07.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Practice_03_07.Data
{
    public class AppDb:IdentityDbContext
    {
        public AppDb(DbContextOptions<AppDb> options):base(options) { }

        public DbSet<AppUser> AppUser { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<SubCategory> subCategories { get; set; }

        public DbSet<Tax> Taxes { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<City> Cities { get; set; }
         
        public DbSet<Brand> Brands { get; set; }


    }
}
