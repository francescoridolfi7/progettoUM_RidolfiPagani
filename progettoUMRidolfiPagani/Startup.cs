using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.EntityFrameworkCore;
using progettoUMRidolfiPagani.Repository;
using progettoUMRidolfiPagani.Services;
using progettoUMRidolfiPagani.Services.Interface;
using System;

namespace progettoUMRidolfiPagani
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Questo metodo viene chiamato dal runtime. Utilizzare per aggiungere servizi al contenitore.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configurazione del DbContext con MySQL
            services.AddDbContext<MagazzinoDbContext>(options =>
                options.UseMySql(
                    Configuration.GetConnectionString("DefaultConnection"),
                    new MySqlServerVersion(new Version(8, 0, 21)) // Versione di MySQL
                ));

            // Aggiungi Identity e collegalo a MagazzinoDbContext
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                // Configura opzioni di Identity
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<MagazzinoDbContext>()
            .AddDefaultTokenProviders();

            // Registrazione dei servizi personalizzati
            services.AddScoped<IArticoloService, ArticoloService>();
            services.AddScoped<IMovimentoService, MovimentoService>();
            services.AddScoped<IPosizioneService, PosizioneService>();
            services.AddScoped<IDashboardService, DashboardService>();

            // Aggiungi i servizi MVC e Razor
            services.AddControllersWithViews();
            services.AddRazorPages();

            // Configurazione SignalR
            services.AddSignalR();

            // Serializzatore JSON per preservare i riferimenti circolari
            services.AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                });

            // Configurazione CORS (facoltativo, se necessario)
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Timeout della sessione
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }

        // Questo metodo viene chiamato dal runtime. Utilizzare per configurare il middleware della pipeline HTTP.
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

            // Configura CORS
            app.UseCors("AllowAllOrigins");

            app.UseSession();

            // Abilita autenticazione e autorizzazione
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // Definizione delle route per i controller
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
