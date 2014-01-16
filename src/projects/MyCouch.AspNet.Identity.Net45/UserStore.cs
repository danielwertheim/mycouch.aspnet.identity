using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnsureThat;
using Microsoft.AspNet.Identity;

namespace MyCouch.AspNet.Identity
{
    //http://msdn.microsoft.com/en-us/library/hh524395.aspx#BKMK_TaskReturnType

    //TODO: Perhaps add a ThrowIfNotSuccessful to each call and check the response.IsSuccess
    public class UserStore<TUser> :
        IUserPasswordStore<TUser>,
        IUserLoginStore<TUser> where TUser : IdentityUser, IUser
    {
        protected bool Disposed { get; private set; }
        protected IClient Client { get; private set; }

        public bool DisposeClient { get; set; }

        public UserStore(IClient client)
        {
            Ensure.That(client, "client").IsNotNull();

            DisposeClient = true;
            Client = client;
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            ThrowIfDisposed();

            Disposed = true;

            if (disposing && DisposeClient && Client != null)
            {
                Client.Dispose();
                Client = null;
            }
        }

        protected virtual void ThrowIfDisposed()
        {
            if (Disposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        public async virtual Task CreateAsync(TUser user)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            if (string.IsNullOrEmpty(user.Id))
                await Client.Entities.PostAsync(user);
            else
                await Client.Entities.PutAsync(user);
        }

        public async virtual Task UpdateAsync(TUser user)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            await Client.Entities.PutAsync(user);
        }

        public async virtual Task DeleteAsync(TUser user)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            await Client.Entities.DeleteAsync(user);
        }

        public virtual Task<TUser> FindByIdAsync(string userId)
        {
            ThrowIfDisposed();

            Ensure.That(userId, "userId").IsNotNullOrWhiteSpace();

            return Client.Entities.GetAsync<TUser>(userId).ContinueWith(r => r.Result.Entity);
        }

        public virtual Task<TUser> FindByNameAsync(string userName)
        {
            ThrowIfDisposed();

            Ensure.That(userName, "userName").IsNotNullOrWhiteSpace();

            return Client.Entities.GetAsync<TUser>(userName).ContinueWith(r => r.Result.Entity);
        }

        public virtual Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            user.PasswordHash = passwordHash;

            return Task.FromResult(0);
        }

        public virtual Task<string> GetPasswordHashAsync(TUser user)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            return Task.FromResult(user.PasswordHash);
        }

        public virtual Task<bool> HasPasswordAsync(TUser user)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            return Task.FromResult(user.PasswordHash != null);
        }

        public virtual Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();
            Ensure.That(login, "login").IsNotNull();

            if (!user.HasLogin(login.LoginProvider, login.ProviderKey))
                user.Logins.Add(new IdentityUserLogin
                {
                    LoginProvider = login.LoginProvider,
                    ProviderKey = login.ProviderKey
                });

            return Task.FromResult(0);
        }

        public virtual Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();
            Ensure.That(login, "login").IsNotNull();

            user.Logins.RemoveAll(x => x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey);

            return Task.FromResult(0);
        }

        public virtual Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            IList<UserLoginInfo> logins = user.Logins.Select(i => new UserLoginInfo(i.LoginProvider, i.ProviderKey)).ToList();

            return Task.FromResult(logins);
        }

        public virtual Task<TUser> FindAsync(UserLoginInfo login)
        {
            Ensure.That(login, "login").IsNotNull();

            throw new System.NotImplementedException();
        }
    }
}