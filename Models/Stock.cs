using System.ComponentModel.DataAnnotations.Schema;

namespace StocksWebApi.Models
{
    /// <summary>
    /// Модель акции.
    /// Представляет данные об акциях в базе данных.
    /// </summary>
    [Table("Stocks")] // Указывает имя таблицы в БД
    public class Stock
    {
        /// <summary>
        /// Уникальный идентификатор акции.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Символ акции (например, AAPL).
        /// По умолчанию пустая строка.
        /// </summary>
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Название компании.
        /// По умолчанию пустая строка.
        /// </summary>
        public string CompanyName { get; set; } = string.Empty;

        /// <summary>
        /// Цена акции.
        /// Используется тип decimal(18,2) для точного хранения денежных значений.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        /// <summary>
        /// Дивиденды акции.
        /// Используется тип decimal(18,2) для точного хранения денежных значений.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Divs { get; set; }

        /// <summary>
        /// Отрасль компании.
        /// По умолчанию пустая строка.
        /// </summary>
        public string Industry { get; set; } = string.Empty;

        /// <summary>
        /// Рыночная капитализация компании.
        /// </summary>
        public long MarketCap { get; set; }

        /// <summary>
        /// Список комментариев, связанных с акцией.
        /// Опциональная связь "один-ко-многим".
        /// </summary>
        public List<Comment> Comments { get; set; } = new List<Comment>();

        /// <summary>
        /// Список портфелей, связанных с акцией.
        /// Опциональная связь "один-ко-многим".
        /// </summary>
        public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    }
}