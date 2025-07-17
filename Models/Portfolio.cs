using System.ComponentModel.DataAnnotations.Schema;

namespace StocksWebApi.Models
{
    /// <summary>
    /// Модель портфеля.
    /// Представляет связь между пользователем и акциями.
    /// </summary>
    [Table("Portfolios")] // Указывает имя таблицы в БД
    public class Portfolio
    {
        /// <summary>
        /// ID пользователя, которому принадлежит портфель.
        /// Часть составного первичного ключа.
        /// </summary>
        public string AppUserId { get; set; }

        /// <summary>
        /// ID акции, добавленной в портфель.
        /// Часть составного первичного ключа.
        /// </summary>
        public Guid StockId { get; set; }

        /// <summary>
        /// Пользователь, которому принадлежит портфель.
        /// Связь "один-ко-многим" с AppUser.
        /// </summary>
        public AppUser AppUser { get; set; }

        /// <summary>
        /// Акция, добавленная в портфель.
        /// Связь "один-ко-многим" с Stock.
        /// </summary>
        public Stock Stock { get; set; }
    }
}