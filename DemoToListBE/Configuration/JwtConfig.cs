namespace DemoToListBE.Configuration
{
    public class JwtConfig
    {
        public const string JwtConfigName = "JwtConfig";
        public string Secret { get; set; } = String.Empty;

    }
}
