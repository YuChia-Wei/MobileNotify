using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Microsoft.Extensions.Options;
using MobileNotify.WebApi.Options;

namespace MobileNotify.WebApi.Certificates;

public class AzureKeyVaultClientFactory : IAzureKeyVaultClientFactory
{
    private readonly AzureKeyVaultSettingOptions _azureKeyVaultSettingOptions;

    public AzureKeyVaultClientFactory(IOptions<AzureKeyVaultSettingOptions> options)
    {
        this._azureKeyVaultSettingOptions = options.Value;
    }

    public CertificateClient CreateCertificateClient()
    {
        var uri = new Uri(this._azureKeyVaultSettingOptions.Endpoint);

        switch (this._azureKeyVaultSettingOptions.KeyVaultAuthMethod)
        {
            case "ClientSecret":
                return new CertificateClient(uri,
                                             new ClientSecretCredential(this._azureKeyVaultSettingOptions.ClientSecret?.TenantId,
                                                                        this._azureKeyVaultSettingOptions.ClientSecret?.ClientId,
                                                                        this._azureKeyVaultSettingOptions.ClientSecret?.ClientSecret));
            case "ManagedIdentity":
                return new CertificateClient(uri, new ManagedIdentityCredential());
            case "Certificate":
                // not tested yet, so maybe not work
                var credential = new DefaultAzureCredential();
                return new CertificateClient(uri, credential);
            default:
                throw new Exception("AzureKeyVaultSettingOptions.KeyVaultAuthMethod is not valid");
        }
    }
}