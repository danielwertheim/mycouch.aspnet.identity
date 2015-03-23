using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace MyCouch.AspNet.Identity.Validation
{
    public class IdentityUserValidator<TUser> : UserValidator<TUser> where TUser : IdentityUser
    {
        private readonly UserManager<TUser> _manager;

        public Func<IdentityUserValidatorReasons.Reason, string> ReasonProvider { protected get; set; }

        public IdentityUserValidator(UserManager<TUser> manager)
            : base(manager)
        {
            _manager = manager;
            ReasonProvider = IdentityUserValidatorReasons.GetMessage;
        }

        public async override Task<IdentityResult> ValidateAsync(TUser item)
        {
            var result = await base.ValidateAsync(item);
            if (!result.Succeeded)
                return result;

            var existingUser = await _manager.GetEmailAsync(item.Email);
            if (existingUser != null)
                return new IdentityResult(ReasonProvider(IdentityUserValidatorReasons.Reason.EmailAllreadyInUse));

            return new IdentityResult();
        }
    }
}