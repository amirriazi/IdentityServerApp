using SharedLibrary;

namespace IdentityServer.Services
{
    public interface ISMS
    {
        GeneralResult SendMessage(string[] mobiles, string text);
    }
}