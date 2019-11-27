using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shop.Auth;
using Shop.Filters;
using Shop.Model;
using Shop.Models;

namespace Shop
    {
    public class Startup
        {
        private IConfigurationRoot _configurationRoot;
        public Startup(IHostingEnvironment hostingEnvironment)
            {
            _configurationRoot = new ConfigurationBuilder()
                           .SetBasePath(hostingEnvironment.ContentRootPath)
                           .AddJsonFile("appsettings.json")
                           .Build();
            }



        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
            {
            //services.AddDbContext<AppDbContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<AppDbContext>(options =>
                                       options.UseSqlServer(_configurationRoot.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
            services.AddTransient<IPieRepository, PieRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ShoppingCart>(sp => ShoppingCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IPieReviewRepository, PieReviewRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();

            services.AddMemoryCache();
            services.AddScoped<TimerAction>();
            services.AddSession();
          

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdministratorOnly", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("DeletePie", policy => policy.RequireClaim("Delete Pie", "Delete Pie"));
                options.AddPolicy("AddPie", policy => policy.RequireClaim("Add Pie", "Add Pie"));
                //options.AddPolicy("MinimumOrderAge", policy => policy.Requirements.Add(new MinimumOrderAgeRequirement(18)));
            });


            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddAntiforgery();

            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });

            services.AddMvc
                (
                    config =>
                    {
                        config.Filters.AddService(typeof(TimerAction));
                        config.CacheProfiles.Add("Default",
                            new CacheProfile()
                                {
                                Duration = 30,
                                Location = ResponseCacheLocation.Any
                                });
                        config.CacheProfiles.Add("None",
                            new CacheProfile()
                                {
                                Location = ResponseCacheLocation.None,
                                NoStore = true
                                });
                    }
                ) .AddViewLocalization(
                    LanguageViewLocationExpanderFormat.Suffix,
                    opts => { opts.ResourcesPath = "Resources"; })
                .AddDataAnnotationsLocalization()
                .AddMvcOptions(options =>
                {
                    options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(s => $"You can't leave this {s} empty");
                }
            );



            }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
            {
            loggerFactory.AddDebug();
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();
            app.UseRequestLocalization(app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value);


            app.UseMvc(routes =>
            {

                //areas
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}");

                routes.MapRoute(
                  name: "categoryfilter",
                  template: "Pie/{action}/{category?}",
                  defaults: new { Controller = "Pie", action = "List" });

                routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");


            });


            }
        }
    }
