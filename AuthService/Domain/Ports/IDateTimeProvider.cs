namespace AuthService.Domain.Ports
{
    public interface IDateTimeProvider
    {
        DateTime NowMexico { get; }
    }
}
