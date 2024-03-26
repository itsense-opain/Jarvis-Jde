using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos;
using Opain.Jarvis.Servicios.ConexionJDE.Helpers;
using Opain.Jarvis.Servicios.ConexionJDE.Repositorio;
using Opain.Jarvis.EnvioCorreos;
using Swashbuckle.AspNetCore.Swagger;
using Serilog;

namespace Opain.Jarvis.Servicios.ConexionJDE
{
    public class Startup
    {
        private readonly string politicaCors = "PoliticaJarvis";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("Config");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();

            var originCors = appSettings.OriginCors;
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var Issuer = appSettings.Issuer;
            var Audience = appSettings.Audience;

            services.AddCors(options => options
            .AddPolicy(politicaCors, builder => builder.WithOrigins(originCors)
            .AllowAnyHeader()
            .AllowAnyMethod()
            ));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(Options =>
            {
                Options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
            });

            services.AddSingleton<IConfiguration>(Configuration);


            services.AddDbContext<ContextoOpain>(options => options.UseMySQL(Configuration.GetConnectionString("ConexionJarvisBD")));
            services.AddIdentity<Usuario, Rol>()
                  .AddEntityFrameworkStores<ContextoOpain>()
                  .AddDefaultTokenProviders();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(cfg =>
                {
                    cfg.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = false;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = Issuer,
                        ValidAudience = Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });

            services.AddScoped<RoleManager<Rol>>();
            services.AddScoped<UserManager<Usuario>>();
            services.AddScoped<SignInManager<Usuario>>();

            services.AddTransient<EjecutorInforme>();
            services.AddTransient<EjecutorIntegracion>();

            
            services.AddTransient<Simulacion>();
            services.AddTransient<IEmailSender, EmailSender>();
            var sendGridConfiguracion = Configuration.GetSection("SendGrid");
            services.Configure<AuthMessageSenderOptions>(sendGridConfiguracion);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "OPAIN Servicios API JDE",
                    Description = "Servicios web API JDE",
                    Contact = new Contact
                    {
                        Name = "Componente Serviex",
                        Url = "https://www.opain.co/"
                    },
                    License = new License
                    {
                        Name = "Use bajo licencia",
                        Url = "https://www.opain.co/"
                    }
                });

                var archivoXml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var rutaXml = Path.Combine(AppContext.BaseDirectory, archivoXml);
                c.IncludeXmlComments(rutaXml);

                c.AddSecurityDefinition("Authorization", new ApiKeyScheme
                {
                    Description = "Authorization by API key.",
                    In = "header",
                    Type = "apiKey",
                    Name = "Authorization"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Authorization", new string[0] }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseSerilogRequestLogging();
            app.UseSwagger();
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "OPAIN JARVIS Servicios JDE API V1");
            });

            app.UseCors(politicaCors);

            app.UseAuthentication();
            app.UseSerilogRequestLogging();

            app.UseMvc();
        }
    }
}
