using Microsoft.EntityFrameworkCore;
using StocksWebApi.Data;
using StocksWebApi.DTOs.Stock;
using StocksWebApi.Helpers;
using StocksWebApi.Interfaces;
using StocksWebApi.Mappers;
using StocksWebApi.Models;

namespace StocksWebApi.Repository
{
    /// <summary>
    /// Репозиторий для работы с акциями.
    /// Реализует интерфейс IStockRepository.
    /// </summary>
    public class StockRepository : IStockRepository
    {
        private readonly StockDBContext _stockDBContext; // Контекст базы данных

        /// <summary>
        /// Конструктор репозитория. Инициализирует контекст базы данных.
        /// </summary>
        /// <param name="stockDBContext">Контекст базы данных</param>
        public StockRepository(StockDBContext stockDBContext)
        {
            _stockDBContext = stockDBContext;
        }

        /// <summary>
        /// Получает все акции с возможностью фильтрации и пагинации.
        /// </summary>
        /// <param name="query">Параметры запроса (фильтрация, пагинация)</param>
        /// <returns>Список акций</returns>
        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocks = _stockDBContext.Stocks
                .Include(c => c.Comments) // Подгружаем связанные комментарии
                .ThenInclude(a => a.AppUser) // Подгружаем данные пользователей комментариев
                .AsQueryable(); // Преобразуем в IQueryable для дальнейшей фильтрации

            // Фильтрация по названию компании
            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            }

            // Фильтрация по символу акции
            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }

            // Пагинация
            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        /// <summary>
        /// Создаёт новую акцию.
        /// </summary>
        /// <param name="stockModel">Модель акции</param>
        /// <returns>Созданная акция</returns>
        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _stockDBContext.Stocks.AddAsync(stockModel); // Добавляем акцию в таблицу Stocks
            await _stockDBContext.SaveChangesAsync(); // Сохраняем изменения
            return stockModel; // Возвращаем созданную акцию
        }

        /// <summary>
        /// Удаляет акцию по её уникальному ID.
        /// </summary>
        /// <param name="id">ID акции</param>
        /// <returns>Удалённая акция или null, если не найдена</returns>
        public async Task<Stock?> DeleteByIdAsync(Guid id)
        {
            var stockModel = await _stockDBContext.Stocks.FirstOrDefaultAsync(y => y.Id == id); // Ищем акцию
            if (stockModel == null)
            {
                return null; // Возвращаем null, если акция не найдена
            }
            _stockDBContext.Remove(stockModel); // Удаляем акцию из таблицы Stocks
            await _stockDBContext.SaveChangesAsync(); // Сохраняем изменения
            return stockModel; // Возвращаем удалённую акцию
        }

        /// <summary>
        /// Получает акцию по её уникальному ID.
        /// </summary>
        /// <param name="id">ID акции</param>
        /// <returns>Акция или null, если не найдена</returns>
        public async Task<Stock?> GetByIdAsync(Guid id)
        {
            return await _stockDBContext.Stocks
                .Include(c => c.Comments) // Подгружаем связанные комментарии
                .FirstOrDefaultAsync(i => i.Id == id); // Ищем акцию по ID
        }

        /// <summary>
        /// Обновляет существующую акцию.
        /// </summary>
        /// <param name="id">ID акции</param>
        /// <param name="updateDto">DTO с данными для обновления</param>
        /// <returns>Обновлённая акция или null, если не найдена</returns>
        public async Task<Stock?> UpdateAsync(Guid id, UpdateStockReqDTO updateDto)
        {
            var stockModel = await _stockDBContext.Stocks.FirstOrDefaultAsync(x => x.Id == id); // Ищем акцию
            if (stockModel == null)
            {
                return null; // Возвращаем null, если акция не найдена
            }

            // Обновляем данные акции
            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Price = updateDto.Price;
            stockModel.Divs = updateDto.Divs;
            stockModel.Industry = updateDto.Industry;
            stockModel.MarketCap = updateDto.MarketCap;

            await _stockDBContext.SaveChangesAsync(); // Сохраняем изменения
            return stockModel; // Возвращаем обновлённую акцию
        }

        /// <summary>
        /// Проверяет, существует ли акция с указанным ID.
        /// </summary>
        /// <param name="id">ID акции</param>
        /// <returns>True, если акция существует; иначе False</returns>
        public async Task<bool> StockExists(Guid id)
        {
            return await _stockDBContext.Stocks.AnyAsync(s => s.Id == id);
        }

        /// <summary>
        /// Получает акцию по её символу.
        /// </summary>
        /// <param name="symbol">Символ акции</param>
        /// <returns>Акция или null, если не найдена</returns>
        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _stockDBContext.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
        }
    }
}