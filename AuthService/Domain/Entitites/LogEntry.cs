namespace AuthService.Domain.Entitites
{
    public class LogEntry
    {
        public int Id { get; set; }
        public DateTime LogDate { get; set; }
        public required string LogLevel { get; set; }
        public required string Logger { get; set; } 
        public required string Message { get; set; }
        public required string Exception { get; set; } 
        public required string Thread { get; set; }
        public required string UserName { get; set; }
        public required string MachineName { get; set; }
    }
}
