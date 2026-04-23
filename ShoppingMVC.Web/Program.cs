using ShoppingMVC.IoC;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoppingMVC.Entidades;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Emit;
using ShoppingMVC.Utilities;
using ShoppingMVC.Datos;

namespace ShoppingMVC.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //var connectionString = 
            //    builder.Configuration.GetConnectionString(
            //        "ShoppingMvcDbContextConnection") ?? 
            //        throw new InvalidOperationException(
            //            "Connection string 'ShoppingMvcDbContextConnection' not found.");

            //    builder.Services.AddDbContext<ShoppingMvcDbContext>(options
            //        => options.UseSqlServer(connectionString));



            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ShoppingMvcDbContext>().AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath= $"/Identity/Account/AccessDenied";
                options.LoginPath = $"/Identity/Account/Login";   // le indico la ruta hacia el login cuando intenta ingresar sin loguarse
                options.LogoutPath= $"/Identity/Account/Logout";
            });

            builder.Services.AddRazorPages();
                                            //Mi aplicación va a usar Razor Pages
                                            //prepará todos los servicios necesarios
                                            // Add services to the container.

            builder.Services.AddControllersWithViews(); // voy a trabajar con las paginas Razor
           
            DI.ConfigurarServicios(builder.Services, builder.Configuration); //ingecto los servicios y  la configuracion con la BD
           
            builder.Services.AddAutoMapper(typeof(Program).Assembly);  //inyecto el automapper
                                                                       //Busca todos los profile del assembly dentro del proyecto web 
            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await SeedRolesAndAdminUser(services);
            }



            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Home/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
                                //si llega una request
                                //buscá si coincide con alguna Razor Page

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        private static async Task SeedRolesAndAdminUser(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (!await roleManager.RoleExistsAsync(WC.Role_Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(WC.Role_Admin));
            }

            if (! await roleManager.RoleExistsAsync(WC.Role_Customer))
            {
                await roleManager.CreateAsync(new IdentityRole(WC.Role_Customer));
            }

            var adminUser = await userManager.FindByEmailAsync("admin@gmail.com");

            if (adminUser== null)
            {
                adminUser = new ApplicationUser
                {

                    UserName = "admin@gmail.com",
                    Email= "admin@gmail.com",
                    EmailConfirmed= true
                };
            await userManager.CreateAsync(adminUser, "Admin123.");  // agrega el usuario admin con esa contraseña

            await userManager.AddToRoleAsync(adminUser, WC.Role_Admin); // agrega el rol admin al usuario

            }
        }
    }
}
