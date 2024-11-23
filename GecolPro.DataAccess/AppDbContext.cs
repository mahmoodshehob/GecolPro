using GecolPro.Models.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace GecolPro.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Meter>? Meters { get; set; } = null;
        public virtual DbSet<Request>? Requests { get; set; } = null;
        public virtual DbSet<IssueTkn>? IssueTkns { get; set; } = null;




    }
}