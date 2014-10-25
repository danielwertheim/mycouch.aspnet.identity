using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using MyCouch;
using MyCouch.AspNet.Identity;
using Samples.Mvc5.Models;

namespace Samples.Mvc5
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new MyCouchUserStore<ApplicationUser>(context.Get<IMyCouchClient>()));

            ConfigureUserValidation(manager);
            ConfigurePasswordValidation(manager);
            ConfigureLockout(manager);
            ConfigureEmailService(manager);
            ConfigureSmsService(manager);
            ConfigureTwoFactorProvider(manager);

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        private static void ConfigureUserValidation(ApplicationUserManager manager)
        {
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
        }

        private static void ConfigurePasswordValidation(ApplicationUserManager manager)
        {
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
        }

        private static void ConfigureLockout(ApplicationUserManager manager)
        {
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;
        }

        private static void ConfigureEmailService(ApplicationUserManager manager)
        {
            manager.EmailService = new EmailService();
        }

        private static void ConfigureSmsService(ApplicationUserManager manager)
        {
            manager.SmsService = new SmsService();
        }

        private static void ConfigureTwoFactorProvider(ApplicationUserManager manager)
        {
            manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is: {0}"
            });

            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is: {0}"
            });
        }
    }

    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your sms service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
