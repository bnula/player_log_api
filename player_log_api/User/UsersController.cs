using player_log_api.Contracts;
using player_log_api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace player_log_api.Controllers
{
    /// <summary>
    /// User management for Player Log API
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInMngr;
        private readonly UserManager<IdentityUser> _userMngr;
        private readonly ILoggerService _logger;
        private readonly IConfiguration _config;

        public UsersController(
            SignInManager<IdentityUser> signInMngr,
            UserManager<IdentityUser> userMngr,
            ILoggerService logger,
            IConfiguration config)
        {
            _signInMngr = signInMngr;
            _userMngr = userMngr;
            _logger = logger;
            _config = config;
        }

        /// <summary>
        /// User Login Endpoint
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UserDTO userDTO)
        {
            var controllerName = GetControllerActionNames();
            try
            {
                var userName = userDTO.EmailAddress;
                var password = userDTO.Password;

                _logger.LogInfo($"{controllerName}: Attempted Login - User Name: {userName}");

                if (userName == null)
                {
                    _logger.LogInfo($"{controllerName}: Empty User Name");
                    return BadRequest();
                }

                if (password == null)
                {
                    _logger.LogInfo($"{controllerName}: Empty Password");
                    return BadRequest();
                }

                var result = await _signInMngr.PasswordSignInAsync(userName, password, false, false);

                if (result.Succeeded == true)
                {
                    var user = await _userMngr.FindByNameAsync(userName);
                    _logger.LogInfo($"{controllerName}: Logged In - User Name: {userName}");
                    var tokenString = await GenerateJWT(user);
                    return Ok(new { token = tokenString });
                }

                _logger.LogInfo($"{controllerName}: Unauthorized - User Name: {userName}");
                return Unauthorized(userDTO);
            }
            catch (Exception ex)
            {
                return InternalError($"{controllerName}: {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// User Register Endpoint
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            var controllerName = GetControllerActionNames();
            try
            {
                var userName = userDTO.EmailAddress;
                var password = userDTO.Password;

                _logger.LogInfo($"{controllerName}: Attempted User Creation - User Name: {userName}");
                if (userName == null)
                {
                    _logger.LogWarn($"{controllerName}: Empty User Name");
                    return BadRequest();
                }
                if (password == null)
                {
                    _logger.LogWarn($"{controllerName}: Empty Password");
                    return BadRequest();
                }

                var user = new IdentityUser
                {
                    Email = userName,
                    UserName = userName
                };

                var result = await _userMngr.CreateAsync(user, password);

                if (result.Succeeded == false)
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError($"{controllerName}: {error.Code} - {error.Description}");
                    }
                    return InternalError($"{controllerName}: User Creation Failed - User Name: {userName}");
                }

                await _userMngr.AddToRoleAsync(user, "user");
                _logger.LogInfo($"{controllerName}: User Created - User Name: {userName}");
                return Created("Create", new { user });

            }
            catch (Exception ex)
            {
                return InternalError($"{controllerName}: {ex.Message} - {ex.InnerException}");
            }
        }

        private async Task<string> GenerateJWT(IdentityUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            var roles = await _userMngr.GetRolesAsync(user);
            claims.AddRange(roles.Select(
                r => new Claim(ClaimsIdentity.DefaultRoleClaimType, r))
                );
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims: claims,
                notBefore: null,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GetControllerActionNames()
        {
            var controllerName = ControllerContext.ActionDescriptor.ControllerName;
            var actionName = ControllerContext.ActionDescriptor.ActionName;
            return $"{controllerName} - {actionName}";
        }

        private ObjectResult InternalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, "Something went wrong, please contact the administrator.");
        }
    }
}
