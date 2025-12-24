using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProjectTaskManager.Infrastructure.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // PostgreSQL credentials & connection string
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=projecttaskmanager;Username=taskmanager;Password=admin");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
