using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Domain.Security
{
    public class SigninConfiguration
    {
        public SigninConfiguration()
        {
            using RSACryptoServiceProvider provider = new RSACryptoServiceProvider(2048);
            Key = new RsaSecurityKey(provider.ExportParameters(true));

            Credential = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);
        }

        public virtual SecurityKey Key { get; set; }
        public virtual SigningCredentials Credential { get; set; }
    }
}
