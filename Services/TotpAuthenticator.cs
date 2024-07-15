using FinancialManagementApp.Services;
using TwoFactorAuthNet;

namespace FinancialNanagementApp.Services
{
    public class TotpAuthenticator : ITotpAuthenticator
    {
        private readonly TwoFactorAuth _tfa;

        public TotpAuthenticator()
        {
            _tfa = new TwoFactorAuth();
        }

        public string GenerateSecret()
        {
            return _tfa.CreateSecret();
        }

        public string GenerateSetupCode(string email, string secret)
        {
            // Ajuste a implementação conforme necessário
            var setupCode = _tfa.GetQrCodeImageAsDataUri(email, secret);
            return setupCode;
        }

        public bool ValidateCode(string secret, string code)
        {
            return _tfa.VerifyCode(secret, code);
        }
    }
}