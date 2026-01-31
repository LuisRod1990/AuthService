namespace AuthService.Application.UseCases
{
    public interface IRegisterUser
    {
        void Execute(string username, string password);
    }
}
