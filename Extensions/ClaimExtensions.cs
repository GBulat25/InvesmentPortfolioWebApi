using System.Security.Claims;

namespace StocksWebApi.Extensions
{
    /// <summary>
    /// Расширения для работы с ClaimsPrincipal.
    /// </summary>
    public static class ClaimExtensions
    {
        /// <summary>
        /// Получает имя пользователя из ClaimsPrincipal.
        /// </summary>
        /// <param name="user">Объект ClaimsPrincipal</param>
        /// <returns>Имя пользователя или null, если не найдено</returns>
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.Claims.SingleOrDefault(
                x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")
            )?.Value;
        }
    }
}