using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using HASmart.Infrastructure.EFDataAccess;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using HASmart.Core;
using HASmart.Core.Repositories;
using HASmart.Core.Services;
using HASmart.Core.Validation;
using HASmart.Infrastructure.EFDataAccess.Repositories;
using System.Collections.Generic;
using HASmart.WebApi.FiltrosSwagger;

namespace HASmart.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.Configuration = configuration;
            this.Environment = environment;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            string tokenUrl;

            if (this.Environment.IsDevelopment())
            {
                services.AddDbContext<AppDBContext>(opt => opt.UseInMemoryDatabase("HASmartDB"));
                IdentityModelEventSource.ShowPII = true;

                services.AddAuthentication("Bearer")
                    .AddJwtBearer("Bearer", options =>
                    {
                        options.Authority = "http://localhost:5050";
                        options.RequireHttpsMetadata = false;
                        options.Audience = "hasmartapi";
                    });

                tokenUrl = "http://localhost:5050";
            }
            else
            {
                services.AddDbContext<AppDBContext>(opt => opt.UseSqlServer("Server="+System.Environment.GetEnvironmentVariable("DB_Server")+";Database=HASmartDB;User Id=sa;Password=P455w0rd;"));
                //services.AddDbContext<AppDBContext>(opt => opt.UseSqlServer("tcp=190.190.200.100,1433;Network Library=DBMSSOCN;Initial Catalog=HASmartDB;User Id=SA;Password=P455w0rd;"));
                services.AddAuthentication("Bearer")
                    .AddJwtBearer("Bearer", options =>
                    {
                        options.Authority = "http://is:5050";
                        options.RequireHttpsMetadata = false;
                        options.Audience = "hasmartapi";
                    });

                tokenUrl = "http://168.138.151.95:5050";
            }

            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            services.AddTransient(typeof(ICidadaoRepository), typeof(CidadaoRepository));
            services.AddTransient(typeof(IFarmaciaRepository), typeof(FarmaciaRepository));
            services.AddTransient(typeof(IMedicoRepository), typeof(MedicoRepository));
            services.AddTransient(typeof(CidadaoService));
            services.AddTransient(typeof(FarmaciaService));
            services.AddTransient(typeof(MedicoService));
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new CustomDateTimeConverter());
            }
            );
 

            // Registre o gerador Swagger, definindo 1 ou mais documentos Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("vs1", new OpenApiInfo
                {
                    Version = "vs1",
                    Title = "API REST do Projeto HASmart",
                    Description = "Documentação da API para controle de dados de hipertensão arterial da população do estado do Ceará.",
                    TermsOfService = new Uri("https://hasmart.unifor.br/"),
                    Contact = new OpenApiContact
                    {
                        Name = "Equipe UNIFOR",
                        Email = string.Empty,
                        Url = new Uri("https://hasmart.unifor.br/desenvolvedores/"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Uso sobre direitos",
                        Url = new Uri("https://hasmart.unifor.br/license"),
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.SchemaFilter<EsquemaValorPadrao>();

                // Definindo uso de Client Credentials para o Swagger
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        ClientCredentials = new OpenApiOAuthFlow
                        {
                            // TODO Receber infos do ambiente e ajustar portas 
                            TokenUrl = new Uri($"{tokenUrl}/connect/token", UriKind.Absolute),
                            Scopes = new Dictionary<string, string>
                            {
                                { "hasmartapi", "API para sistema de controle de dados de Hipertensão Arterial"}
                            }
                        }
                    }
                });

                // Aplicando a definicao para todas as operacoes
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                        },
                        new[] { "hasmartapi" }
                    }
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

            // Ativa o middleware para servir o Swagger gerado como um terminal JSON.
            app.UseSwagger();

            // Ativa o middleware para exibir o swagger-ui (HTML, JS, CSS etc.),
            // especificando o terminal JSON do Swagger.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/vs1/swagger.json", "Documentação API");
                c.DefaultModelsExpandDepth(-1);
                
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

}