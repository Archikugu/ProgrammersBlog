﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProgrammersBlog.Core.Utilities.Helpers.Abstract;
using ProgrammersBlog.Core.Utilities.Helpers.Concrete;

namespace ProgrammersBlog.Core.Utilities.Extensions;
public static class ServiceCollectionExtensions
{
    public static void ConfigureWritable<T>(
        this IServiceCollection services,
        IConfigurationSection section,
        string file = "appsettings.json") where T : class, new()
    {
        services.Configure<T>(section);
        services.AddTransient<IWritableOptions<T>>(provider =>
        {
            var environment = provider.GetService<IHostingEnvironment>();
            var options = provider.GetService<IOptionsMonitor<T>>();
            return new WritableOptions<T>(environment, options, section.Key, file);
        });
    }
}
