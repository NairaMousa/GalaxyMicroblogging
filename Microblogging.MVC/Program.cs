
using Microblogging.Helper.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using NLog.Web;



var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add authentication services
    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // total timeout
            options.SlidingExpiration = true; // ?? this enables session extension
            options.LoginPath = "/Account/Login";
        });

    // Add services and NLog as logging provider
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    builder.Services.AddSession();
    builder.WebHost.UseWebRoot("wwwroot");
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddHttpClient();
    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.Configure<MVCAppSettings>(builder.Configuration.GetSection("AppSettings"));
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();
    app.UseSession();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{action=Login}");

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Stopped program due to exception");
    throw;
}







finally
{
    NLog.LogManager.Shutdown();
}