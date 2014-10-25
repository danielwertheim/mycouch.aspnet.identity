using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;

namespace MyCouch.AspNet.Identity
{
    [Serializable]
    [Document(DocType = "IdentityUser", DocVersion = "2")]
    public class IdentityUser : IUser
    {
        public string Id { get { return UserName; } set { UserName = value; } }
        public string Rev { get; set; }

        public bool IsApproved { get; set; }
        public string UserName { get { return Email; } set { Email = value; } }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }

        public int AccessFailedCount { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }

        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }

        public List<string> Roles { get; set; }
        public List<IdentityUserLogin> Logins { get; set; }
        public List<IdentityUserClaim> Claims { get; set; }

        public IdentityUser()
        {
            IsApproved = false;
            Roles = new List<string>();
            Logins = new List<IdentityUserLogin>();
            Claims = new List<IdentityUserClaim>();
        }

        public virtual void AssignRole(string role)
        {
            if (!HasRole(role))
                Roles.Add(role);
        }

        public virtual void RemoveRole(string role)
        {
            if (HasRoles())
                Roles.RemoveAll(i =>
                    i.Equals(role, StringComparison.OrdinalIgnoreCase));
        }

        public virtual bool HasRole(string role)
        {
            return HasRoles() && Roles.Any(i =>
                i.Equals(role, StringComparison.OrdinalIgnoreCase));
        }

        public virtual bool HasRoles()
        {
            return Roles != null && Roles.Any();
        }

        public virtual void AssignLogin(string loginProvider, string providerKey)
        {
            if (!HasLogin(loginProvider, providerKey))
                Logins.Add(new IdentityUserLogin
                {
                    LoginProvider = loginProvider,
                    ProviderKey = providerKey
                });
        }

        public virtual void RemoveLogin(string loginProvider, string providerKey)
        {
            if (HasLogins())
                Logins.RemoveAll(x =>
                    x.LoginProvider.Equals(loginProvider, StringComparison.OrdinalIgnoreCase) &&
                    x.ProviderKey.Equals(providerKey, StringComparison.OrdinalIgnoreCase));
        }

        public virtual bool HasLogin(string loginProvider, string providerKey)
        {
            return HasLogins() && Logins.Any(i =>
                i.LoginProvider.Equals(loginProvider, StringComparison.OrdinalIgnoreCase) &&
                i.ProviderKey.Equals(providerKey, StringComparison.OrdinalIgnoreCase));
        }

        public virtual bool HasLogins()
        {
            return Logins != null && Logins.Any();
        }

        public virtual void AssignClaim(string claimType, string claimValue)
        {
            if (!HasClaim(claimType, claimValue))
                Claims.Add(new IdentityUserClaim
                {
                    ClaimType = claimType,
                    ClaimValue = claimValue
                });
        }

        public virtual void RemoveClaim(string claimType, string claimValue)
        {
            if (HasClaims())
                Claims.RemoveAll(x =>
                    x.ClaimType.Equals(claimType, StringComparison.OrdinalIgnoreCase) &&
                    x.ClaimValue.Equals(claimValue, StringComparison.OrdinalIgnoreCase));
        }

        public virtual bool HasClaim(string claimType, string claimValue)
        {
            return HasClaims() && Claims.Any(i =>
                i.ClaimType.Equals(claimType, StringComparison.OrdinalIgnoreCase) &&
                i.ClaimValue.Equals(claimValue, StringComparison.OrdinalIgnoreCase));
        }

        public virtual bool HasClaims()
        {
            return Claims != null && Claims.Any();
        }
    }
}