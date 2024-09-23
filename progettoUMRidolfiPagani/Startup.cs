using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure; 
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using progettoUMRidolfiPagani.Repository;
using progettoUMRidolfiPagani.Services.Interface;
using progettoUMRidolfiPagani.Services;

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
            // Configurazione del DbContext con MySQL 
            services.AddDbContext<MagazzinoDbContext>(options =>
             options.UseMySql(
                 Configuration.GetConnectionString("DefaultConnection"),
                 new MySqlServerVersion(new Version(8, 0, 21)) // Specifica la versione di MySQL
             ));

            // Registrazione dei servizi personalizzati
            services.AddScoped<IArticoloService, ArticoloService>();
            services.AddScoped<IMovimentoService, MovimentoService>();
            services.AddScoped<IPosizioneService, PosizioneService>();
            services.AddScoped<IStoricoService, StoricoService>();
            services.AddScoped<IDashboardService, DashboardService>();

            // Configurazione MVC
        services.AddControllersWithViews();
            services.AddRazorPages();

            // Configurazione di SignalR 
            services.AddSignalR();

            //serializzatore JSON per preservare i riferimenti circolari
            services.AddControllersWithViews()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
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
                pattern: "{controller=Articoli}/{action=Index}/{id?}");
                endpoints.MapRazorPages();

        });
    }
}
}
