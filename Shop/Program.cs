

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Shop.Model;
using Shop.Models;
using System;

namespace Shop
    {
    public class Program
        {
      

            public static void Main(string[] args)
                {


            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
                {

                var services = scope.ServiceProvider;
                try
                    {
                    var context = services.GetRequiredService<AppDbContext>();
                    DbInitializer.Seed(context);
                    }
                catch (Exception)
                    {


                    }
                }
            host.Run();
            }



        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().Build();



        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>();
        }
    }
