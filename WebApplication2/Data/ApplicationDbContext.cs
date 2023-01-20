using System.Data.Entity;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnectionString")

        {

        }
        public DbSet<ITHealthCheckDBModel> dbModels { get; set; }
    }
}