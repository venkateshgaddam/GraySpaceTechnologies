using Amazon;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Amazon.SimpleSystemsManagement;
using Microsoft.Extensions.Configuration;
using Amazon.SimpleSystemsManagement.Model;
using ZingTickets.Common;
using ZingTickets.API;
using Microsoft.AspNetCore.Mvc;
using ZingTickets.MovieManagement.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;

namespace ZingTickets.MovieManagement;

public class Startup : BaseStartup
{
    public Startup(IConfiguration configuration) : base(configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container
    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        services.AddAmazonSystemsManager();
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                                new HeaderApiVersionReader("x-api-version"),
                                                                new MediaTypeApiVersionReader("x-api-version"));
        });
        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        //services.AddSwaggerGen(c =>
        //{
        //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Movies Management API V1", Version = "v1" });
        //    c.SwaggerDoc("v2", new OpenApiInfo { Title = "Movies Management API V2", Version = "v2" });

        //    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        //    {
        //        Name = "Authorization",
        //        Type = SecuritySchemeType.ApiKey,
        //        Scheme = "Bearer",
        //        BearerFormat = "JWT",
        //        In = ParameterLocation.Header,
        //        Description = "JWT Authorization header using the Bearer scheme."
        //    });
        //    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        //        {
        //            {
        //            new OpenApiSecurityScheme
        //            {
        //                Reference = new OpenApiReference
        //                    {
        //                        Type = ReferenceType.SecurityScheme,
        //                        Id = "Bearer"
        //                    }
        //            },
        //                Array.Empty<string>()
        //            }
        //        });
        //});
        services.ConfigureOptions<ConfigureSwaggerOptions>();
        services.AddCognitoIdentity();
        services.AddAuthentication(a =>
        {
            a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            var authority = Configuration[GlobalConstants.COGNITO_VALID_AUTHORITY];
            options.Authority = authority;
            options.Audience = Configuration[GlobalConstants.APP_CLIENT_ID_KEY] ?? string.Empty;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = authority,
                ValidateAudience = false,
                ValidateLifetime = true,
                LifetimeValidator = isExpired
            }; ;
        });
    }


    private bool isExpired(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
    {
        return expires < DateTime.Now;
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        base.Configure(app, env);
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        var apiVersionDescriptionProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();


        app.UseHttpsRedirection();
        app.UseApiVersioning();
        app.UseRouting();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            }
        });

        //    app.UseSwaggerUI(c =>
        //{
        //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movies API V1");
        //    c.SwaggerEndpoint("/swagger/v2/swagger.json", "Movies API V2");
        //});
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }




    public class ConfigureSwaggerOptions
    : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(
            IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Configure each API discovered for Swagger Documentation
        /// </summary>
        /// <param name="options"></param>
        public void Configure(SwaggerGenOptions options)
        {
            // add swagger document for every API version discovered
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    CreateVersionInfo(description));
            }
        }

        /// <summary>
        /// Configure Swagger Options. Inherited from the Interface
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        public void Configure(string name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        /// <summary>
        /// Create information about the version of the API
        /// </summary>
        /// <param name="description"></param>
        /// <returns>Information about the API</returns>
        private OpenApiInfo CreateVersionInfo(
                ApiVersionDescription desc)
        {
            var info = new OpenApiInfo()
            {
                Title = ".NET Core (.NET 6) Web API",
                Version = desc.ApiVersion.ToString()
            };

            if (desc.IsDeprecated)
            {
                info.Description += " This API version has been deprecated. Please use one of the new APIs available from the explorer.";
            }

            return info;
        }
    }

}