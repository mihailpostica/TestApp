using Google.Apis.Auth;
using System;
using System.Threading.Tasks;
using Test.DTO;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Test
{
    public class GoogleAuthService : IGoogleAuthService
    {
        GoogleAuthSettings googleAuthSettings;

        public GoogleAuthService(GoogleAuthSettings googleAuthSettings)
        {
            this.googleAuthSettings = googleAuthSettings;
        }

        public async Task<SocialUserDetails> getGoogleUserInfo(string token)
        {
           
            var payload = await validateToken(token);
            var googleUserDetails = new SocialUserDetails();
            googleUserDetails.Email = payload.Email;
            googleUserDetails.FirstName = payload.GivenName;
            googleUserDetails.LastName = payload.FamilyName;
            googleUserDetails.Id = Guid.NewGuid().ToString();
            return googleUserDetails;
        }


        public async Task<Payload> validateToken(string token)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(token);
            if (payload.Audience.ToString() != googleAuthSettings.AppKey || (payload.Issuer != googleAuthSettings.Issuer))
            {
                throw new Exception("Something Went wrong");
            }
            return payload;
        }

       
    }
}
