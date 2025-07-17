using System.ComponentModel.DataAnnotations.Schema;

namespace StocksWebApi.Models
{
    /// <summary>
    /// Модель комментария.
    /// Представляет данные о комментарии в базе данных.
    /// </summary>
    [Table("Comments")] // Указывает имя таблицы в БД
    public class Comment
    {
        /// <summary>
        /// Уникальный идентификатор комментария.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Заголовок комментария.
        /// По умолчанию пустая строка.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Содержание комментария.
        /// По умолчанию пустая строка.
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Дата создания комментария.
        /// По умолчанию текущая дата и время.
        /// </summary>
        public DateTime Created { get; set; } = DateTime.Now;

        /// <summary>
        /// ID акции, к которой относится комментарий (если есть).
        /// Может быть null.
        /// </summary>
        public Guid? StockId { get; set; }

        /// <summary>
        /// Акция, к которой относится комментарий.
        /// Опциональная связь "один-ко-многим".
        /// </summary>
        public Stock? Stock { get; set; }

        /// <summary>
        /// ID пользователя, создавшего комментарий.
        /// </summary>
        public string AppUserId { get; set; }

        /// <summary>
        /// Пользователь, создавший комментарий.
        /// Связь "один-ко-многим" с AppUser.
        /// </summary>
        public AppUser AppUser { get; set; }
    }
}