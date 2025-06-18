using StocksWebApi.DTOs.Stock;
using StocksWebApi.Helpers;
using StocksWebApi.Models;

namespace StocksWebApi.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObject query);
        Task<Stock?> GetByIdAsync(Guid id);
        Task<Stock?> GetBySymbolAsync(string symbol);
        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock?> UpdateAsync(Guid id, UpdateStockReqDTO updateDto);
        Task<Stock?> DeleteByIdAsync(Guid id);
        Task<bool> StockExists(Guid id);

    }
}
