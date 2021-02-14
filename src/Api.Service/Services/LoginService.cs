using Api.Domain.Entities;
using Api.Domain.Interfaces;
using Domain.Interfaces.Services.User;
using Domain.Repository;
using System.Threading.Tasks;

namespace Service.Services
{
    public class LoginService : ILoginService
    {
        private ILoginRepository _repository;

        public LoginService(ILoginRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserEntity> FindByEmail(string email)
        {
            return await _repository.FindByEmail(email);
        }
    }
}
