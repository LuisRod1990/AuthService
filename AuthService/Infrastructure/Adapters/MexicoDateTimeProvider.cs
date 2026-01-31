using AuthService.Domain.Ports;

namespace AuthService.Infrastructure.Adapters
{
    public class MexicoDateTimeProvider : IDateTimeProvider
    {
        private readonly TimeZoneInfo _mexicoTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
        public DateTime NowMexico => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _mexicoTimeZone);
    }
}
