
using Api.Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services.User
{
    public interface ILoginService
    {
        Task<UserEntity> FindByEmail(string email);
    }
}
