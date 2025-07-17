using Microsoft.EntityFrameworkCore;
using StocksWebApi.Data;
using StocksWebApi.Interfaces;
using StocksWebApi.Models;

namespace StocksWebApi.Repository
{
    /// <summary>
    /// Репозиторий для работы с комментариями.
    /// Реализует интерфейс ICommentRepository.
    /// </summary>
    public class CommentRepository : ICommentRepository
    {
        private readonly StockDBContext _stockDBContext; // Контекст базы данных

        /// <summary>
        /// Конструктор репозитория. Инициализирует контекст базы данных.
        /// </summary>
        /// <param name="stockDBContext">Контекст базы данных</param>
        public CommentRepository(StockDBContext stockDBContext)
        {
            _stockDBContext = stockDBContext;
        }

        /// <summary>
        /// Создаёт новый комментарий.
        /// </summary>
        /// <param name="commentModel">Модель комментария</param>
        /// <returns>Созданный комментарий</returns>
        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _stockDBContext.AddAsync(commentModel); // Добавляем комментарий в БД
            await _stockDBContext.SaveChangesAsync(); // Сохраняем изменения
            return commentModel; // Возвращаем созданный комментарий
        }

        /// <summary>
        /// Удаляет комментарий по его ID.
        /// </summary>
        /// <param name="id">ID комментария</param>
        /// <returns>Удалённый комментарий или null, если не найден</returns>
        public async Task<Comment?> DeleteAsync(Guid id)
        {
            var commentModel = await _stockDBContext.Comments.FirstOrDefaultAsync(c => c.Id == id); // Ищем комментарий
            if (commentModel == null)
            {
                return null; // Возвращаем null, если комментарий не найден
            }
            _stockDBContext.Remove(commentModel); // Удаляем комментарий из БД
            await _stockDBContext.SaveChangesAsync(); // Сохраняем изменения
            return commentModel; // Возвращаем удалённый комментарий
        }

        /// <summary>
        /// Получает все комментарии.
        /// </summary>
        /// <returns>Список всех комментариев с данными пользователей</returns>
        public async Task<List<Comment>> GetAllAsync()
        {
            return await _stockDBContext.Comments
                .Include(a => a.AppUser) // Подгружаем связанные данные пользователя
                .ToListAsync(); // Преобразуем результат в список
        }

        /// <summary>
        /// Получает комментарий по его ID.
        /// </summary>
        /// <param name="id">ID комментария</param>
        /// <returns>Комментарий или null, если не найден</returns>
        public async Task<Comment?> GetByIdAsync(Guid id)
        {
            return await _stockDBContext.Comments
                .Include(a => a.AppUser) // Подгружаем связанные данные пользователя
                .FirstOrDefaultAsync(c => c.Id == id); // Ищем комментарий по ID
        }

        /// <summary>
        /// Обновляет существующий комментарий.
        /// </summary>
        /// <param name="id">ID комментария</param>
        /// <param name="commentModel">Обновлённая модель комментария</param>
        /// <returns>Обновлённый комментарий или null, если не найден</returns>
        public async Task<Comment?> UpdateAsync(Guid id, Comment commentModel)
        {
            var existingComment = await _stockDBContext.Comments.FindAsync(id); // Ищем комментарий
            if (existingComment == null)
            {
                return null; // Возвращаем null, если комментарий не найден
            }
            existingComment.Title = commentModel.Title; // Обновляем заголовок
            existingComment.Content = commentModel.Content; // Обновляем содержание
            await _stockDBContext.SaveChangesAsync(); // Сохраняем изменения
            return existingComment; // Возвращаем обновлённый комментарий
        }
    }
}