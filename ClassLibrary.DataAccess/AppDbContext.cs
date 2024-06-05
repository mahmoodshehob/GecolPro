using ClassLibrary.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ClassLibrary.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        private const string Schema = "dbo";

        public virtual DbSet<Meter>? Meters { get; set; } = null;
        public virtual DbSet<Request>? Requests { get; set; } = null;


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Meter>(entity => { entity.ToTable("Meters", Schema); });
        //    modelBuilder.Entity<Request>(entity => { entity.ToTable("Requests", Schema); });

        //}


    }
}