//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//     Author: Scott Green
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Net;

using CBT3_Shared.Configuration;
using CBT3_Application.Configuration;
using CBT3_Infrastructure.Configuration;
using Radzen;

namespace CBT3_UI;

public static class DependencyInjection
{
    
public static IServiceCollection Initialize(this IServiceCollection services)
    {
        //ServiceCollection services = new ServiceCollection();
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        
        services.AddSingleton<IConfiguration>(configuration);
        services.AddScoped<CBT3_Shared.UserDetails>();
        services.AddSharedServices(configuration);
        services.AddInfrastructureServices(configuration);
        services.AddApplicationServices();


        //ServiceProvider = services.BuildServiceProvider();
        return services;
    }
    
    //public static IServiceProvider ServiceProvider { get; private set; }


}
