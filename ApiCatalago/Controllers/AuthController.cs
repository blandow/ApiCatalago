
using ApiCatalago.DTO;
using ApiCatalago.Models;
using ApiCatalago.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace ApiCatalago.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManeger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(ITokenService tokenService, UserManager<ApplicationUser> userManeger, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _tokenService = tokenService;
            _userManeger = userManeger;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelDTO model)
        {
            var user = await _userManeger.FindByNameAsync(model.UserName!);

            if(user != null && await _userManeger.CheckPasswordAsync(user, model.Password!))
            {
                var userRoles = await _userManeger.GetRolesAsync(user);
                var authClaims = new List<Claim> 
                { 
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach(var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }
                var token = _tokenService.GenerateAccessToken(authClaims, _configuration);

                var refreshToken = _tokenService.GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);

                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(refreshTokenValidityInMinutes);

                await _userManeger.UpdateAsync(user);

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                });

            }

            return Unauthorized();
        }
        
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModelDTO model)
        {
            var validUser = await _userManeger.FindByNameAsync(model.UserName!);

            if(validUser != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new ResponseDTO { 
                        Status = "Error",
                        Message = "User already exists!"
                });
            }

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName
            };
            
            var result = await _userManeger.CreateAsync(user, model.Password!);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResponseDTO
                    {
                        Status = "Error",
                        Message = "User creation failed!"
                    });
            }

            return Ok(new ResponseDTO
            {
                Status = "Success",
                Message = "User created successfully!"
            });
        }

        [HttpPost]
        [Route("refreshToken")]
        public async Task<IActionResult> RefreshToken(TokenModelDTO tokenModel)
        {
            if(tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }
            
            string? accessToken = tokenModel.AccesToken ?? throw new ArgumentNullException(nameof(tokenModel));

            string? refreshToken = tokenModel.RefreshToken ?? throw new ArgumentNullException(nameof(tokenModel));

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);

            if(principal == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            string userName = principal.Identity.Name;

            var user = await _userManeger.FindByNameAsync(userName!);

            if(user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return BadRequest("Invalid access token, refresh token or the refresh token has expired");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManeger.UpdateAsync(user);


            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken,
            });
        }

        [Authorize]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await _userManeger.FindByNameAsync(username);

            if(user == null)
            {
                return BadRequest("Invalid User");
            }

            user.RefreshToken = null;
            await _userManeger.UpdateAsync(user);

            return NoContent();
        }
    }
}
