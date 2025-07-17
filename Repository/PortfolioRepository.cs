using Microsoft.EntityFrameworkCore;
using StocksWebApi.Data;
using StocksWebApi.Interfaces;
using StocksWebApi.Models;

namespace StocksWebApi.Repository
{
    /// <summary>
    /// Репозиторий для работы с портфелями пользователей.
    /// Реализует интерфейс IPortfolioRepository.
    /// </summary>
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly StockDBContext _dbContext; // Контекст базы данных

        /// <summary>
        /// Конструктор репозитория. Инициализирует контекст базы данных.
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        public PortfolioRepository(StockDBContext context)
        {
            _dbContext = context;
        }

        /// <summary>
        /// Добавляет новую запись в портфель пользователя.
        /// </summary>
        /// <param name="portfolio">Модель портфеля</param>
        /// <returns>Созданная запись портфеля</returns>
        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _dbContext.Portfolios.AddAsync(portfolio); // Добавляем запись в таблицу Portfolios
            await _dbContext.SaveChangesAsync(); // Сохраняем изменения
            return portfolio; // Возвращаем созданную запись
        }

        /// <summary>
        /// Удаляет акцию из портфеля пользователя по символу акции.
        /// </summary>
        /// <param name="appUser">Пользователь</param>
        /// <param name="symbol">Символ акции (например, AAPL)</param>
        /// <returns>Удалённая запись портфеля или null, если не найдена</returns>
        public async Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol)
        {
            var portfolioModel = await _dbContext.Portfolios
                .FirstOrDefaultAsync(x => x.AppUserId == appUser.Id && x.Stock.Symbol.ToLower() == symbol.ToLower()); // Ищем запись портфеля

            if (portfolioModel == null)
            {
                return null; // Возвращаем null, если запись не найдена
            }

            _dbContext.Portfolios.Remove(portfolioModel); // Удаляем запись из таблицы Portfolios
            await _dbContext.SaveChangesAsync(); // Сохраняем изменения
            return portfolioModel; // Возвращаем удалённую запись
        }

        /// <summary>
        /// Получает список акций в портфеле пользователя.
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Список акций в портфеле пользователя</returns>
        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await _dbContext.Portfolios
                .Where(u => u.AppUserId == user.Id) // Фильтруем записи портфеля по ID пользователя
                .Select(stock => new Stock
                {
                    Id = stock.StockId, // ID акции
                    Symbol = stock.Stock.Symbol, // Символ акции
                    CompanyName = stock.Stock.CompanyName, // Название компании
                    Price = stock.Stock.Price, // Цена акции
                    Divs = stock.Stock.Divs, // Дивиденды
                    Industry = stock.Stock.Industry, // Отрасль
                    MarketCap = stock.Stock.MarketCap // Рыночная капитализация
                })
                .ToListAsync(); // Преобразуем результат в список
        }
    }
}