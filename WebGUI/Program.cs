using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataAccessLayer.Utils;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebGUI.Data;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;

namespace WebGUI
{
    public class Program
    {
        public static void Main(string[] args)
        {

//            var host = new WebHostBuilder()
//             .UseContentRoot(Directory.GetCurrentDirectory())
//             .UseKestrel()
////             .UseKestrel(options =>
////             {
//// //                options.Listen(IPAddress.Loopback, 5001, listenOptions =>
///////                 {
//////                     listenOptions.UseHttps("testCert.pfx", "testPassword");
//////                     listenOptions.UseConnectionLogging();
//////                 });
////             })
//////             .UseIISIntegration()
//             .UseStartup<Startup>()
//             .Build();

//            host.Run();
            var host = CreateWebHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var ctx = services.GetRequiredService<ApplicationDbContext>();
                    ctx.Database.Migrate();

                    AppSeed.SeedAsync(services).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while attempting to seed data.");
                }
            }
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
         //       .UseKestrel()
         //    .UseUrls("http://localhost:8080", "http://odin:8080", "http://192.168.43.143:8080")
                .UseStartup<Startup>();
    }

}
