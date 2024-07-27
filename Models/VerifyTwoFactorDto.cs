namespace FinancialManagementApp.Models;

    public class VerifyTwoFactorDto
    {
        public string? UserName { get; set; }
        public string Code { get; set; }
    }