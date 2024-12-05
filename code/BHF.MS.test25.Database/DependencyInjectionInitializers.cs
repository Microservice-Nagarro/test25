using BHF.MS.test25.Database.Context;
using BHF.MS.test25.Database.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace BHF.MS.test25.Database
{
    [ExcludeFromCodeCoverage(Justification = "It's an IoC initializer")]
    public static class DependencyInjectionInitializers
    {
        public static void AddDatabases(IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<CustomDbContext>(options =>
            {
                options.UseSqlServer("name=ConnectionStrings:Dbtest25");
            });
        }

        public static void AddCustomServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IDbItemService, DbItemService>();
        }

        public static void AddHealthCheckConfiguration(IServiceCollection serviceCollection)
        {
            serviceCollection.AddHealthChecks()
                .AddDbContextCheck<CustomDbContext>(
                    name: $"{nameof(CustomDbContext)} health check",
                    tags: ["ready"]);
        }

        public static void InitializeDatabases(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<CustomDbContext>();
            db.Database.Migrate();
        }
    }
}

