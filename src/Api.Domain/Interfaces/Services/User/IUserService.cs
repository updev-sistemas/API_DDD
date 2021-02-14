using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Api.Domain.Entities;

namespace Api.Domain.Interfaces.Services.User
{
    public interface IUserService
    {
        Task<UserEntity> Register(string name, string email);
        Task<bool> Remove(Guid id);
        Task<UserEntity> Update(Guid id, string name, string email);
        Task<UserEntity> Get(Guid id);
        Task<KeyValuePair<int, IEnumerable<UserEntity>>> GetAll(int currentPage = 1, int perPage = 25, int ordernation = 1, Expression<Func<UserEntity, bool>> expression = null);
        Task<UserEntity> Post(UserEntity user);
        Task<UserEntity> Put(UserEntity user);
        Task<bool> Delete(Guid id);
    }
}
