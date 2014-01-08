using Microsoft.AspNet.Identity;

namespace MyCouch.AspNet.Identity
{
    public class IdentityUser : IUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
    }
}