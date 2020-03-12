using Microsoft.EntityFrameworkCore;
using PocketLists.Models;

namespace PocketLists.Data
{
    public class TasksContext : DbContext
    {
        public TasksContext(DbContextOptions<TasksContext> options)
            : base(options)
        {
        }
        public DbSet<Task> Tasks { get; set; }
    }
}
