using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SportsStore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace SportsStore
{
  
    public class Startup
    {
        IConfigurationRoot Configuration;        
        public Startup(IHostingEnvironment env) 
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .Build();                     
        }

        public void ConfigureServices(IServiceCollection services)
        {           
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration["Data:SportStoreProducts:ConnectionString"]));

            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(Configuration["Data:SportStoreIdentity:ConnectionString"]));
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();
         
            services.AddTransient<IProductRepository, EFProductRepository>();
          
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IOrderRepository, EFOrderRepository>(); 
         
            services.AddMvc(); 
            services.AddMemoryCache(); 
            services.AddSession(); 
        }
      
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) // 
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            // app.UseMvcWithDefaultRoute(); 
            app.UseSession();              
            app.UseIdentity();

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "Error", template: "Error", defaults: new { controller = "Error", action = "Error" });

                routes.MapRoute
               (
                   name: null,
                   template: "{category}/Page{page:int}",
                       defaults: new { controller = "Product", action = "List" }
                       ); 

                routes.MapRoute
                    (
                        name: null,
                        template: "Page{page:int}",
               defaults: new { controller = "Product", action = "List", page = 1 }
               ); 

                routes.MapRoute
                    (
                        name: null,
                        template: "{category}",
               defaults: new { controller = "Product", action = "List", page = 1 }
               ); 

                routes.MapRoute
                    (
                        name: null,
                        template: "",
               defaults: new { controller = "Product", action = "List", page = 1 }
               ); 

                routes.MapRoute
                    (
                        name: null, template: "{controller}/{action}/{id?}");          
            });

            using (var scope = app.ApplicationServices.CreateScope())
            {           
                ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                SeedData.EnsurePopulated(context);
            }
            IdentitySeedData.EnsurePopulated(app); 
                                                   // отключил в Program.cs Scope сервис следующй строкой : 
                                                   // .UseDefaultServiceProvider(options => options.ValidateScopes = false);
        }
    }
}

