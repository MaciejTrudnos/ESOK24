namespace esok.api.Application.Common
{
    public static class AppDate
    {
        private static DateTime _dateTimeNow;

        public static DateTime DateTimeNow 
        {
            get { return DateTime.UtcNow; }   
            private set { _dateTimeNow = value; }
        }
    }
}
