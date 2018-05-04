using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GerenciadorFC.Administrativo.Web.Data;
using GerenciadorFC.Administrativo.Web.Models;
using GerenciadorFC.Administrativo.Web.Services;
using AutoMapper;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace GerenciadorFC.Administrativo.Web
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
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

			services.AddAutoMapper();

			services.AddMvc();

		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

			//Fixar Cultura para pt-BR
			RequestLocalizationOptions localizationOptions = new RequestLocalizationOptions
			{
				SupportedCultures = new List<CultureInfo> { new CultureInfo("pt-BR") },
				SupportedUICultures = new List<CultureInfo> { new CultureInfo("pt-BR") },
				DefaultRequestCulture = new RequestCulture("pt-BR")
			};
			app.UseRequestLocalization(localizationOptions);
		}
    }
}
