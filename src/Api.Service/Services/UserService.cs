using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Interfaces;
using Api.Domain.Interfaces.Services.User;

namespace Api.Service.Services
{
    public class UserService : IUserService
    {
        private IRepository<UserEntity> _repository;

        public UserService(IRepository<UserEntity> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Delete(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<UserEntity> Get(Guid id)
        {
            return await _repository.SelectByIdAsync(id);
        }

        public async Task<KeyValuePair<int, IEnumerable<UserEntity>>> GetAll(int currentPage = 1, int perPage = 25, int ordernation = 1, Expression<Func<UserEntity, bool>> expression = null)
        {
            return await _repository.SelectAsync(currentPage, perPage, ordernation, expression);
        }

        public async Task<UserEntity> Post(UserEntity user)
        {
            return await _repository.InsertAsync(user);
        }

        public async Task<UserEntity> Put(UserEntity user)
        {
            return await _repository.UpdateAsync(user);
        }

        public async Task<UserEntity> Register(string name, string email)
        {
            UserEntity userEntity = new UserEntity
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Name = name,
                Email = email,
                Id = Guid.NewGuid()
            };

            return await Post(userEntity);
        }

        public async Task<UserEntity> Update(Guid id, string name, string email)
        {
            UserEntity userEntity = await this.Get(id);

            if (userEntity == null)
            {
                throw new Exception("Usuário não foi localizado");
            }

            userEntity.Name = name;
            userEntity.Email = email;

            return await Put(userEntity);
        }

        public async Task<bool> Remove(Guid id)
        {
            UserEntity user = await Get(id);
            if (user == null)
            {
                throw new Exception("Usuário não existe");
            }
            return await this.Delete(id);
        }
    }
}
