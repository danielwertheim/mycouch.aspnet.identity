using System;

namespace MyCouch.AspNet.Identity
{
    [Serializable]
    public class IdentityUserLogin
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
    }
}