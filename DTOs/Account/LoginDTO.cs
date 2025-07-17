using System.ComponentModel.DataAnnotations;

namespace StocksWebApi.DTOs.Account
{
    /// <summary>
    /// DTO для передачи данных при входе пользователя.
    /// </summary>
    public class LoginDTO
    {
        /// <summary>
        /// Имя пользователя (логин).
        /// Обязательное поле.
        /// </summary>
        [Required(ErrorMessage = "Username is required")] // Поле обязательно для заполнения
        public string UserName { get; set; }

        /// <summary>
        /// Пароль пользователя.
        /// Обязательное поле.
        /// </summary>
        [Required(ErrorMessage = "Password is required")] // Поле обязательно для заполнения
        public string Password { get; set; }
    }
}