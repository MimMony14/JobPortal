using Microsoft.EntityFrameworkCore;
using JobPortal.Models;

namespace JobPortal.Data
{
    public class JobPortalContext : DbContext
    {
        public JobPortalContext(DbContextOptions<JobPortalContext> options)
            : base(options) { }

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Apply> ApplyForms { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }

    }
}
