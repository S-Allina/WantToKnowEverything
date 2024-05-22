namespace Kyrsach.Services.IServices
{
    public interface IUserService
    {
        Task<T> GetAllUserAsync<T>(string token = null);
    }
}
