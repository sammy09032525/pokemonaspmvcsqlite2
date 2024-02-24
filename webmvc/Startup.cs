using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using webmvc.Infrastructure;
using webmvc.Models;

namespace webmvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. 
        // Use this method to add services to the container. 
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            var application = Configuration.GetSection("AppSettings:Application");
            services.Configure<Application>(application);

            services.Configure<IdentityOptions>(options => { 
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                options.ClaimsIdentity.UserNameClaimType = ClaimTypes.Name;

                options.Password.RequiredLength = 1;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 1;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "PokemonCookie";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(5);
            });

            services.AddDbContext<MyDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")), 
                ServiceLifetime.Singleton
                );

            services.AddIdentity<User, IdentityRole<int>>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
                .AddEntityFrameworkStores<MyDbContext>();

            services.AddControllersWithViews(options => options.EnableEndpointRouting = false);
            services.AddRazorPages(options => { });
            services.AddSingleton<PokeApiNet.PokeApiClient>();
        }
        
        // This method gets called by the runtime.  
        // Use this method to configure the HTTP request pipeline. 
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
            app.UseRouting();
            //app.MapControllerRoute(
            //    name: "default",
            //    pattern: "{controller=Home}/{action=Index}/{id?}");
            app.UseEndpoints(endpoints => { 
                endpoints.MapRazorPages();
            });
//            app.MapRazorPages();
        }
    }
}