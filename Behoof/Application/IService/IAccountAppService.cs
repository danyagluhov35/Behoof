using Behoof.Application.DTO;
using Behoof.Infrastructure.Data;

namespace Behoof.Application.IService
{
    public interface IAccountAppService
    {

        Task<User> Login(UserLoginDto userLoginDto);

        Task Register(UserRegisterDto userRegisterDto);

        Task<User> Update(UserUpdateDto userUpdateDto);

        Task Delete(string id);

        Task<string> IsRegistered();
    }
}
