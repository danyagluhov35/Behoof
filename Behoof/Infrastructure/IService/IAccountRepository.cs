using Behoof.Infrastructure.Data;

namespace Behoof.Infrastructure.IService
{
    public interface IAccountRepository
    {
        Task<User> Validation(User user);

        Task Login(User user);

        Task Register(User user);

        Task<User> Update(User user);

        Task Delete(string id);

        Task<string> IsRegistered();
    }
}
