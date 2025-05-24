using System.Security.Cryptography.X509Certificates;
using Azure.Security.KeyVault.Certificates;

namespace MobileNotify.WebApi.Certificates;

/// <summary>
/// 從 azure key vault 取得必要憑證
/// </summary>
public class AzureKeyVaultCertificateProvider : ICertificateProvider
{
    private readonly IAzureKeyVaultClientFactory _azureKeyVaultClientFactory;
    private readonly ILogger<AzureKeyVaultCertificateProvider> _logger;

    public AzureKeyVaultCertificateProvider(ILogger<AzureKeyVaultCertificateProvider> logger, IAzureKeyVaultClientFactory azureKeyVaultClientFactory)
    {
        this._logger = logger;
        this._azureKeyVaultClientFactory = azureKeyVaultClientFactory;
    }

    public async Task<X509Certificate2> GetX509Certificate2Async(string certName, string certPassword)
    {
        var certificateClient = this._azureKeyVaultClientFactory.CreateCertificateClient();

        // 在部署在 windows 的時候有機會發生無法下載的問題，這邊指定下載選項以處理下載問題
        var downloadCertificateOptions = new DownloadCertificateOptions(certName)
        {
            Version = null,
            KeyStorageFlags = X509KeyStorageFlags.MachineKeySet
        };
        var cert = await certificateClient.DownloadCertificateAsync(downloadCertificateOptions);
        // 在 linux / mac os 可以直接這樣下載
        // var cert = await certificateClient.DownloadCertificateAsync(certName);
        return cert.Value;
    }
}