using Microsoft.AspNetCore.Identity;

namespace StocksWebApi.Models
{
    /// <summary>
    /// Модель пользователя с расширенными данными.
    /// Наследуется от IdentityUser для поддержки аутентификации и авторизации.
    /// </summary>
    public class AppUser : IdentityUser
    {
        /// <summary>
        /// Список портфелей пользователя.
        /// Каждый пользователь может иметь несколько портфелей.
        /// </summary>
        public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    }
}