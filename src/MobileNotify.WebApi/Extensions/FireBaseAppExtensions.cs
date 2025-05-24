using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using MobileNotify.WebApi.Services;

namespace MobileNotify.WebApi.Extensions;

public static class FireBaseAppExtensions
{
    public static void AddFirebaseServices(this WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.AddFireBaseApp("firebaseKey.json");
        webApplicationBuilder.Services.AddScoped<FirebaseCloudMessagingService>();
    }

    private static WebApplicationBuilder AddFireBaseApp(this WebApplicationBuilder builder, string configFileName)
    {
        FirebaseApp.Create(new AppOptions
        {
            // Credential = GoogleCredential.FromFile(Path.Combine(builder.Environment.ContentRootPath, configFileName))
            Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFileName))
        });

        return builder;
    }
}