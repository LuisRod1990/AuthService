using AuthService.Domain.Entitites;
using AuthService.Infrastructure.Persistence;

namespace AuthService.Infrastructure.Adapters
{
    public class EfAppender : log4net.Appender.AppenderSkeleton
    {
        private readonly IServiceProvider _serviceProvider;

        public EfAppender(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AuthDbContext>();

            var logEntry = new LogEntry
            {
                LogDate = loggingEvent.TimeStamp,
                LogLevel = loggingEvent.Level.Name ?? string.Empty,
                Logger = loggingEvent.LoggerName ?? string.Empty,
                Message = loggingEvent.RenderedMessage ?? string.Empty,
                Exception = loggingEvent.GetExceptionString() ?? string.Empty,
                Thread = loggingEvent.ThreadName ?? string.Empty,
                UserName = Environment.UserName,
                MachineName = Environment.MachineName
            };

            context.Logs.Add(logEntry);
            context.SaveChanges();
        }
    }
}