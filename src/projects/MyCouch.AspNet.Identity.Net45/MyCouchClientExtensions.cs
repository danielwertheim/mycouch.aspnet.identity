using System;
using System.Threading.Tasks;
using MyCouch.AspNet.Identity.Views;

namespace MyCouch.AspNet.Identity
{
    public static class MyCouchClientExtensions
    {
        public static async Task EnsureDesignDocsExists(this IClient client)
        {
            var getResponse = await client.Documents.GetAsync("_design/userstore");
            if (!getResponse.IsEmpty)
                return;

            var putResponse = await client.Documents.PutAsync(Uri.EscapeDataString("_design/userstore"), DesignDocs.UserStore);
            if (!putResponse.IsSuccess)
            {
                var msg = string.Format("{0}[Error]: {1}.{0}[Reason]: {2}.",
                    Environment.NewLine,
                    putResponse.Error ?? string.Empty,
                    putResponse.Reason ?? string.Empty);

                throw new MyCouchException("Could not create design document 'userstore' with required views." + msg);
            }
        }
    }
}