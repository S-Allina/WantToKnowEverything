using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

public class CustomUserIdProvider : IUserIdProvider
{
    public virtual string GetUserId(HubConnectionContext connection)
    {
        // Получить идентификатор пользователя из контекста
        return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}