using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EnsureThat;
using Microsoft.AspNet.Identity;
using MyCouch.Requests;

namespace MyCouch.AspNet.Identity
{
    public class MyCouchUserStore<TUser> :
        IUserStore<TUser>,
        IUserLockoutStore<TUser, string>,
        IUserTwoFactorStore<TUser, string>,
        IUserPasswordStore<TUser>,
        IUserLoginStore<TUser>,
        IUserClaimStore<TUser>,
        IUserRoleStore<TUser>,
        IUserEmailStore<TUser>,
        IUserPhoneNumberStore<TUser>,
        IUserSecurityStampStore<TUser> where TUser : IdentityUser, IUser
    {
        private readonly ViewIdentity _loginProviderProviderKeyView;

        protected bool IsDisposed { get; private set; }
        protected IMyCouchClient Client { get; private set; }

        public bool DisposeClient { get; set; }

        public MyCouchUserStore(IMyCouchClient client)
        {
            Ensure.That(client, "client").IsNotNull();

            _loginProviderProviderKeyView = new ViewIdentity("userstore", "loginprovider_providerkey");

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

            IsDisposed = true;

            if (disposing && DisposeClient && Client != null)
            {
                Client.Dispose();
                Client = null;
            }
        }

        protected virtual void ThrowIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        public async virtual Task CreateAsync(TUser user)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();
            Ensure.That(user.Id, "user.Id").IsNotNullOrWhiteSpace();

            await Client.Entities.PutAsync(user);
        }

        public async virtual Task UpdateAsync(TUser user)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();
            Ensure.That(user.Id, "user.Id").IsNotNullOrWhiteSpace();
            Ensure.That(user.Rev, "user.Rev").IsNotNullOrWhiteSpace();

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

            return Client.Entities.GetAsync<TUser>(userId).ContinueWith(r => r.Result.Content);
        }

        public async virtual Task<TUser> FindByNameAsync(string userName)
        {
            ThrowIfDisposed();

            Ensure.That(userName, "userName").IsNotNullOrWhiteSpace();

            var u = await Client.Entities.GetAsync<TUser>(userName);

            return await Task.FromResult(u.Content);
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

            var request = new QueryViewRequest(_loginProviderProviderKeyView.DesignDocument, _loginProviderProviderKeyView.Name)
                .Configure(q => q.Key(new[] { login.LoginProvider, login.ProviderKey }));

            var qr = await Client.Views.QueryAsync<string>(request);

            return qr.IsEmpty
                ? await Task.FromResult(null as TUser)
                : await Client.Entities.GetAsync<TUser>(qr.Rows[0].Id).ContinueWith(r => r.Result.Content);
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

            user.AssignClaim(claim.Type, claim.Value);

            return Task.FromResult(0);
        }

        public virtual Task RemoveClaimAsync(TUser user, Claim claim)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();
            Ensure.That(claim, "claim").IsNotNull();

            user.RemoveClaim(claim.Type, claim.Value);

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

        public virtual Task SetSecurityStampAsync(TUser user, string stamp)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            user.SecurityStamp = stamp;

            return Task.FromResult(0);
        }

        public virtual Task<string> GetSecurityStampAsync(TUser user)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            return Task.FromResult(user.SecurityStamp);
        }

        public virtual Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            return Task.FromResult(user.LockoutEndDateUtc ?? new DateTimeOffset());
        }

        public virtual Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            user.LockoutEndDateUtc = lockoutEnd.UtcDateTime;

            return Task.FromResult(0);
        }

        public virtual Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            user.AccessFailedCount += 1;

            return Task.FromResult(user.AccessFailedCount);
        }

        public virtual Task ResetAccessFailedCountAsync(TUser user)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            user.AccessFailedCount = 0;

            return Task.FromResult(0);
        }

        public virtual Task<int> GetAccessFailedCountAsync(TUser user)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            return Task.FromResult(user.AccessFailedCount);
        }

        public virtual Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            return Task.FromResult(user.LockoutEnabled);
        }

        public virtual Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            user.LockoutEnabled = enabled;

            return Task.FromResult(0);
        }

        public virtual Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            user.TwoFactorEnabled = enabled;

            return Task.FromResult(0);
        }

        public virtual Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            return Task.FromResult(user.TwoFactorEnabled);
        }

        public virtual Task SetEmailAsync(TUser user, string email)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            user.Email = email;

            return Task.FromResult(0);
        }

        public virtual Task<string> GetEmailAsync(TUser user)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            return Task.FromResult(user.Email);
        }

        public virtual Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            return Task.FromResult(user.EmailConfirmed);
        }

        public virtual Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            user.EmailConfirmed = confirmed;

            return Task.FromResult(0);
        }

        public virtual async Task<TUser> FindByEmailAsync(string email)
        {
            ThrowIfDisposed();

            Ensure.That(email, "email").IsNotNullOrWhiteSpace();

            var u = await Client.Entities.GetAsync<TUser>(email);

            return await Task.FromResult(u.Content);
        }

        public virtual Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            user.PhoneNumber = phoneNumber;

            return Task.FromResult(0);
        }

        public virtual Task<string> GetPhoneNumberAsync(TUser user)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            return Task.FromResult(user.PhoneNumber);
        }

        public virtual Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public virtual Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {
            ThrowIfDisposed();

            Ensure.That(user, "user").IsNotNull();

            user.PhoneNumberConfirmed = confirmed;

            return Task.FromResult(0);
        }
    }
}