using dotAPNS;
using dotAPNS.AspNetCore;
using Microsoft.Extensions.Options;
using MobileNotify.WebApi.Certificates;
using MobileNotify.WebApi.Options;

namespace MobileNotify.WebApi.Services;

public class ApplePushNotificationService
{
    private readonly IApnsClientFactory _apnsClientFactory;
    private readonly ApnsSettingOptions _apnsSettingOptions;
    private readonly ICertificateProvider _certificateProvider;

    /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
    public ApplePushNotificationService(
        IOptions<ApnsSettingOptions> apnsSettingOptions,
        IApnsClientFactory apnsClientFactory,
        ICertificateProvider certificateProvider)
    {
        this._apnsSettingOptions = apnsSettingOptions.Value;
        this._apnsClientFactory = apnsClientFactory;
        this._certificateProvider = certificateProvider;
    }

    public async Task<ApnsResponse> SendVoipAsync(string voipToken)
    {
        var x509Certificate =
            await this._certificateProvider.GetX509Certificate2Async(this._apnsSettingOptions.VoipCertificateName,
                                                                     this._apnsSettingOptions.VoipCertificatePassword);

        var apnsClient = this._apnsClientFactory.CreateUsingCert(x509Certificate);

        var payload = new ApplePush(ApplePushType.Voip)
                      .AddAlert("title", "message title")
                      // to flutter app
                      .AddCustomProperty("click_action", "FLUTTER_NOTIFICATION_CLICK")
                      .AddSound("default")
                      .AddContentAvailable()
                      .AddImmediateExpiration()
                      .AddMutableContent()
                      .AddVoipToken(voipToken);

        payload.AddCustomProperty("data", new Dictionary<string, object>
        {
            {
                "traceId", Guid.NewGuid()
            }
        });

        if (this._apnsSettingOptions.SendToDevelopServer)
        {
            return await apnsClient.SendAsync(payload.SendToDevelopmentServer());
        }

        return await apnsClient.SendAsync(payload);
    }
}