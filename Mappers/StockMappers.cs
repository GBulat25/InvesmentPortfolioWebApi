using StocksWebApi.DTOs.Stock;
using StocksWebApi.Models;

namespace StocksWebApi.Mappers
{
    /// <summary>
    /// Класс для маппинга данных между моделями и DTO для акций.
    /// </summary>
    public static class StockMappers
    {
        /// <summary>
        /// Преобразует модель акции в DTO.
        /// </summary>
        /// <param name="stockModel">Модель акции</param>
        /// <returns>DTO акции</returns>
        public static StockDTO ToStockDto(this Stock stockModel)
        {
            return new StockDTO
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Price = stockModel.Price,
                Divs = stockModel.Divs,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
                Comments = stockModel.Comments.Select(c => c.ToCommentDTO()).ToList() // Маппинг связанных комментариев
            };
        }

        /// <summary>
        /// Преобразует DTO создания акции в модель.
        /// </summary>
        /// <param name="stockModel">DTO создания акции</param>
        /// <returns>Модель акции</returns>
        public static Stock ToCreateStockReqDto(this CreateStockReqDTO stockModel)
        {
            return new Stock
            {
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Price = stockModel.Price,
                Divs = stockModel.Divs,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap
            };
        }
    }
}