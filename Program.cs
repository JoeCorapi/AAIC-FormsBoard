using System;
using System.Threading.Tasks;
using Serilog;
using Blazored.Modal;
using Syncfusion.Blazor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.ApplicationInsights.Extensibility;
using FormsBoard.Infrastructure.Configuration;
using FormsBoard.Infrastructure.Services;
using FormsBoard.Application.Interfaces;
using FormsBoard.Application.Services;
using FormsBoard.Data.Context;
using Microsoft.EntityFrameworkCore;
using FormsBoard.Data.Repositories;
using Microsoft.Extensions.Caching.Memory;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Application Initializing...");

try
{
    var builder = WebApplication.CreateBuilder(args);


    // Core authentication setup with Microsoft.Identity.Web
    builder.Services.AddAuthentication(options =>
    {
        // Set default scheme for cookie-based authentication
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        // Set default scheme for challenging unauthenticated requests
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        // This is the critical one you're missing for sign-out
        options.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddMicrosoftIdentityWebApp(options =>
    {

        builder.Configuration.GetSection("AzureAd").Bind(options);

        var postLogoutRedirectUri = builder.Configuration["AzureAd:PostLogoutRedirectUri"];

        // Add this event to handle the final redirect
        options.Events = new OpenIdConnectEvents
        {
            OnSignedOutCallbackRedirect = context =>
            {
                // Instead of using the built-in redirect, use your custom one
                context.Response.Redirect(postLogoutRedirectUri);
                context.HandleResponse(); // This prevents further processing
                return Task.CompletedTask;
            },
        };
    });

    builder.Services.AddControllersWithViews()
        .AddMicrosoftIdentityUI();

    // Make authentication required for ALL endpoints
    builder.Services.AddAuthorization(options =>
    {
        // This is the key part - it sets a default policy that requires authentication
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
    });

    builder.Services.AddApplicationInsightsTelemetry(options =>
    {
        options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
    });

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")));

    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor()
        .AddMicrosoftIdentityConsentHandler()
        .AddHubOptions(o => { o.MaximumReceiveMessageSize = 102400000; });
    builder.Services.AddSyncfusionBlazor();
    builder.Services.AddBlazoredModal();
    builder.Services.AddSingleton(builder.Configuration.GetSection("MailSettings").Get<MailSettings>());
    builder.Services.AddScoped<IToastService, ToastService>();
    builder.Services.AddScoped<IMailService, MailService>();
    // Add repositories and services
    builder.Services.AddScoped<IMileageFormRepository, MileageFormRepository>();
    builder.Services.AddScoped<IMileageService, MileageService>();

    // Configure Serilog
    builder.Host.UseSerilog((ctx, serviceProvider, cfg) =>
    {
        // Get the service provider so we can access the registered TelemetryConfiguration
        var telemetryConfig = serviceProvider.GetRequiredService<TelemetryConfiguration>();


        cfg.ReadFrom.Configuration(ctx.Configuration)
           .WriteTo.ApplicationInsights(
               telemetryConfig,
               TelemetryConverter.Traces);
    });

    var app = builder.Build();

    // Ensure database is created and migrations are applied
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
    }

    // Register Syncfusion license
    Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NBaF5cXmZCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXxfeHVSQ2dcUUF/WEc=");

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    //app.UseSerilogRequestLogging();

    // These MUST come before endpoints are mapped

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    // Now any endpoint mapping will require authentication by default
    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}
