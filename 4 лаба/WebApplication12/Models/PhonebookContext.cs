using Microsoft.EntityFrameworkCore;

namespace WebApplication12.Models
{
    public class PhonebookContext : DbContext
    {
        public PhonebookContext(DbContextOptions<PhonebookContext> options)
            : base(options)
        {
        }

        public DbSet<Entry> Entries { get; set; }
    }

}
