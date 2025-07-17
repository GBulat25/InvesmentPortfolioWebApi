using Microsoft.IdentityModel.Tokens;
using StocksWebApi.Interfaces;
using StocksWebApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StocksWebApi.Service
{
    /// <summary>
    /// Сервис для генерации JWT-токенов.
    /// Реализует интерфейс ITokenService.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration; // Конфигурация приложения
        private readonly SymmetricSecurityKey _key; // Ключ для подписи токена

        /// <summary>
        /// Конструктор сервиса. Инициализирует конфигурацию и ключ для подписи токена.
        /// </summary>
        /// <param name="configuration">Конфигурация приложения</param>
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"])); // Получаем ключ из конфигурации
        }

        /// <summary>
        /// Создаёт JWT-токен для указанного пользователя.
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>JWT-токен в виде строки</returns>
        public string CreateToken(AppUser user)
        {
            // Создаём список claims (утверждений) для токена
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email), // Email пользователя
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName) // Имя пользователя
            };

            // Создаём учётные данные для подписи токена
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // Описываем параметры токена
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), // Утверждения (claims)
                Expires = DateTime.Now.AddDays(7), // Время истечения токена (7 дней)
                SigningCredentials = creds, // Учётные данные для подписи
                Issuer = _configuration["JWT:Issuer"], // Издатель токена
                Audience = _configuration["JWT:Audience"] // Аудитория токена
            };

            // Создаём и записываем токен
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor); // Создаём токен
            return tokenHandler.WriteToken(token); // Преобразуем токен в строку
        }
    }
}