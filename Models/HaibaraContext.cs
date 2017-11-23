using Microsoft.EntityFrameworkCore;
namespace haibara.Models
{
    public class HaibaraContext : DbContext
    {
        public HaibaraContext(DbContextOptions<HaibaraContext> options) : base(options)
        {
        }

        //List of Models to map via Entity Framework
        public DbSet<Contact> Contacts { get; set; }
    }
}