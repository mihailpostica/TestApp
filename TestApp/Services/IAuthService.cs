using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.DTO.Requests;
using Test.DTO.Responses;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Test
{
    public interface IAuthService
    {
        public Task<AuthResult> RegisterUserAsync(RegisterRequest registerRequest );
        public Task<AuthResult> LoginAsync(string email, string password);
        public Task<AuthResult> LoginWithGoogleAsync(string token);
    }

}
