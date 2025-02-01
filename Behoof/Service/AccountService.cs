﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Behoof.Domain.Entity;
using Behoof.Domain.Entity.Context;
using Behoof.Domain.JwtTokenSetting;
using Behoof.IService;
using Behoof.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Behoof.Service
{
    public class AccountService : IAccountService
    {
        private ApplicationContext db;
        private HttpContext context;
        private IHttpContextAccessor httpContextAccessor;
        private IMapper Mapper;
        public AccountService(IHttpContextAccessor httpContextAccessor, ApplicationContext _db, IMapper mapper)
        {
            db = _db;
            this.httpContextAccessor = httpContextAccessor;
            context = httpContextAccessor.HttpContext!;
            Mapper = mapper;
        }

        public async Task Delete(User user)
        {
            var data = db.Users.FirstOrDefault(u => u.Id == user.Id);
            if(data != null)
                db.Users.Remove(data);
            await db.SaveChangesAsync();
        }

        public async Task Login(User user)
        {
            try
            {
                var claim = new List<Claim>() 
                {
                    new Claim("Id", user.Id!),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(ClaimTypes.Name, user.Login!),
                    new Claim("Passowrd", user.Passoword!),
                    new Claim("CountryId", user?.CountryId ?? "Не быбрана"),
                    new Claim("CityId", user?.CityId ?? "Не быбрана"),
                };
                var jwt = new JwtSecurityToken
                (
                    issuer : AuthOption.Issuer,
                    audience : AuthOption.Audience,
                    claims : claim,
                    expires : DateTime.UtcNow.AddDays(1),
                    signingCredentials : new SigningCredentials(AuthOption.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                );
                context.Response.Cookies.Append("JwtToken", new JwtSecurityTokenHandler().WriteToken(jwt));
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task Register(UserRegister user)
        {
            try
            {
                Favorite favorite = new Favorite();
                db.Favorite.Add(favorite);
                db.SaveChanges();

                User newUser = new User() 
                {
                    Email = user.Email,
                    Login = user.Login,
                    Passoword = user.Password,
                    FavoriteId = favorite.Id
                };
                db.Users.Add(newUser);
                await db.SaveChangesAsync();

                
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task ResetPassword(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Update(User user)
        {
            try
            {
                var data = db.Users.FirstOrDefault(u => u.Id == user.Id);

                Mapper.Map(user,data);

                db.Users.Update(data);
                await db.SaveChangesAsync();

                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<User> Validation(UserLogin user)
        {
            User? data = await db?.Users?
                .Include(u => u.Country)
                .ThenInclude(u => u.City)
                .FirstOrDefaultAsync(u => u.Email == user.Email && u.Passoword == user.Password)!;
            if (data == null)
                return null!;
            return data;
        }

        public async Task Delete(string id)
        {
            try
            {
                var user = db.Users.FirstOrDefault(u => u.Id == id);
                if(user != null)
                    db.Users.Remove(user);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> IsRegistered()
        {
            var res = context.User.Identity.IsAuthenticated;

            if (res)
                return context.User.Claims.FirstOrDefault(u => u.Type == "Id")?.Value!;
            return null!;
        }
    }
}
