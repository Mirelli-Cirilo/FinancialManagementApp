using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using FinancialManagementApp.Models;
using FinancialManagementApp.Services;
using FinancialManagementApp.JwtFeatures;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;  
using Microsoft.IdentityModel.Tokens;  
using System.Security.Claims;
using AutoMapper;

namespace FinancialManagementApp.controller;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITotpAuthenticator _totpAuthenticator;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;
    private readonly IMapper _mapper;
    private readonly JwtHandler _jwtHandler;

    public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ITotpAuthenticator totpAuthenticator,IConfiguration configuration, ILogger<AuthController> logger, IMapper mapper, JwtHandler jwtHandler)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _totpAuthenticator = totpAuthenticator;
        _configuration = configuration;
        _logger = logger;
        _mapper = mapper;
        _jwtHandler = jwtHandler;
    }

    // [HttpPost("register")]
    //     public async Task<IActionResult> Register([FromBody] RegisterDto model)
    //     {
    //         if (ModelState.IsValid)
    //         {
    //             var user = new ApplicationUser
    //             {
    //                 UserName = model.Email,
    //                 Email = model.Email
    //             };
    //             var result = await _userManager.CreateAsync(user, model.Password);

    //             if (result.Succeeded)
    //             {
    //                 return Ok(new { Message = "User registered successfully" });
    //             }
    //             else
    //             {
    //                 return BadRequest(result.Errors);
    //             }
    //         }

    //         return BadRequest(new { Message = "Invalid data" });
    //     }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            return Unauthorized(new LoginResponseDto { ErrorMessage = "Invalid Authentication" });

        var signingCredentials = _jwtHandler.GetSigningCredentials();
        var claims = _jwtHandler.GetClaims(user);
        var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        _logger.LogInformation("Attempting to setup 2FA for user: {User}", User.Identity.Name);
        _logger.LogInformation("User {UserName} logged in successfully", user.UserName);
        return Ok(new LoginResponseDto { IsAuthSuccessful = true, Token = token });
    }

    //[AllowAnonymous]
    // [HttpPost("login")]
    // public async Task<IActionResult> Login([FromBody] LoginRequest request)
    // {
    //     var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, lockoutOnFailure: false);

    //     if (result.Succeeded)
    //     {
    //         var user = await _userManager.FindByEmailAsync(request.Email);
    //         if (user == null)
    //         {
    //             return BadRequest("Usuário não encontrado.");
    //         }

    //         if (user.TwoFactorEnabled)
    //         {
    //             // Retorna um status que indica que o 2FA está habilitado
    //             return Ok(new { RequiresTwoFactor = true });
    //         }

    //         // Retorna um token JWT se o Two-Factor Authentication não está habilitado
    //         var token = await _userManager.GenerateJwtSecurityTokenAsync(user);
    //         return Ok(new { Token = token, RequiresTwoFactor = false });
    //     }

//     return Unauthorized("Login inválido.");
// }

    [Authorize]
    [HttpGet("enable")]
    public async Task<IActionResult> EnableTwoFactor()
    {
        
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        user.TwoFactorEnabled = true;
        

        await _userManager.UpdateAsync(user);
        // Redirect to success page 
        return Ok("You have Sucessfully Enabled Two Factor Authentication");
    }

    [Authorize]
    [HttpGet("setup")]
    public async Task<IActionResult> SetupTwoFactor()
    {
        _logger.LogInformation("Attempting to setup 2FA for user: {User}", User.Identity.Name);
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {

            _logger.LogWarning("User not found: {User}", User.Identity.Name);
            return Unauthorized();
        }

        // Generate a secret key for the user
        var secretKey = _totpAuthenticator.GenerateSecret();

        // Save the secret key to the user
        user.TwoFactorSecret = secretKey;
        await _userManager.UpdateAsync(user);

        // Generate QR code URI
        var qrCodeUri = _totpAuthenticator.GenerateSetupCode(user.Email, secretKey);

        await _userManager.UpdateAsync(user);

        return Ok(new { qrCodeUri });
    }

    [HttpPost("verify-2fa")]
    public async Task<IActionResult> VerifyTwoFactor([FromBody] VerifyTwoFactorDto model)
    {
        var user = await _userManager.FindByNameAsync(model.Email);
        if (user == null)
        {
            return Unauthorized();
        }

        var isValid = _totpAuthenticator.ValidateCode(user.TwoFactorSecret, model.Code);
        if (isValid)
        {
            // Sign the user in after successful 2FA verification
            user.TwoFactorEnabled = true;
            await _userManager.UpdateAsync(user);
            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok(new { Message = "Two-factor authentication successful." });
        }

        return BadRequest(new { Message = "Invalid code." });
    }

    
}