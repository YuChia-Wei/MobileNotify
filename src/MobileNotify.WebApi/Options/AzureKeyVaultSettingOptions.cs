namespace MobileNotify.WebApi.Options;

public class AzureKeyVaultSettingOptions
{
    public const string SectionName = "AzureKeyVaultSetting";
    public string Endpoint { get; set; }
    public string KeyVaultAuthMethod { get; set; }
    public ClientSecretSetting? ClientSecret { get; set; }
}