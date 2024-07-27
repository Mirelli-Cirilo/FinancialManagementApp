using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using FinancialManagementApp.Models;
using FinancialManagementApp.Services;
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
   

    public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ITotpAuthenticator totpAuthenticator,IConfiguration configuration, ILogger<AuthController> logger, IMapper mapper)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _totpAuthenticator = totpAuthenticator;
        _configuration = configuration;
        _logger = logger;
        _mapper = mapper;
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
            return Unauthorized(new { ErrorMessage = "Invalid Authentication" });

        if (user.TwoFactorEnabled)
        {
            // Retorne um indicador que a autenticação de dois fatores é necessária
            return Ok(new { RequiresTwoFactor = true });
        }    

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

    

    
}