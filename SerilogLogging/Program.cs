using DataAccess.Contexts;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Services;
using System.Collections.ObjectModel;
using System.Configuration;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Connection Configuration
        builder.Services.AddDbContext<AppDbContext>
            (option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


        // Doctor Services
        builder.Services.AddScoped<IBaseService<VCard>, BaseService<VCard>>();
        builder.Services.AddScoped<IVCardService,VCardService>();


        #region Configure serilog

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();


        Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.Debug(outputTemplate: DateTime.Now.ToString())
        .WriteTo.Seq("http://localhost:5341/")
        .ReadFrom.Configuration(configuration)
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                         .CreateLogger();

        // Add services to the container.
        //var logger = new LoggerConfiguration()
        //  .ReadFrom.Configuration(builder.Configuration)
        //  .Enrich.FromLogContext()
        //  .CreateLogger();
        //builder.Logging.ClearProviders();
        //builder.Logging.AddSerilog(logger);


        builder.Host.UseSerilog();

        #endregion

        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseSerilogRequestLogging();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Card}/{action=Index}/{id?}");

        app.Run();
    }
}

#region Configure serilog


public class CustomUserNameColumn : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var (username, value) = logEvent.Properties.FirstOrDefault(x => x.Key == "UserName");
        if (value != null)
        {
            var getValue = propertyFactory.CreateProperty(username, value);
            logEvent.AddPropertyIfAbsent(getValue);
        }
    }
}

#endregion
