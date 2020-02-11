using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.Extensions.DependencyInjection;

namespace Shiliyuanma.Generator.SqlServer.Internal
{
    public static class ServicesColloctionExtensions
    {
        public static IServiceCollection RemoveEfCoreMigrationsScaffolder(this IServiceCollection services)
        {
            return services.Remove(typeof(MigrationsScaffolderDependencies), typeof(IMigrationsScaffolder));
        }

        public static IServiceCollection Remove(this IServiceCollection servcies, params Type[] types)
        {
            if (servcies == null || (types?.Length ?? 0) == 0)
            {
                return servcies;
            }

            var list = servcies.Where(q => types.Contains(q.ServiceType)).ToList();
            list.ForEach(q => servcies.Remove(q));
            return servcies;
        }
    }
}