using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace MyCouch.AspNet.Identity
{
    public class UserStore<TUser> : IUserStore<TUser> where TUser : IUser
    {
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public Task CreateAsync(TUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(TUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(TUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task<TUser> FindByIdAsync(string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<TUser> FindByNameAsync(string userName)
        {
            throw new System.NotImplementedException();
        }
    }
}