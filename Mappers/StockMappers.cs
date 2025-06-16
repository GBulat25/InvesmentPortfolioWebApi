using StocksWebApi.DTOs.Stock;
using StocksWebApi.Models;

namespace StocksWebApi.Mappers
{
    public static class StockMappers
    {
        public static StockDTO ToStockDto (this Stock stockModel)
        {
            return new StockDTO
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Price = stockModel.Price,
                Divs = stockModel.Divs,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap
            };
        }
        public static Stock ToCreateStockReqDto(this CreateStockReqDTO stockModel)
        {
            return new Stock
            {
                Symbol = stockModel.Symbol,
                CompanyName=stockModel.CompanyName,
                Price = stockModel.Price,
                Divs = stockModel.Divs,
                Industry=stockModel.Industry,
                MarketCap = stockModel.MarketCap
            };
        }
    }
}
