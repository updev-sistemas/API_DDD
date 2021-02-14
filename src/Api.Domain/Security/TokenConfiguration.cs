namespace Domain.Security
{
    public class TokenConfiguration
    {
        public virtual string Audience { get; set; }
        public virtual string Issuer { get; set; }
        public virtual int SecoundTime { get; set; }
    }
}
