using Api.Domain.Entities;
using Api.Domain.Interfaces;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IUserRepository : IRepository<UserEntity>
    {
    }
}
