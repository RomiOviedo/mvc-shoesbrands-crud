using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoppingMVC.Datos;
using ShoppingMVC.Datos.Interfaces;
using ShoppingMVC.Datos.Repositorios;
using ShoppingMVC.Entidades;
using ShoppingMVC.Servicios.Interfaces;
using ShoppingMVC.Servicios.Servicios;
using ShoppingMVC.Utilities;

namespace ShoppingMVC.IoC       // Inversion de Control
{
    public class DI             // Inyeccion de dependencias
    {

        public static void ConfigurarServicios(IServiceCollection servicios, IConfiguration configuration) //IServiceCollection= contenedor de serv, donde se registran los serv q seran inyectados
        {                                                                                                  //IConfiguration= lee las configuraciones de archivos asppsettin.json, como la cadena de coneccion 

            servicios.AddScoped<IRepositorioBrand, RepositorioBrand>(); // addScopen= agregar Ambito 
                                                                        // registra la relacio Interface -> implementacion
                                                                        // con ciclo de vida= SCOPED => Cada solicitud HTTP obtendra una nueva instancia de estas clases

            servicios.AddScoped<IServicioBrand, ServicioBrand>();


            servicios.AddScoped<IRepositorioShoe, RepositorioShoe>();
            servicios.AddScoped<IServicioShoe, ServicioShoe>();

            servicios.AddScoped<IUnitOfWork, UnitOfWork>();

            servicios.AddScoped<IRepositorioApplicationUser, RepositorioApplicationUser>();
            servicios.AddScoped<IServicioApplicationUser, ServicioAplicationUser>();

            servicios.AddScoped<IRepositorioShoppingCart, RepositorioShoppingCart>();
            servicios.AddScoped<IServicioShoppingCart, ServicioShoppingCart>();

            servicios.AddScoped<IRepositorioOrderDetail, RepositorioOrderDetail>();
            servicios.AddScoped<IServicioOrderDetail, ServicioOrderDetail>();






            servicios.AddDbContext<ShoppingMvcDbContext>(options =>
            {
                options.UseSqlServer
                (configuration.GetConnectionString("MyConnection"));
            });

            servicios.AddScoped<IEmailSender, EmailSender>();

        }


    }
}
