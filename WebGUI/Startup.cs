﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessLayer.Context;
using DataAccessLayer.IServices;
using DataAccessLayer.Sevices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using WebGUI.Data;
using WebGUI.Mapping;
using WebGUI.Models;
using WebGUI.SignalRClass;
namespace WebGUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ILogService, LogService>();
            services.AddTransient<IRoomService, RoomService>();
            services.AddTransient<IDeviceService, DeviceService>();
            services.AddTransient<ICommunicationUnitService, CommunicationUnitService>();
            services.AddSingleton<IMapper>(MapperConfig.Configure());

            services.AddMvc();
            services.AddSignalR();
            // services.AddDbContext<HeatingContext>(opt => opt.UseSqlServer(@"Server = (localdb)\mssqllocaldb; Database=HeatingController;Trusted_Connection=True;"));
            services.AddDbContextPool<HeatingContext>( // replace "YourDbContext" with the class name of your DbContext
               //options => options.UseMySql("Server=192.168.43.143;Database=HeatingController;User=heatingcontroluser;Password=1werwerwer;", // replace with your Connection String
               options => options.UseMySql("Server=localhost;Database=HeatingController;User=root;Password=1werwerwer;", // replace with your Connection String
                    mySqlOptions =>
                    {
                        mySqlOptions.ServerVersion(new Version(8, 0, 15), ServerType.MySql); // replace with your Server Version and Type
                    }
            ));
            services.AddDbContextPool<ApplicationDbContext>( 
               // options => options.UseMySql("Server=192.168.43.143;Database=Identity;User=heatingcontroluser;Password=1werwerwer;", // replace with your Connection String
                options => options.UseMySql("Server=localhost;Database=Identity;User=root;Password=1werwerwer;", // replace with your Connection String
                    mySqlOptions =>
                    {
                        mySqlOptions.ServerVersion(new Version(8, 0, 15), ServerType.MySql); 
                    }
            ));
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
           .AddEntityFrameworkStores<ApplicationDbContext>()
           .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options => options.LoginPath = "/Login/");
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownProxies.Add(IPAddress.Parse("192.168.43.143"));
            });

            services.AddSignalR(o =>
            {
                o.EnableDetailedErrors = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {



            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }


            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChartHub>("/chartHub");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
