using Api.Data.Context;
using Api.Data.Repository;
using Api.Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class LoginRepository : BaseRepository<UserEntity>, ILoginRepository
    {
        private DbSet<UserEntity> _dataset;

        public LoginRepository(MyContext context): base(context)
        {
            this._dataset = context.Set<UserEntity>();
        }

        public async Task<UserEntity> FindByEmail(string email)
        {
            return await this._dataset.FirstOrDefaultAsync<UserEntity>(x => x.Email == email);
        }
    }
}
