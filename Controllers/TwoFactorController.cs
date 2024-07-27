using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using FinancialManagementApp.Models;
using FinancialManagementApp.Services;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;  
using Microsoft.IdentityModel.Tokens;  



namespace FinancialManagementApp.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class TwoFactorController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITotpAuthenticator _totpAuthenticator;


        public TwoFactorController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ITotpAuthenticator totpAuthenticator)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _totpAuthenticator = totpAuthenticator;

        }

        [HttpGet, Authorize]
        public async Task<IActionResult> EnableTwoFactor()
        {
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return Unauthorized("User not found");
            }

            user.TwoFactorEnabled = true;
            
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest("Failed to enable two-factor authentication");
            }

            return Ok(user);
        }

        
        [HttpPost("setup")]
        public async Task<IActionResult> SetupTwoFactor([FromBody] LoginModel model)
        {
           
            try
            {
                if (string.IsNullOrWhiteSpace(model.UserName))
                {
                    return BadRequest("UserName cannot be null or empty.");
                }

                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user == null)
                {
                    return Unauthorized("User not found");
                }

                // Generate a secret key for the user
                var secretKey = _totpAuthenticator.GenerateSecret();

                // Save the secret key to the user
                user.TwoFactorSecret = secretKey;
                var updateResult = await _userManager.UpdateAsync(user);
            
                if (!updateResult.Succeeded)
                {
                    throw new Exception("Failed to update user with TwoFactorSecret");
                }

                // Generate QR code URI
                var qrCodeUri = _totpAuthenticator.GenerateSetupCode(user.UserName, secretKey);

                return Ok(new { qrCodeUri });
            }
            catch (Exception ex)
            {
                // Log the exception (use a logging framework or simply log to console for now)
                Console.WriteLine($"Error setting up two-factor authentication: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("verify-2fa")]
        public async Task<IActionResult> VerifyTwoFactor([FromBody] VerifyTwoFactorDto model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                return Unauthorized("User not found");
            }

            var isValid = _totpAuthenticator.ValidateCode(user.TwoFactorSecret, model.Code);
            if (isValid)
            {
                // Habilitar 2FA no usuário e atualizar no banco de dados
                user.TwoFactorEnabled = true;
                var updateResult = await _userManager.UpdateAsync(user);

                if (!updateResult.Succeeded)
                {
                    return StatusCode(500, "Error updating user with 2FA enabled.");
                }

                // Fazer login do usuário
                await _signInManager.SignInAsync(user, isPersistent: false);

                // Gerar token JWT
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                };

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superAddingMoreBitsSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                // Talvez fazer o login da parte do identity, alem de enviar o token
                return Ok(new { Token = tokenString });
            }

            return BadRequest(new { Message = "Invalid code." });
        }

            
    }
}