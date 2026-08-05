using ApiCatalago.DTO;
using ApiCatalago.Models;
using ApiCatalago.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ApiCatalago.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManeger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CategoriasController> _logger;

        public AuthController(ITokenService tokenService, UserManager<ApplicationUser> userManeger, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger<CategoriasController> logger)
        {
            _tokenService = tokenService;
            _userManeger = userManeger;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        [Route("CreateRole")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateRole (string roleName)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExist)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));

                if (roleResult.Succeeded)
                {
                    _logger.LogInformation("Role adicionada: " + roleName);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDTO
                    {
                        Status = "Success",
                        Message = "Role created successfully!"
                    });
                }
                else
                {
                    _logger.LogWarning("Erro ao criar role: " + roleName);
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new ResponseDTO { Status = "Error", Message = $"Error creating role {roleName}" });
                }

                }
            
            return StatusCode(StatusCodes.Status400BadRequest,
                new ResponseDTO { Status = "Error", Message = $"Role {roleName} already exists!" });
        }

        [HttpPost]
        [Route("AddUserToRole")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var user = await _userManeger.FindByEmailAsync(email);

            if (user != null)
            {
                var result = await _userManeger.AddToRoleAsync(user, roleName);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"user {user.UserName} email {email} added to {roleName} !");
                    return StatusCode(StatusCodes.Status200OK,
                        new ResponseDTO
                        {
                            Status = "Success",
                            Message = $"email {user.Email} added to {roleName}"
                        });
                }
                else
                {
                    _logger.LogError($"unable to add {user.Email} to {roleName}");
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseDTO
                    {
                        Status = "Error",
                        Message = $"Error: unable to add user {user.UserName} - {user.Email} to the {roleName} role"
                    });
                }
            }

            _logger.LogError($"Unable to find user {email}");
                return BadRequest(new { error = "Unable to find user" });
        }


        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesDefaultResponseType]
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
                    new Claim("id",user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach(var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }
                var token = _tokenService.GenerateAccessToken(authClaims, _configuration);

                var refreshToken = _tokenService.GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);

                user.RefreshToken = refreshToken;
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("revoke/{username}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
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
