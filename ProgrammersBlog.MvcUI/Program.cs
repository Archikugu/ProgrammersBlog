using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using ProgrammersBlog.Business.Extensions;
using ProgrammersBlog.DataAccess.Concrete.EntityFramework.Contexts;
using ProgrammersBlog.MvcUI.AutoMapper;
using ProgrammersBlog.MvcUI.Helpers.Abstract;
using ProgrammersBlog.MvcUI.Helpers.Concrete;
using ProgrammersBlog.Entities.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(value => "This field must not be left empty!");
}).AddRazorRuntimeCompilation().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
}).AddNToastNotifyToastr();

builder.Services.AddSession();
builder.Services.LoadMyServices();
builder.Services.AddAutoMapper(typeof(ViewModelsProfile));
builder.Services.AddScoped<IImageHelper, ImageHelper>();
builder.Services.Configure<AboutUsPageInfo>(builder.Configuration.GetSection("AboutUsPageInfo"));
builder.Services.Configure<WebsiteInfo>(builder.Configuration.GetSection("WebsiteInfo"));

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
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
