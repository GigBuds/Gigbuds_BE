using FirebaseAdmin;
using Gigbuds_BE.Application.Configurations;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;

namespace Gigbuds_BE.Infrastructure.Services.Firebase
{
    internal class FirebaseService : IFirebaseService
    {
        private readonly FirebaseApp _app;

        public FirebaseService(IOptions<FirebaseSettings> settings)
        {
            _app = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(settings.Value.NotificationServiceAccountKeyPath)
            });
        }

        public FirebaseApp GetApp()
        {
            return _app;
        }
    }

    public interface IFirebaseService
    {
        FirebaseApp GetApp();
    }
}
