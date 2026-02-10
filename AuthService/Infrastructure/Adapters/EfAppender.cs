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
                LogLevel = loggingEvent.Level.Name,
                Logger = loggingEvent.LoggerName,
                Message = loggingEvent.RenderedMessage,
                Exception = loggingEvent.GetExceptionString(),
                Thread = loggingEvent.ThreadName,
                UserName = Environment.UserName,
                MachineName = Environment.MachineName
            };

            context.Logs.Add(logEntry);
            context.SaveChanges();
        }
    }
}