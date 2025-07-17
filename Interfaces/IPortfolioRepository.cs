using StocksWebApi.Models;

namespace StocksWebApi.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с портфелями пользователей.
    /// Определяет методы для управления портфелями (добавление, удаление, получение).
    /// </summary>
    public interface IPortfolioRepository
    {
        /// <summary>
        /// Получает портфель пользователя.
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Список акций в портфеле пользователя</returns>
        Task<List<Stock>> GetUserPortfolio(AppUser user);

        /// <summary>
        /// Добавляет новую запись в портфель пользователя.
        /// </summary>
        /// <param name="portfolio">Модель портфеля</param>
        /// <returns>Созданная запись портфеля</returns>
        Task<Portfolio> CreateAsync(Portfolio portfolio);

        /// <summary>
        /// Удаляет акцию из портфеля пользователя.
        /// </summary>
        /// <param name="appUser">Пользователь</param>
        /// <param name="symbol">Символ акции (например, AAPL)</param>
        /// <returns>Удалённая запись портфеля</returns>
        Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol);
    }
}