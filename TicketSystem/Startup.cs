using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Repositories;
using TicketSystem.IRepositories;
using TicketSystem.Services;
using TicketSystem.FIlters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using TicketSystem.Profiles;
using System.Net;
using TicketSystem.Authorizations;
using Microsoft.AspNetCore.Authorization;

namespace TicketSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) 
        {
            services.AddControllersWithViews();
            
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(SystemFilter));
            });

            services.AddHttpContextAccessor();

            services.AddDbContext<TicketContext>(optionBuilder =>
            {
                optionBuilder.UseSqlServer(Configuration.GetConnectionString("TicketConn"));
            });

            services.AddAutoMapper(config =>
            {
                config.AddProfile<ControllerMappings>();
            });
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // 啟用Session
            services.AddDistributedMemoryCache(); // 使用內存記憶體來記錄Session
            services.AddSession();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)  
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/User/Login");
                    options.LogoutPath = new PathString("/User/Logout");
                    options.AccessDeniedPath = new PathString("/User/NoPermission"); // 沒有權限
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.SlidingExpiration = false;                
                });

            services.AddAuthorization(option =>
            {
                option.AddPolicy("P_Create", policy =>
                {
                    policy.RequireRole(new string[] {"Pm","Qa"})
                        .Requirements.Add(new ProblemCreateRequireMent());
                });
                option.AddPolicy("P_Solve", policy =>
                 {
                     policy.RequireRole(new string[] { "Rd", "Qa" })
                        .Requirements.Add(new ProblemSolveRequireMent());
                 });
            });
            services.AddScoped<IAuthorizationHandler, ProblemCreateHandler>();
            services.AddScoped<IAuthorizationHandler, ProblemSolveHandler>();


            // 泛行倉庫
            services.AddScoped(typeof(ITRepository<>),typeof(TRepository<>));
            //services.AddTransient<IUserService, UserService>();
            services.AddScoped<UserService>();
            services.AddScoped<RoleService>();
            services.AddScoped<LoginService>();
            services.AddScoped<PriorityService>();
            services.AddScoped<SeverityService>();
            services.AddScoped<ProblemService>();
            services.AddScoped<ProblemCatrgoryService>();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession(); // 啟用Session

            app.UseAuthentication(); // 驗證
            app.UseAuthorization(); // 授權

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=User}/{action=Login}/{id?}");
            });
        }
    }
}
