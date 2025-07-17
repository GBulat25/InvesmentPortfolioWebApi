using StocksWebApi.Models;

namespace StocksWebApi.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с комментариями.
    /// Определяет методы для CRUD-операций (создание, чтение, обновление, удаление).
    /// </summary>
    public interface ICommentRepository
    {
        /// <summary>
        /// Получает все комментарии.
        /// </summary>
        /// <returns>Список всех комментариев</returns>
        Task<List<Comment>> GetAllAsync();

        /// <summary>
        /// Получает комментарий по его ID.
        /// </summary>
        /// <param name="id">ID комментария</param>
        /// <returns>Комментарий или null, если не найден</returns>
        Task<Comment?> GetByIdAsync(Guid id);

        /// <summary>
        /// Создаёт новый комментарий.
        /// </summary>
        /// <param name="commentModel">Модель комментария</param>
        /// <returns>Созданный комментарий</returns>
        Task<Comment> CreateAsync(Comment commentModel);

        /// <summary>
        /// Обновляет существующий комментарий.
        /// </summary>
        /// <param name="id">ID комментария</param>
        /// <param name="commentModel">Обновлённая модель комментария</param>
        /// <returns>Обновлённый комментарий или null, если не найден</returns>
        Task<Comment?> UpdateAsync(Guid id, Comment commentModel);

        /// <summary>
        /// Удаляет комментарий по его ID.
        /// </summary>
        /// <param name="id">ID комментария</param>
        /// <returns>Удалённый комментарий или null, если не найден</returns>
        Task<Comment?> DeleteAsync(Guid id);
    }
}