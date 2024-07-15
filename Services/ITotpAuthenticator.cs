namespace FinancialManagementApp.Services;

    public interface ITotpAuthenticator
    {
        string GenerateSecret();
        string GenerateSetupCode(string email, string secret);
        bool ValidateCode(string secret, string code);
    }
