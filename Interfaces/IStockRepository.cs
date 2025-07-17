using StocksWebApi.DTOs.Stock;
using StocksWebApi.Helpers;
using StocksWebApi.Models;

namespace StocksWebApi.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с акциями.
    /// Определяет методы для CRUD-операций (создание, чтение, обновление, удаление) и дополнительных проверок.
    /// </summary>
    public interface IStockRepository
    {
        /// <summary>
        /// Получает все акции с возможностью фильтрации и пагинации.
        /// </summary>
        /// <param name="query">Параметры запроса (фильтрация, пагинация)</param>
        /// <returns>Список акций</returns>
        Task<List<Stock>> GetAllAsync(QueryObject query);

        /// <summary>
        /// Получает акцию по её уникальному ID.
        /// </summary>
        /// <param name="id">ID акции</param>
        /// <returns>Акция или null, если не найдена</returns>
        Task<Stock?> GetByIdAsync(Guid id);

        /// <summary>
        /// Получает акцию по её символу (например, AAPL).
        /// </summary>
        /// <param name="symbol">Символ акции</param>
        /// <returns>Акция или null, если не найдена</returns>
        Task<Stock?> GetBySymbolAsync(string symbol);

        /// <summary>
        /// Создаёт новую акцию.
        /// </summary>
        /// <param name="stockModel">Модель акции</param>
        /// <returns>Созданная акция</returns>
        Task<Stock> CreateAsync(Stock stockModel);

        /// <summary>
        /// Обновляет существующую акцию.
        /// </summary>
        /// <param name="id">ID акции</param>
        /// <param name="updateDto">DTO с данными для обновления</param>
        /// <returns>Обновлённая акция или null, если не найдена</returns>
        Task<Stock?> UpdateAsync(Guid id, UpdateStockReqDTO updateDto);

        /// <summary>
        /// Удаляет акцию по её уникальному ID.
        /// </summary>
        /// <param name="id">ID акции</param>
        /// <returns>Удалённая акция или null, если не найдена</returns>
        Task<Stock?> DeleteByIdAsync(Guid id);

        /// <summary>
        /// Проверяет, существует ли акция с указанным ID.
        /// </summary>
        /// <param name="id">ID акции</param>
        /// <returns>True, если акция существует; иначе False</returns>
        Task<bool> StockExists(Guid id);
    }
}