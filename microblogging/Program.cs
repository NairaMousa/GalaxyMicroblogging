using Hangfire;
using Hangfire.SqlServer;
using Microblogging.API;
using Microblogging.API.HangFire;
using Microblogging.API.JWT;
using Microblogging.Data.Entities;
using Microblogging.Helper.Models;
using Microblogging.Repository.IRepo;
using Microblogging.Repository.Repo;
using Microblogging.Service;
using Microblogging.Service.IServices;
using Microblogging.Service.Services;
using Microblogging.Services.IService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using System.Text;



var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    // Add services and NLog as logging provider
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    #region Injections
    builder.Services.AddDbContext<microbloggingContext>(options =>
                     options.UseSqlServer(builder.Configuration.GetConnectionString("MicrobloggingConnectionString")));


    builder.Services.AddScoped<IPostService, PostService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IImageService, ImageService>();

    builder.Services.AddScoped<IContainerRepo, ContainerRepo>();
    #endregion
    #region JWT Injection







    // Add JWT config (you can also pull from appsettings.json)
    var jwtKey = builder.Configuration["JwtSettings:SecretKey"] ?? "YourSuperSecretKey!";
    var jwtIssuer = builder.Configuration["JwtSettings:Issuer"] ?? "YourApp";

    // Configure authentication
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

    #endregion
    #region Hangfire
    builder.Services.AddHangfire(config =>
    {
        config.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"));
    });

    // Register the Hangfire server
    builder.Services.AddHangfireServer();




    #endregion


    builder.Services.AddAuthorization();
    builder.Services.AddControllers();

    #region Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

        // Define the security scheme
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });

        // Add security requirement
        c.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference{
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
        });
    });

    #endregion


    builder.Services.AddScoped<StartupJobInitializer>();
    builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
    builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
    builder.Services.AddAutoMapper(typeof(ServiceAutoMapper).Assembly);

    var app = builder.Build();




    using (var scope = app.Services.CreateScope())
    {
        var initializer = scope.ServiceProvider.GetRequiredService<StartupJobInitializer>();
        initializer.Schedule();
    }

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseMiddleware<TokenExpirationMiddleware>();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();


   

    app.UseHangfireDashboard("/hangfire");
    app.Run();
}
catch(Exception ex)
{
    logger.Error(ex, "Stopped program due to exception");
    throw;
}







finally
{
    NLog.LogManager.Shutdown();
}


