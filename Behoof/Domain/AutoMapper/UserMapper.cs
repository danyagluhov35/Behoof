using AutoMapper;
using Behoof.Domain.Entity;

namespace Behoof.Domain.AutoMapper
{
    public class UserMapper : Profile
    {
        public UserMapper() 
        {
            CreateMap<User, User>().ForAllMembers(opt => opt.Condition((src, dest,srcMember) => srcMember != null));
        }
    }
}
