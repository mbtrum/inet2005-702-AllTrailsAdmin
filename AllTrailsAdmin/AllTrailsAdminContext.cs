using Microsoft.EntityFrameworkCore;

public class AllTrailsAdminContext(DbContextOptions<AllTrailsAdminContext> options) : DbContext(options)
{
    public DbSet<AllTrailsAdmin.Models.Trail> Trail { get; set; } = default!;

    public DbSet<AllTrailsAdmin.Models.Category> Category { get; set; } = default!;
}
