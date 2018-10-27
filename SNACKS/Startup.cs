using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SNACKS.Data;
using SNACKS.Models;

namespace SNACKS
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDefaultIdentity<IdentityUser>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<IRepositorioBase<Usuario>, RepositorioBase<Usuario>>();
            services.AddScoped<IRepositorioBase<Persona>, RepositorioBase<Persona>>();
            services.AddScoped<IRepositorioBase<Unidad>, RepositorioBase<Unidad>>();
            services.AddScoped<IRepositorioBase<Categoria>, RepositorioBase<Categoria>>();
            services.AddScoped<IRepositorioBase<Producto>, RepositorioBase<Producto>>();
            services.AddScoped<IRepositorioBase<ItemProducto>, RepositorioBase<ItemProducto>>();
            services.AddScoped<IRepositorioBase<Pedido>, RepositorioBase<Pedido>>();
            services.AddScoped<IRepositorioBase<ItemPedido>, RepositorioBase<ItemPedido>>();
            services.AddScoped<IRepositorioBase<IngresoInsumo>, RepositorioBase<IngresoInsumo>>();
            services.AddScoped<IRepositorioBase<ItemIngresoInsumo>, RepositorioBase<ItemIngresoInsumo>>();
            services.AddScoped<IRepositorioBase<IngresoProducto>, RepositorioBase<IngresoProducto>>();
            services.AddScoped<IRepositorioBase<ItemIngresoProducto>, RepositorioBase<ItemIngresoProducto>>();
            services.AddScoped<IRepositorioBase<SalidaInsumo>, RepositorioBase<SalidaInsumo>>();
            services.AddScoped<IRepositorioBase<ItemSalidaInsumo>, RepositorioBase<ItemSalidaInsumo>>();
            services.AddScoped<IRepositorioBase<SalidaProducto>, RepositorioBase<SalidaProducto>>();
            services.AddScoped<IRepositorioBase<ItemSalidaProducto>, RepositorioBase<ItemSalidaProducto>>();
            services.AddScoped<IRepositorioBase<Reporte>, RepositorioBase<Reporte>>();
            services.AddScoped<IRepositorioBase<ItemReporte>, RepositorioBase<ItemReporte>>();
            services.AddScoped<IRepositorioBase<InventarioInsumo>, RepositorioBase<InventarioInsumo>>();
            services.AddScoped<IRepositorioBase<InventarioProducto>, RepositorioBase<InventarioProducto>>();
            services.AddScoped<IRepositorioBase<ZonaVenta>, RepositorioBase<ZonaVenta>>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc().AddJsonOptions(configureJson);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        public void configureJson(MvcJsonOptions obj)
        {
            obj.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
