using Microsoft.EntityFrameworkCore;
using PocketLists.Models;

namespace PocketLists.Data
{
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions<UsersContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}
