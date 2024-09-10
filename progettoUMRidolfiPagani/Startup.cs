using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using progettoUMRidolfiPagani.Services; // Namespace dei Services
using progettoUMRidolfiPagani.Services.Interface;
using progettoUMRidolfiPagani.Repository;
using Microsoft.EntityFrameworkCore;

namespace progettoUMRidolfiPagani
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
            // Configurazione del DbContext con SQL Server 
            services.AddDbContext<MagazzinoDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            // Registrazione dei servizi personalizzati
            services.AddScoped<IArticoloService, ArticoloService>();
            services.AddScoped<IMovimentoService, MovimentoService>();
            services.AddScoped<IMagazzinoService, MagazzinoService>();
            services.AddScoped<IStoricoService, StoricoService>();
            services.AddScoped<IDashboardService, DashboardService>();

            // Configurazione MVC
            services.AddControllersWithViews();
            services.AddRazorPages();

            // Configurazione di SignalR se necessaria
            services.AddSignalR();

            // Configurazione di CORS (se necessario)
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Configurazione CORS
            app.UseCors("AllowAllOrigins");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // Definizione delle route per i controller
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Dashboard}/{action=Index}/{id?}");
                endpoints.MapRazorPages();

            });
        }
    }
}
