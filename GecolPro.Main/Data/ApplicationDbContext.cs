using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ClassLibrary.Models.GecolModels;
using ClassLibrary.Models.DcbModels;
using ClassLibrary.Models.UssdModels;

namespace GecolPro.Main.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //public DbSet<Category> Categories { get; set; }

    }
}