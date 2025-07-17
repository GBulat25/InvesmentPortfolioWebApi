using StocksWebApi.Models;

namespace StocksWebApi.Interfaces
{
    /// <summary>
    /// Интерфейс для работы с JWT-токенами.
    /// Определяет метод для создания токена на основе данных пользователя.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Создаёт JWT-токен для указанного пользователя.
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>JWT-токен в виде строки</returns>
        string CreateToken(AppUser user);
    }
}