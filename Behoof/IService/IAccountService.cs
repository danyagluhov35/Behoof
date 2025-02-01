using Behoof.Domain.Entity;
using Behoof.Models.User;

namespace Behoof.IService
{
    public interface IAccountService
    {
        Task<User> Validation(UserLogin user);

        Task Login(User user);

        Task Register(UserRegister user);

        Task Delete(User user);

        Task<User> Update(User user);

        Task ResetPassword(User user);

        Task Delete(string id);

        Task<string> IsRegistered();
    }
}
