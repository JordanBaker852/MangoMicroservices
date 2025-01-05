namespace Mango.Web.Service.IService
{
    public interface ITokenProvider : IDisposable
    {
        void SetToken(string token);
        string? GetToken();
        void ClearToken();
    }
}
