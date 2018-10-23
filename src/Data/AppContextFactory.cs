using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data
{
    public class AppContextFactory : IDesignTimeDbContextFactory<AppContext>
    {
        public AppContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppContext>();
            optionsBuilder.UseNpgsql(GetConnectionString());
            return new AppContext(optionsBuilder.Options);
        }
        public static void SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }
        private static string ConnectionString = null;
        private static string GetConnectionString()
        {
            if(ConnectionString == null)
            {
                return "Fake connection string";
            }
            return ConnectionString;
        }
    }
}
