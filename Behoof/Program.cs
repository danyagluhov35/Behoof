using System;
using System.Text.Json.Serialization;
using AutoMapper;
using Behoof.Application.IService;
using Behoof.Application.Service;
using Behoof.Core.JwtTokenSetting;
using Behoof.Core.Services;
using Behoof.Infrastructure.BackgroundServices;
using Behoof.Infrastructure.Data;
using Behoof.Infrastructure.IService;
using Behoof.Infrastructure.Service;
using Behoof.Middleware.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(@"Server=host.docker.internal,1433;Database=Behoof;User Id=DESKTOP-82QJ6HP\deni3;"));
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddControllersWithViews().AddJsonOptions(op =>
{
    op.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});
builder.Services.AddScoped<IAccountAppService, AccountAppService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

builder.Services.AddScoped<ICategoryAppService, CategoryAppService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<IFavoriteAppService, FavoriteAppService>();
builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();

builder.Services.AddScoped<IProductAppService, ProductAppService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<TrimProductName>();


builder.Services.AddTransient<IFoldProductMemoryCacheService, FoldProductMemoryCacheService>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<SupplierFactory>();
builder.Services.AddHostedService<ParsingSitesBackgroundService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(op =>
{
    op.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = AuthOption.Issuer,
        ValidAudience = AuthOption.Audience,
        IssuerSigningKey = AuthOption.GetSymmetricSecurityKey()
    };
});




var app = builder.Build();
app.UseMiddleware<JwtSecurity>();
app.UseSession();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute
    (
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
app.MapControllerRoute
    (
        name : "default",
        pattern : "{controller=Home}/{action=Index}/{id?}"
    );
app.MapControllerRoute
    (
        name: "default",
        pattern: "{controller=Favorite}/{action=Favorite}/{id?}"
    );

app.UseStaticFiles();
app.UseDefaultFiles();



app.Run();
