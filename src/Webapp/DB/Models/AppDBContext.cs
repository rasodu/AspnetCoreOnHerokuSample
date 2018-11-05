using Microsoft.EntityFrameworkCore;

namespace Webapp.DB.Models
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }
        public DbSet<TestTableRecord> TestTableRecords { get; set; }
    }
}
