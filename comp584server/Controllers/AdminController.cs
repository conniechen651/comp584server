using comp584server.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using WorldModel;

namespace comp584server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(UserManager<WorldModelUser> userManager, JwtHandler jwtHandler) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            WorldModelUser? worldUser = await userManager.FindByNameAsync(loginRequest.Username);
            //if (worldUser is null || !await userManager.CheckPasswordAsync(worldUser, loginRequest.Password))
            //{
            //    Response.StatusCode = StatusCodes.Status401Unauthorized;
            //    await Response.WriteAsync("Invalid username or password.");
            //    return;
            //}
            if (worldUser == null)
            {
                return Unauthorized("Invalid username.");
            }
            bool loginStatus = await userManager.CheckPasswordAsync(worldUser, loginRequest.Password);
            if(!loginStatus)
            {
                return Unauthorized("Invalid password.");
            }
            JwtSecurityToken token  = await jwtHandler.GenerateTokenAsync(worldUser);
            string stringToken = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new LoginResponse
            {
                Success = true,
                Message = "Mom loves me",
                Token = stringToken
            });
        }
    }
}
