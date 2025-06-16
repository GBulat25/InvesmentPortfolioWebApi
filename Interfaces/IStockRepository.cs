using StocksWebApi.DTOs.Stock;
using StocksWebApi.Models;

namespace StocksWebApi.Interfaces
{
    public interface IStockRepository
    {
        public Task<List<Stock>> GetAllAsync();
        public Task<Stock?> GetByIdAsync(Guid id);
        public Task<Stock> CreateAsync(Stock stockModel);
        public Task<Stock?> UpdateAsync(Guid id, UpdateStockReqDTO updateDto);
        public Task<Stock?> DeleteByIdAsync(Guid id);
    }
}
