using System;
using System.Text.Json.Serialization;
using AutoMapper;
using Behoof.Domain.AutoMapper;
using Behoof.Domain.Entity.Context;
using Behoof.Domain.JwtTokenSetting;
using Behoof.Domain.Middleware;
using Behoof.Domain.Middleware.Jwt;
using Behoof.Domain.Parsing;
using Behoof.Domain.Parsing2;
using Behoof.IService;
using Behoof.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer("Server=DESKTOP-82QJ6HP;Database=MyDataBase;Trusted_Connection=True;Encrypt=False;"));
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddControllersWithViews().AddJsonOptions(op =>
{
    op.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});
builder.Services.AddAuthorization();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IProductSorterService, ProductSorterService>();
builder.Services.AddTransient<IFavoriteService, FavoriteService>();
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

builder.Services.AddAutoMapper(typeof(UserMapper));



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
