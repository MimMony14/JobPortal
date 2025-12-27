using Microsoft.EntityFrameworkCore;

public class JobPortalContext : DbContext
{
    public JobPortalContext(DbContextOptions<JobPortalContext> options)
        : base(options) { }

    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Job> Jobs { get; set; }
}
