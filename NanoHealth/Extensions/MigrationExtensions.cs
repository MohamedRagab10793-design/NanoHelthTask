using Microsoft.EntityFrameworkCore;
using NanoHealth.Data;

namespace NanoHealth.Extensions;

public static class  MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using WorkflowDbContext dbContext =
            scope.ServiceProvider.GetRequiredService<WorkflowDbContext>();

        dbContext.Database.Migrate();
    }
}
