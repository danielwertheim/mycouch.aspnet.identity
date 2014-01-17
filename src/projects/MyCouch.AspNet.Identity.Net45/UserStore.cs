using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EnsureThat;
using Microsoft.AspNet.Identity;

namespace MyCouch.AspNet.Identity
{
    //http://msdn.microsoft.com/en-us/library/hh524395.aspx#BKMK_TaskReturnType

    //TODO: Perhaps add a ThrowIfNotSuccessful to each call and check the response.IsSuccess
    public class UserStore<TUser> :
        IUserPasswordStore<TUser>,
        IUserLoginStore<TUser>,
        IUserClaimStore<TUser>,
        IUserRoleStore<TUser> where TUser : IdentityUser, IUser
    {
        protected bool Disposed { get; private set; }
        protected IClient Client { get; private set; }

        public bool DisposeClient { get; set; }

        public UserStore(IClient client)
        {
            Ensure.That(client, "client").IsNotNull();

            DisposeClient = false;
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

        public async virtual Task<TUser> FindByNameAsync(string userName)
        {
            ThrowIfDisposed();

            Ensure.That(userName, "userName").IsNotNullOrWhiteSpace();

            var qr = await Client.Views.QueryAsync<string>("userstore", "usernames", q => q.Key(userName));

            return qr.IsEmpty
                ? await Task.FromResult(null as TUser)
                : await Client.Entities.GetAsync<TUser>(qr.Rows[0].Id).ContinueWith(r => r.Result.Entity);
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

            user.AssignLogin(login.LoginProvider, login.ProviderKey);

            return Task.FromResult(0);
        }

        public virtual Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();
            Ensure.That(login, "login").IsNotNull();

            user.RemoveLogin(login.LoginProvider, login.ProviderKey);

            return Task.FromResult(0);
        }

        public virtual Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            IList<UserLoginInfo> logins = user.HasLogins()
                ? user.Logins.Select(i => new UserLoginInfo(i.LoginProvider, i.ProviderKey)).ToList()
                : new List<UserLoginInfo>();

            return Task.FromResult(logins);
        }

        public async virtual Task<TUser> FindAsync(UserLoginInfo login)
        {
            ThrowIfDisposed();

            Ensure.That(login, "login").IsNotNull();

            var qr = await Client.Views.QueryAsync<string>("userstore", "loginprovider_providerkey", q => q.Key(new[]{login.LoginProvider, login.ProviderKey}));

            return qr.IsEmpty
                ? await Task.FromResult(null as TUser)
                : await Client.Entities.GetAsync<TUser>(qr.Rows[0].Id).ContinueWith(r => r.Result.Entity);
        }

        public virtual Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            IList<Claim> claims = user.HasClaims()
                ? user.Claims.Select(i => new Claim(i.ClaimType, i.ClaimValue)).ToList()
                : new List<Claim>();

            return Task.FromResult(claims);
        }

        public virtual Task AddClaimAsync(TUser user, Claim claim)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();
            Ensure.That(claim, "claim").IsNotNull();

            if(!user.HasClaim(claim.Type, claim.ValueType))
                user.Claims.Add(new IdentityUserClaim
                {
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value
                });

            return Task.FromResult(0);
        }

        public virtual Task RemoveClaimAsync(TUser user, Claim claim)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();
            Ensure.That(claim, "claim").IsNotNull();

            user.Claims.RemoveAll(i =>
                i.ClaimType.Equals(claim.Type, StringComparison.OrdinalIgnoreCase) &&
                i.ClaimValue.Equals(claim.Value, StringComparison.OrdinalIgnoreCase));

            return Task.FromResult(0);
        }

        public virtual Task AddToRoleAsync(TUser user, string role)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();
            Ensure.That(role, "role").IsNotNullOrWhiteSpace();

            user.AssignRole(role);

            return Task.FromResult(0);
        }

        public virtual Task RemoveFromRoleAsync(TUser user, string role)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();
            Ensure.That(role, "role").IsNotNullOrWhiteSpace();

            user.RemoveRole(role);

            return Task.FromResult(0);
        }

        public virtual Task<IList<string>> GetRolesAsync(TUser user)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            IList<string> roles = user.Roles ?? new List<string>();

            return Task.FromResult(roles);
        }

        public virtual Task<bool> IsInRoleAsync(TUser user, string role)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();
            Ensure.That(role, "role").IsNotNullOrWhiteSpace();

            return Task.FromResult(user.HasRole(role));
        }
    }
}