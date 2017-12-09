using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Model;
using Model.Repositories;
using Entities;
using WepAPI.Models;
using Extensions;

namespace Gorilla
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
            services.AddDbContext<RedditDBContext>(o =>
             o.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IRedditDBContext, RedditDBContext>();
            services.AddScoped<ISubredditRepository, SubredditRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserPreferenceRepository, UserPreferenceRepository>();
            services.AddScoped<ISubredditConnectionRepository, SubredditConnectionRepository>();

            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                //.RequireRole("Admin", "SuperUser")
                .Build();


            var options = new AzureAdOptions();
            Configuration.Bind("AzureAd", options);
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
          .AddOpenIdConnect(o =>
          {
              o.ClientId = options.ClientId;
              o.Authority = $"{options.Instance}{options.TenantId}";
              o.UseTokenLifetime = true;
              o.CallbackPath = options.CallbackPath;
          })
          .AddJwtBearer(o =>
          {
              o.Authority = options.Authority;  
              o.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateAudience = true,
                  ValidAudiences = new[] { options.ClientId, options.Audience }
              };
          })
          .AddCookie();
            
         
            services.AddRouting(o =>
            {
                o.LowercaseUrls = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Gorilla API", Version = "v1" });
                c.DocumentFilter<LowerCaseDocumentFilter>();
            });

            services.Configure<MvcOptions>(o =>
            {
                o.Filters.Add(new RequireHttpsAttribute());
                o.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddMvc(config =>
            {
                config.RespectBrowserAcceptHeader = true;
                config.InputFormatters.Add(new XmlSerializerInputFormatter());
                config.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gorilla API");
            });
            var options = new RewriteOptions().AddRedirectToHttps();
            app.UseRewriter(options);
            app.UseAuthentication();
            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod());

            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
