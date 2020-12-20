using SharedLibrary;

namespace IdentityServer.Services
{
    public interface IEmail
    {
        GeneralResult SendEmail(string toAddress, string subject, string msgBody);
    }
}