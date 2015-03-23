using System.Collections.Generic;

namespace MyCouch.AspNet.Identity.Validation
{
    public static class IdentityUserValidatorReasons
    {
        private static readonly Dictionary<Reason, string> Reasons;

        public enum Reason
        {
            EmailAllreadyInUse
        }

        static IdentityUserValidatorReasons()
        {
            Reasons = new Dictionary<Reason, string>
            {
                {Reason.EmailAllreadyInUse, "The provided email address is already in use."}
            };
        }

        public static string GetMessage(Reason reason)
        {
            return Reasons[reason];
        }
    }
}