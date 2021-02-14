using Api.Domain.Entities;
using Api.Domain.Interfaces;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface ILoginRepository : IRepository<UserEntity>
    {
        Task<UserEntity> FindByEmail(string email);
    }
}
