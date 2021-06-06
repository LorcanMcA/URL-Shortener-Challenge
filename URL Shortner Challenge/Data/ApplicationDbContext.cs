using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using URL_Shortner_Challenge.Models;

namespace URL_Shortner_Challenge.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<URL_Shortner_Challenge.Models.ShortLink> ShortLink { get; set; }
    }
}
