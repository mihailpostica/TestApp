using Google.Apis.Auth;
using System.Threading.Tasks;
using Test.DTO;

namespace Test
{
    public interface IGoogleAuthService
    {
        Task<SocialUserDetails> getGoogleUserInfo(string token);
        Task<GoogleJsonWebSignature.Payload> validateToken(string token);
    }
}