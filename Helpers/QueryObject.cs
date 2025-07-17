namespace StocksWebApi.Helpers
{
    /// <summary>
    /// Класс для передачи параметров запроса (фильтрация, сортировка, пагинация).
    /// </summary>
    public class QueryObject
    {
        /// <summary>
        /// Символ акции (например, AAPL). Необязательное поле.
        /// </summary>
        public string? Symbol { get; set; } = null;

        /// <summary>
        /// Название компании (например, Apple Inc.). Необязательное поле.
        /// </summary>
        public string? CompanyName { get; set; } = null;

        /// <summary>
        /// Номер страницы для пагинации. По умолчанию: 1.
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Размер страницы (количество элементов на странице). По умолчанию: 20.
        /// </summary>
        public int PageSize { get; set; } = 20;
    }
}