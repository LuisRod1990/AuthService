namespace AuthService.Application.UseCases
{
    public interface IUpdateUserPassword
    {
        void Execute(int usuarioId, string newPassword);
    }
}
