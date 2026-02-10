namespace AuthService.Domain.Entitites
{
    public class LogEntry
    {
        public int Id { get; set; }
        public DateTime LogDate { get; set; }
        public string LogLevel { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string Thread { get; set; }
        public string UserName { get; set; }
        public string MachineName { get; set; }
    }
}
