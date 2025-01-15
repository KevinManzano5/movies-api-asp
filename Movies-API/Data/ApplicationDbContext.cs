using Microsoft.EntityFrameworkCore;
using Movies_API.Models;

namespace Movies_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Category> Category { get; set; }
    }
}
