using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Design;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Reflection;

namespace DXApplication4.Blazor.Server
{
    public class Program : IDesignTimeApplicationFactory
    {
        static bool HasArg(string[] args, string argument) =>
            args.Any(arg => arg.TrimStart('/').TrimStart('-')
                .Equals(argument, StringComparison.OrdinalIgnoreCase));

        public static int Main(string[] args)
        {
            if (HasArg(args, "help") || HasArg(args, "h"))
            {
                Console.WriteLine("Updates the database when its version does not match the application's version.");
                Console.WriteLine();
                Console.WriteLine($"    {Assembly.GetExecutingAssembly().GetName().Name}.exe --updateDatabase [--forceUpdate --silent]");
                Console.WriteLine();
                Console.WriteLine("--forceUpdate - Marks that the database must be updated whether its version matches the application's version or not.");
                Console.WriteLine("--silent - Marks that database update proceeds automatically and does not require any interaction with the user.");
                Console.WriteLine();
                Console.WriteLine($"Exit codes: 0 - {DevExpress.ExpressApp.Utils.DBUpdaterStatus.UpdateCompleted}");
                Console.WriteLine($"            1 - {DevExpress.ExpressApp.Utils.DBUpdaterStatus.UpdateError}");
                Console.WriteLine($"            2 - {DevExpress.ExpressApp.Utils.DBUpdaterStatus.UpdateNotNeeded}");
                return 0;
            }

            DevExpress.ExpressApp.FrameworkSettings.DefaultSettingsCompatibilityMode =
                DevExpress.ExpressApp.FrameworkSettingsCompatibilityMode.Latest;
            DevExpress.ExpressApp.Security.SecurityStrategy.AutoAssociationReferencePropertyMode =
                DevExpress.ExpressApp.Security.ReferenceWithoutAssociationPermissionsMode.AllMembers;

            IHost host = CreateHostBuilder(args).Build();

            if (HasArg(args, "updateDatabase"))
            {
                using var scope = host.Services.CreateScope();
                var updater = scope.ServiceProvider.GetRequiredService<DevExpress.ExpressApp.Utils.IDBUpdater>();
                return updater.Update(HasArg(args, "forceUpdate"), HasArg(args, "silent"));
            }

            host.Run();
            return 0;
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        XafApplication IDesignTimeApplicationFactory.Create()
        {
            IHostBuilder hostBuilder = CreateHostBuilder(Array.Empty<string>());
            return DevExpress.ExpressApp.Blazor.DesignTime.DesignTimeApplicationFactoryHelper.Create(hostBuilder);
        }
    }
}
 