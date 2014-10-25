using System;
using System.Threading.Tasks;
using MyCouch.AspNet.Identity.Views;
using MyCouch.Responses;

namespace MyCouch.AspNet.Identity
{
    public static class MyCouchClientExtensions
    {
        private const string UserStoreDesignDocId = "_design/userstore";

        public static async Task EnsureAspNetIdentityDesignDocsExists(this IMyCouchClient client)
        {
            var head = await client.Documents.HeadAsync(UserStoreDesignDocId);
            if (head.IsSuccess)
                return;

            var response = await client.Documents.PutAsync(UserStoreDesignDocId, DesignDocs.UserStore);

            EnsureOkResponse(response);
        }

        public static async Task EnsureCleanAspNetIdentityDesignDocsExists(this IMyCouchClient client)
        {
            var head = await client.Documents.HeadAsync(UserStoreDesignDocId);

            var response = head.IsSuccess
                ? await client.Documents.PutAsync(head.Id, head.Rev, DesignDocs.UserStore)
                : await client.Documents.PutAsync(UserStoreDesignDocId, DesignDocs.UserStore);

            EnsureOkResponse(response);
        }

        private static void EnsureOkResponse(Response response)
        {
            if (!response.IsSuccess)
            {
                var msg = string.Format("{0}[Error]: {1}.{0}[Reason]: {2}.",
                    Environment.NewLine,
                    response.Error ?? string.Empty,
                    response.Reason ?? string.Empty);

                throw new Exception("Could not create design document 'userstore' with required views." + msg);
            }
        }
    }
}