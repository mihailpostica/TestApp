using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Test.Domain;
using Test.DTO;
using Test.DTO.Requests;
using Test.DTO.Responses;

namespace Test
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IGoogleAuthService _googleAuthService;
        private readonly AppDbContext _context;
        private readonly JwtSettings _jwtSettings;

        public AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext context, JwtSettings jwtSettings, IGoogleAuthService googleAuthService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _jwtSettings = jwtSettings;
            _googleAuthService = googleAuthService;
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            var authResult = new AuthResult();
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var isValidPassword = await _userManager.CheckPasswordAsync(user, password);
                if (isValidPassword)
                {
                    var createdToken = await GetAuthenticationResultForUserAsync(user);
                    authResult.Token = createdToken;
                    authResult.Success = true;
                }
                else
                {
                    authResult.Errors.Append("Invalid credentials");
                }
            }
            else 
            {
                authResult.Errors.Append("Invalid credentials"); 
            }
            return authResult;
        }

        public async Task<AuthResult> RegisterUserAsync(RegisterRequest registerRequest)
        { 
            var authResult = new AuthResult();
            var user = await _userManager.FindByEmailAsync(registerRequest.Email);
            if (user == null) {
                var newUser = new AppUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = registerRequest.Email,
                    UserName = registerRequest.Email,
                    FirstName = registerRequest.FirstName,
                    LastName = registerRequest.LastName
                    
                };
                var createdUser = await _userManager.CreateAsync(newUser, registerRequest.Password);
                if (createdUser.Succeeded)
                {
                    var createdToken = await GetAuthenticationResultForUserAsync(newUser);
                    authResult.Success = true;
                    authResult.Token = createdToken;     
                }
                else
                {  
                    foreach (var error in createdUser.Errors) {
                        authResult.Errors.Add(error.Description);
                        
                    }
                }
            }
            else 
            {
                authResult.Errors.Add("Account already exists");
            }
            return authResult;
        
        }



        private async Task<string> GetAuthenticationResultForUserAsync(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Id)
            };

            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role == null) continue;
                var roleClaims = await _roleManager.GetClaimsAsync(role);

                foreach (var roleClaim in roleClaims)
                {
                    if (claims.Contains(roleClaim))
                        continue;

                    claims.Add(roleClaim);
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token).ToString();
        }
      

        public async Task<AuthResult> LoginWithGoogleAsync(string token)
        {
            var authResult = new AuthResult();
            try
            {
                var userDetails = await _googleAuthService.getGoogleUserInfo(token);
                var createdToken= await LoginSocialUser(userDetails);
                authResult.Token = createdToken;
                authResult.Success = true;
            }
            catch(Exception ex)
            {
                authResult.Errors.Add(ex.Message);
            }

            return authResult;
        }


        private async Task<string> LoginSocialUser(SocialUserDetails userDetails)
        {
            var user = await _userManager.FindByEmailAsync(userDetails.Email);
            if (user != null)
            {
                return await GetAuthenticationResultForUserAsync(user);
            }
            else
            {
                var newUser = new AppUser();
                newUser.Email = userDetails.Email;
                newUser.Id = userDetails.Id;
                newUser.UserName = userDetails.Email;
                var identityResult = await _userManager.CreateAsync(newUser);
                Debug.WriteLine("Google user was registered in the system with email: " + newUser.Email);
                if (identityResult.Succeeded)
                {
                    return await GetAuthenticationResultForUserAsync(newUser);
                }
                else
                {
                    throw new Exception("Couldn't create user");
                }
                    
            }
        }

    }


}
