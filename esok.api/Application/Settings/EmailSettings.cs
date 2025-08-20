namespace esok.api.Application.Settings
{
    public class EmailSettings
    {
        public readonly string UserName;
        public readonly string Password;
        public readonly string Host;
        public readonly int Port;

        public EmailSettings(string userName, string password, string host, int port)
        {
            UserName = userName;
            Password = password;
            Host = host;
            Port = port;
        }
    }
}
