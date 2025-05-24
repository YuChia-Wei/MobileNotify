using System.Security.Cryptography.X509Certificates;

namespace MobileNotify.WebApi.Certificates;

public interface ICertificateProvider
{
    Task<X509Certificate2> GetX509Certificate2Async(string certName, string certPassword);
}