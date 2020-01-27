using FlutterTraining.Helper;
using FlutterTraining.Models;
using FlutterTraining.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System.Globalization;
using System.Text;

namespace FlutterTraining
{
    public class Startup
    {
        public Resource resource;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var connectionString = Configuration.GetConnectionString("ApplicationDbContext");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddScoped<IAuthService, AuthService>();

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Info
                {
                    Title = "SBI API"
                });
                x.OperationFilter<SwaggerHeader>();
            });
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });

            app.MapWhen(
                delegate (HttpContext context)
                {
                    handleRequest(context);
                    return false;
                }, handleMapWhen
                );

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("../swagger/v1/swagger.json", "SBI Core API");
            });


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void handleMapWhen(IApplicationBuilder app)
        {

        }

        //handle request untuk set culture info karena ada issue al.exe not found 
        //saat menggunakan resource .resx pada system globalization 
        //jadi diganti dengan sistem resource class 
        private void handleRequest(HttpContext context)
        {
            //set default culture
            CultureInfo culture = new CultureInfo("id-ID");

            //read header
            culture = !string.IsNullOrEmpty(context.Request.Headers["lang"].ToString()) ? new CultureInfo(context.Request.Headers["lang"]) : culture;

            //read query string
            culture = !string.IsNullOrEmpty(context.Request.Query["lang"].ToString()) ? new CultureInfo(context.Request.Query["lang"]) : culture;

            //set culture
            CultureInfo.CurrentCulture = culture;

            //init resource
            GlobalData.get.resource = Resource.Lang(CultureInfo.CurrentCulture.Name.Split('-')[0]);

        }
    }
}
