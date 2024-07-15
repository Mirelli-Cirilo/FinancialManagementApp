using Microsoft.AspNetCore.Identity;

namespace FinancialManagementApp.Models;

    public class ApplicationUser : IdentityUser
    {
        // Adicione propriedades personalizadas aqui
        public string? TwoFactorSecret { get; set; }
    }
