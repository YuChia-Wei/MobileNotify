using System.Text.Json;
using System.Text.Json.Serialization;
using FirebaseAdmin.Messaging;

namespace MobileNotify.WebApi.Services;

/// <summary>
/// FCM service
/// </summary>
public class FirebaseCloudMessagingService
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly ILogger<FirebaseCloudMessagingService> _logger;

    public FirebaseCloudMessagingService(
        ILogger<FirebaseCloudMessagingService> logger,
        JsonSerializerOptions jsonSerializerOptions)
    {
        this._logger = logger;
        this._jsonSerializerOptions = jsonSerializerOptions;
    }

    public async Task<string> PushAsync(string deviceToken)
    {
        var customMessageData = new CustomMessageData
        {
            TraceId = Guid.NewGuid()
        };

        var messageData = JsonSerializer.Serialize(customMessageData, this._jsonSerializerOptions);
        var buildMessage = this.BuildMessage(messageData, deviceToken, "I'm test", "test body");
        return await this.SendFcmMessage(buildMessage, CancellationToken.None);
    }

    private Message BuildMessage(string messageData, string messageDeviceToken, string messageNotificationTitle, string messageNotificationBody)
    {
        var dataDic = new Dictionary<string, string>
        {
            // to flutter app
            {
                "click_action", "FLUTTER_NOTIFICATION_CLICK"
            },
            {
                "additionalData", messageData
            },
            {
                "content_available", "true"
            }
        };

        this._logger.LogDebug("Push FCM Notification, Message Data JSON: {MessageData}", messageData);

        return new Message
        {
            Token = messageDeviceToken,
            Topic = null,
            Notification = new Notification
            {
                Title = messageNotificationTitle,
                Body = messageNotificationBody,
                ImageUrl = null
            },
            Data = dataDic,
            Android = new AndroidConfig
            {
                Priority = Priority.High,
                TimeToLive = TimeSpan.FromSeconds(0)
            }
        };
    }

    private async Task<string> SendFcmMessage(Message tokenMessage, CancellationToken cancellationToken)
    {
        var target = tokenMessage.Token ?? tokenMessage.Topic;

        try
        {
            var response = await FirebaseMessaging.DefaultInstance.SendAsync(tokenMessage, cancellationToken);

            this._logger.LogInformation("Firebase Messaging Send Response {Response}", response);
            return response;
        }
        catch (FirebaseMessagingException e)
        {
            var response = await e.HttpResponse.Content.ReadAsStringAsync(cancellationToken);

            this._logger.LogError("Firebase Messaging Send Response {Response} With Error Code {MessagingErrorCode}, Exception Info : {Exception}",
                                  response,
                                  e.MessagingErrorCode,
                                  e);
            return response;
        }
    }
}

public class CustomMessageData
{
    [JsonPropertyName("traceId")]
    public Guid TraceId { get; set; }
}