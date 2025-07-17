using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StocksWebApi.Interfaces;
using StocksWebApi.Models;
using StocksWebApi.Extensions;

namespace StocksWebApi.Controllers
{
    /// <summary>
    /// Контроллер для управления портфелями пользователей.
    /// Поддерживает CRUD-операции (добавление, удаление, получение).
    /// </summary>
    [Route("api/portfolio")] // Базовый маршрут для всех эндпоинтов
    [ApiController] // Указывает, что контроллер работает как API
    public class PortfolioController : ControllerBase
    {
        private readonly IStockRepository _stockRepo; // Репозиторий для работы с акциями
        private readonly UserManager<AppUser> _userManager; // Для получения текущего пользователя
        private readonly IPortfolioRepository _portfolioRepo; // Репозиторий для работы с портфелями

        /// <summary>
        /// Конструктор контроллера. Инициализирует необходимые сервисы.
        /// </summary>
        public PortfolioController(
            IStockRepository stockRepo,
            UserManager<AppUser> userManager,
            IPortfolioRepository portfolioRepo)
        {
            _stockRepo = stockRepo;
            _userManager = userManager;
            _portfolioRepo = portfolioRepo;
        }

        /// <summary>
        /// Получает портфель текущего пользователя.
        /// </summary>
        /// <returns>Портфель пользователя</returns>
        [HttpGet]
        [Authorize] // Защищённый метод — доступен только авторизованным пользователям
        public async Task<IActionResult> GetUserPortfolio()
        {
            var userName = User.GetUsername(); // Получаем имя текущего пользователя
            var appUser = await _userManager.FindByNameAsync(userName); // Находим пользователя в БД
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser); // Получаем портфель пользователя
            return Ok(userPortfolio); // Возвращаем портфель
        }

        /// <summary>
        /// Добавляет акцию в портфель пользователя.
        /// </summary>
        /// <param name="symbol">Символ акции (например, AAPL)</param>
        /// <returns>201 Created или ошибку</returns>
        [HttpPost]
        [Authorize] // Защищённый метод
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var userName = User.GetUsername(); // Получаем имя текущего пользователя
            var appUser = await _userManager.FindByNameAsync(userName); // Находим пользователя в БД
            var stock = await _stockRepo.GetBySymbolAsync(symbol); // Ищем акцию по символу

            if (stock == null)
            {
                return BadRequest("Stock not found"); // Возвращаем ошибку, если акция не найдена
            }

            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser); // Получаем текущий портфель
            if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower()))
            {
                return BadRequest("Cannot add same stock to portfolio"); // Возвращаем ошибку, если акция уже в портфеле
            }

            var portfolioModel = new Portfolio
            {
                StockId = stock.Id, // Привязываем акцию к портфелю
                AppUserId = appUser.Id // Привязываем портфель к пользователю
            };

            await _portfolioRepo.CreateAsync(portfolioModel); // Сохраняем портфель в БД

            if (portfolioModel == null)
            {
                return StatusCode(500, "Could not create"); // Возвращаем 500 при сбое
            }

            return Created(); // Возвращаем 201 Created
        }

        /// <summary>
        /// Удаляет акцию из портфеля пользователя.
        /// </summary>
        /// <param name="symbol">Символ акции (например, AAPL)</param>
        /// <returns>200 OK или ошибку</returns>
        [HttpDelete]
        [Authorize] // Защищённый метод
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var userName = User.GetUsername(); // Получаем имя текущего пользователя
            var appUser = await _userManager.FindByNameAsync(userName); // Находим пользователя в БД

            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser); // Получаем текущий портфель
            var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();

            if (filteredStock.Count() != 1)
            {
                return BadRequest("Stock not in your portfolio"); // Возвращаем ошибку, если акция не найдена в портфеле
            }

            await _portfolioRepo.DeletePortfolio(appUser, symbol); // Удаляем акцию из портфеля
            return Ok(); // Возвращаем 200 OK
        }
    }
}