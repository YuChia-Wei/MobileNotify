using Azure.Security.KeyVault.Certificates;

namespace MobileNotify.WebApi.Certificates;

public interface IAzureKeyVaultClientFactory
{
    public CertificateClient CreateCertificateClient();
}