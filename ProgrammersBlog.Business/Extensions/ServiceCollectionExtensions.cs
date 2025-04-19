﻿using Microsoft.Extensions.DependencyInjection;
using ProgrammersBlog.Business.Abstract;
using ProgrammersBlog.Business.AutoMapper.Profiles;
using ProgrammersBlog.Business.Concrete;
using ProgrammersBlog.Core.DataAccess.Concrete;
using ProgrammersBlog.DataAccess.Abstract;
using ProgrammersBlog.DataAccess.Concrete.EntityFramework.Contexts;
using ProgrammersBlog.Entities.Concrete;

namespace ProgrammersBlog.Business.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection LoadMyServices(this IServiceCollection serviceCollection)
    {
        //serviceCollection.AddDbContext<ProgrammersBlogContext>();

        serviceCollection.AddIdentity<User, Role>(options =>
        {
            //Test User Password Configuration
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 5;
            options.Password.RequiredUniqueChars = 0;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireLowercase = false;

            //Test User Name and Email Configuration
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;

        }).AddEntityFrameworkStores<ProgrammersBlogContext>();

        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        serviceCollection.AddScoped<ICategoryService, CategoryManager>();
        serviceCollection.AddScoped<IArticleService, ArticleManager>();
        serviceCollection.AddAutoMapper(typeof(CategoryProfile), typeof(ArticleProfile));
        return serviceCollection;
    }
}
