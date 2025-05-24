using System.Security.Cryptography.X509Certificates;

namespace MobileNotify.WebApi.Certificates;

public class LocalFileCertificateProvider : ICertificateProvider
{
    public Task<X509Certificate2> GetX509Certificate2Async(string certName, string certPassword)
    {
        var pathToCert = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Certificaties", certName);
        var x509Certificate = new X509Certificate2(pathToCert, certPassword);

        return Task.FromResult(x509Certificate);
    }
}