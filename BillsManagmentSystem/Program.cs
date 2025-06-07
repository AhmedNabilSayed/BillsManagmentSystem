using BillsBLL.IReposatories;
using BillsBLL.Reposatories;
using BillsDAL.Context;
using BillsDAL.Identity;
using BillsDAL.Reposatories;
using BillsEntity;
using BillsManagmentSystem.Mapped;
using DevExpress.AspNetCore;
using DevExpress.AspNetCore.Reporting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace BillsManagmentSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Config Of DevExpress
            builder.Services.AddControllersWithViews().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
            builder.Services.AddDevExpressControls();


            builder.Services.ConfigureReportingServices(configurator =>
            {
                configurator.DisableCheckForCustomControllers();
            });


            builder.Services.AddDbContext<BillsDbContext>(option=>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("defaultconnection"));
            });

			builder.Services.AddDbContext<AppIdentityDbContext>(option =>
			{
				option.UseSqlServer(builder.Configuration.GetConnectionString("defaultconnection"));
			});

			builder.Services.AddAutoMapper(map => map.AddProfile(new MappingProfiles()));
            builder.Services.AddScoped(typeof(IgenericReposatory<>) , typeof(GenericReposatory<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IBillDetailsRepository, BillDetailsRepository>();
            builder.Services.AddScoped<IBillHeaderRepository, BillHeaderRepository>();
            builder.Services.AddScoped<IItemsRepository, ItemsRepository>();
            builder.Services.AddScoped<IVendorRepository, VendorRepository>();
            builder.Services.AddScoped<IStoresRepository, StoresRepository>();
            builder.Services.AddScoped<IStockRepository, StockRepository>();
            builder.Services.AddScoped<ISalesBillHeaderRepository, SalesBillHeaderRepository>();
            builder.Services.AddScoped<ISalesBillDetailsRepository, SalesBillDetailsRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

			builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme ,options =>
							{
								options.LoginPath = new PathString("/Account/LogIn");
								options.AccessDeniedPath = new PathString("/Home/Error");
							});

            var app = builder.Build();

            #region Update Database inside Main
            var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var ILoggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                var dbCotenxt = services.GetRequiredService<BillsDbContext>();
                await dbCotenxt.Database.MigrateAsync();

                var identityDbContext = services.GetRequiredService<AppIdentityDbContext>();
                await identityDbContext.Database.MigrateAsync();

                var userManager = services.GetRequiredService<UserManager<AppUser>>();

                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                await AppIdentityDbContextSeed.SeedUserAsync(userManager);
                await AppIdentityDbContextSeed.SeedRolesAsync(roleManager);

            }catch(Exception ex)
            {
                var ILogger = ILoggerFactory.CreateLogger<Program>();
                ILogger.LogError(ex.Message, "an error occured during apply the migration");
            }
            #endregion

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "node_modules")),
                RequestPath = "/node_modules"
            });

            app.UseRouting();
            app.UseAuthentication();    
            app.UseAuthorization();
            app.UseDevExpressControls();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }
    }
}