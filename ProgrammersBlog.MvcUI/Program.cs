using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NLog.Web;
using ProgrammersBlog.Business.AutoMapper.Profiles;
using ProgrammersBlog.Business.Extensions;
using ProgrammersBlog.Core.Utilities.Extensions;
using ProgrammersBlog.DataAccess.Concrete.EntityFramework.Contexts;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.MvcUI.AutoMapper;
using ProgrammersBlog.MvcUI.Filters;
using ProgrammersBlog.MvcUI.Helpers.Abstract;
using ProgrammersBlog.MvcUI.Helpers.Concrete;

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Konfigürasyon kaynaklarını temizle ve yeniden ekle
    builder.Configuration.Sources.Clear();

    var env = builder.Environment;

    builder.Configuration
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();

    if (args != null)
    {
        builder.Configuration.AddCommandLine(args);
    }

    // Servisleri ekle
    // builder.Services.AddControllersWithViews(); // örnek

    // NLog: Remove all default logging providers and use NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Add services to the container.
    builder.Services.AddControllersWithViews(options =>
    {
        options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(value => "This field must not be left empty!");
        options.Filters.Add<MvcExceptionFilter>();
    }).AddRazorRuntimeCompilation().AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    }).AddNToastNotifyToastr();

    builder.Services.AddSession();
    builder.Services.LoadMyServices();
    builder.Services.AddAutoMapper(cfg => { cfg.AddProfile<ViewModelsProfile>(); });

    builder.Services.AddScoped<IImageHelper, ImageHelper>();
    builder.Services.Configure<AboutUsPageInfo>(builder.Configuration.GetSection("AboutUsPageInfo"));
    builder.Services.Configure<WebsiteInfo>(builder.Configuration.GetSection("WebsiteInfo"));
    builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
    builder.Services.Configure<ArticleRightSideBarWidgetOptions>(builder.Configuration.GetSection("ArticleRightSideBarWidgetOptions"));
    builder.Services.ConfigureWritable<AboutUsPageInfo>(builder.Configuration.GetSection("AboutUsPageInfo"));
    builder.Services.ConfigureWritable<WebsiteInfo>(builder.Configuration.GetSection("WebsiteInfo"));
    builder.Services.ConfigureWritable<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
    builder.Services.ConfigureWritable<ArticleRightSideBarWidgetOptions>(builder.Configuration.GetSection("ArticleRightSideBarWidgetOptions"));


    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.LoginPath = new PathString("/Admin/Auth/Login");
        options.LogoutPath = new PathString("/Admin/Auth/Logout");
        options.Cookie = new CookieBuilder()
        {
            Name = "ProgrammersBlog",
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            SecurePolicy = CookieSecurePolicy.SameAsRequest // Always
        };
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.AccessDeniedPath = new PathString("/ErrorPage/AccessDenied");
    });


    builder.Services.AddDbContext<ProgrammersBlogContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

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

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseNToastNotify();

    app.UseStatusCodePagesWithReExecute("/ErrorPage/Error404", "?code={0}");

    app.MapStaticAssets();

    app.MapAreaControllerRoute(
        name: "Admin",
        areaName: "Admin",
        pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

    app.MapControllerRoute(
        name: "article",
        pattern: "{title}/{articleId}",
        defaults: new { controller = "Article", action = "Detail" });

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Program başlatılırken hata oluştu");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}
