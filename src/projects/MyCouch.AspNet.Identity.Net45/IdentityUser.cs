using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;

namespace MyCouch.AspNet.Identity
{
    [Serializable]
    [Document(DocType = "IdentityUser")]
    public class IdentityUser : IUser
    {
        public string Id { get; set; }
        public string Rev { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }

        public List<IdentityUserLogin> Logins { get; set; }

        public virtual bool HasLogin(string loginProvider, string providerKey)
        {
            return Logins.Any(i =>
                i.LoginProvider.Equals(loginProvider, StringComparison.OrdinalIgnoreCase) &&
                i.ProviderKey.Equals(providerKey, StringComparison.OrdinalIgnoreCase));
        }
    }
}