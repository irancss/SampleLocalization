using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SampleLocalization.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SampleLocalization
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
            services.AddControllersWithViews().AddViewLocalization();
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });
            services.AddMvc()
                 .AddDataAnnotationsLocalization(options =>
                 {
                     options.DataAnnotationLocalizerProvider = (type, factory) =>
                         factory.Create(typeof(SharedResource));
                 })
                 .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                 .AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(
               options =>
               {
                   var supportedCultures = new List<CultureInfo>
                   {
                        new CultureInfo("en-US"),
                        new CultureInfo("fa-IR")
                   };

                   options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                   options.SupportedCultures = supportedCultures;
                   options.SupportedUICultures = supportedCultures;
                   options.RequestCultureProviders = new[] { new Extensions.RouteDataRequestCultureProvider { IndexOfCulture = 1, IndexofUICulture = 1 } };
               });

            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("culture", typeof(LanguageRouteConstraint));
            });


            //Service zir agar nabashe error mikhore . fght gozashtam run beshe
            services.AddScoped<RequestLocalizationOptions, RequestLocalizationOptions>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env , RequestLocalizationOptions localizationOptions)
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

            var localizeOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizeOptions.Value);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{culture:culture}/{controller=Home}/{action=Index}/{id?}");
                
                endpoints.MapGet("{culture:culture}/{*path}", appBuilder =>
                {
                    // inja darkhasthaee miad ke culture daran ama be hichkodum az contoller ha naraftan
                    return Task.CompletedTask;
                });
                endpoints.MapGet("{*path}", (RequestDelegate)(ctx =>
                {
                    // injam darkhasthaee ke culture nadaran redirect mishan be clture pish farz
                    var defaultCulture = localizeOptions.Value.DefaultRequestCulture.Culture.Name;
                    var path = ctx.GetRouteValue("path") ?? string.Empty;
                    var culturedPath = $"/{defaultCulture}/{path}";
                    ctx.Response.Redirect(culturedPath);
                    return Task.CompletedTask;
                }));
            });
        }
    }
}
