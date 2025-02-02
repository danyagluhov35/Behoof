using Behoof.Application.DTO;
using Behoof.Application.IService;
using Behoof.Infrastructure.Data;
using Behoof.Infrastructure.IService;
using Microsoft.AspNetCore.Mvc;

namespace Behoof.Application.Service
{
    public class AccountAppService : IAccountAppService
    {
        private readonly IAccountRepository AccountRepository;
        public AccountAppService(IAccountRepository accountRepository) => AccountRepository = accountRepository;

        public async Task Delete(string id)
        {
            await AccountRepository.Delete(id);
        }

        public async Task<User> Login(UserLoginDto userLoginDto)
        {
            User user = new User()
            {
                Email = userLoginDto.Email,
                Passoword = userLoginDto.Password
            };
            var result = await AccountRepository.Validation(user);
            if (result != null)
                await AccountRepository.Login(result);
            return result!;
        }

        public async Task Register(UserRegisterDto userRegisterDto)
        {
            User user = new User()
            {
                Email = userRegisterDto.Email,
                Login = userRegisterDto.Login,
                Passoword = userRegisterDto.Password
            };
            await AccountRepository.Register(user);
        }

        public async Task<User> Update(UserUpdateDto userUpdateDto)
        {
            User user = new User()
            {
                Email = userUpdateDto.Email,
                Login = userUpdateDto.Login,
                Passoword = userUpdateDto.Passoword,
                Country = userUpdateDto.Country,
                City = userUpdateDto.City,
            };
            return await AccountRepository.Update(user);
        }
        public async Task<string> IsRegistered()
        {
            return await AccountRepository.IsRegistered();
        }
    }
}
