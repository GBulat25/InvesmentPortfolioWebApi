using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StocksWebApi.DTOs.Account;
using StocksWebApi.Interfaces;
using StocksWebApi.Models;

namespace StocksWebApi.Controllers
{
    /// <summary>
    /// Контроллер для управления учётными записями пользователей.
    /// Поддерживает регистрацию и авторизацию через JWT-токены.
    /// </summary>
    [Route("api/account")] // Базовый маршрут для всех эндпоинтов
    [ApiController] // Указывает, что контроллер работает как API
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager; // Для управления пользователями
        private readonly ITokenService _tokenService; // Для генерации JWT-токенов
        private readonly SignInManager<AppUser> _signInManager; // Для проверки пароля

        /// <summary>
        /// Конструктор контроллера. Инициализирует необходимые сервисы.
        /// </summary>
        public AccountController(
            UserManager<AppUser> userManager,
            ITokenService tokenService,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Эндпоинт для входа в систему.
        /// Проверяет логин и пароль, возвращает JWT-токен.
        /// </summary>
        /// <param name="loginDto">DTO с данными для входа (логин и пароль)</param>
        /// <returns>JWT-токен или сообщение об ошибке</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            // Проверяем, что данные валидны (например, заполнены все поля)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Возвращаем ошибки валидации
            }

            // Находим пользователя по имени
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName);
            if (user == null)
            {
                return Unauthorized("Invalid Username"); // Неверный логин
            }

            // Проверяем пароль
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Username not found and/or password incorrect"); // Неверный пароль
            }

            // Если всё успешно, возвращаем токен
            return Ok(
                new NewUserDTO
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user) // Генерация JWT-токена
                }
            );
        }

        /// <summary>
        /// Эндпоинт для регистрации нового пользователя.
        /// Создаёт пользователя и назначает роль "User".
        /// </summary>
        /// <param name="registerDto">DTO с данными для регистрации</param>
        /// <returns>JWT-токен или сообщение об ошибке</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            try
            {
                // Проверяем, что данные валидны
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Возвращаем ошибки валидации
                }

                // Создаём нового пользователя
                var appUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email
                };

                // Пытаемся создать пользователя в БД
                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);
                if (createdUser.Succeeded)
                {
                    // Назначаем роль "User"
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        // Возвращаем токен
                        return Ok(
                            new NewUserDTO
                            {
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                                Token = _tokenService.CreateToken(appUser) // Генерация JWT-токена
                            }
                        );
                    }
                    else
                    {
                        return BadRequest(roleResult.Errors); // Ошибки при назначении роли
                    }
                }
                else
                {
                    return BadRequest(createdUser.Errors); // Ошибки при создании пользователя
                }
            }
            catch (Exception e)
            {
                // Логируем ошибку (можно использовать ILogger)
                return StatusCode(500, "Internal server error"); // Возвращаем 500 при сбое
            }
        }
    }
}