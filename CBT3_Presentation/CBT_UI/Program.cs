using CBT_UI.Components;
using CBT3_Shared.Configuration;
using CBT3_Application.Configuration;
using CBT3_Infrastructure.Configuration;
using Radzen;
using CBT3_Application.Services;
using CBT3_Application.Interfaces;
using System.Reflection;
using CBT3_Application.Configuration;
using CBT3_UI;

namespace CBT_UI;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        builder.Services.AddScoped<DialogService>();
        builder.Services.AddScoped<CBT3_App>();
        CBT3_UI.DependencyInjection.Initialize(builder.Services);
        


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
