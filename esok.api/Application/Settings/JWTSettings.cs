namespace esok.api.Application.Settings
{
    public class JWTSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int DurationInMinutes { get; set; }

        public JWTSettings(string key, string issuer, string audience, int durationInMinutes)
        {
            Key = key;
            Issuer = issuer;
            Audience = audience;
            DurationInMinutes = durationInMinutes;
        }
    }
}
