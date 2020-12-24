using SharedLibrary;

namespace IdentityServer.Services
{
    public interface IEmail
    {
        GeneralResult<dynamic> SendEmail(string toAddress, string subject, string msgBody);
    }
}