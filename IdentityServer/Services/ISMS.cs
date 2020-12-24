using SharedLibrary;

namespace IdentityServer.Services
{
    public interface ISMS
    {
        GeneralResult<dynamic> SendMessage(string[] mobiles, string text);
    }
}