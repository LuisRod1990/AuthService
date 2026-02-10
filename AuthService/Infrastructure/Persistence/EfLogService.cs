using AuthService.Domain.Entitites;

namespace AuthService.Infrastructure.Persistence
{
    public class EfLogService
    {
        private readonly AuthDbContext _context;

        public EfLogService(AuthDbContext context)
        {
            _context = context;
        }

        public async Task SaveLogAsync(string level, string logger, string message, Exception? ex = null)
        {
            if (ex != null)
            {
                var logEntry = new LogEntry
                {
                    LogDate = DateTime.UtcNow,
                    LogLevel = level,
                    Logger = logger,
                    Message = message,
                    Exception = string.IsNullOrEmpty(ex.ToString()) ? "No Exception!" : ex.ToString(),
                    Thread = Thread.CurrentThread.ManagedThreadId.ToString(),
                    UserName = Environment.UserName,
                    MachineName = Environment.MachineName
                };
                _context.Logs.Add(logEntry);
                await _context.SaveChangesAsync();
            }
            else
            {
                var logEntry = new LogEntry
                {
                    LogDate = DateTime.UtcNow,
                    LogLevel = level,
                    Logger = logger,
                    Message = message,
                    Exception = "No Exception!",
                    Thread = Thread.CurrentThread.ManagedThreadId.ToString(),
                    UserName = Environment.UserName,
                    MachineName = Environment.MachineName
                };
                _context.Logs.Add(logEntry);
                await _context.SaveChangesAsync();
            }

        }

    }
}
