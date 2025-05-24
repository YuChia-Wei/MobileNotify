namespace MobileNotify.WebApi.Options;

public class ApnsSettingOptions
{
    public const string SectionName = "APNSSetting";

    /// <summary>
    /// 是否由開發版 server 傳送通知
    /// </summary>
    public bool SendToDevelopServer { get; set; }

    /// <summary>
    /// Voip 憑證
    /// </summary>
    public string VoipCertificateName { get; set; }

    /// <summary>
    /// 憑證密碼
    /// </summary>
    public string VoipCertificatePassword { get; set; }

    /// <summary>
    /// 一般推播憑證
    /// </summary>
    public string NotificationCertificateName { get; set; }

    /// <summary>
    /// 憑證密碼
    /// </summary>
    public string NotificationCertificatePassword { get; set; }
}