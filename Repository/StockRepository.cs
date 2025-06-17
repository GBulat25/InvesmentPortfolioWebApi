using Microsoft.EntityFrameworkCore;
using StocksWebApi.Data;
using StocksWebApi.DTOs.Stock;
using StocksWebApi.Helpers;
using StocksWebApi.Interfaces;
using StocksWebApi.Mappers;
using StocksWebApi.Models;


namespace StocksWebApi.Repository
{
    public class StockRepository: IStockRepository
    {
        private readonly StockDBContext _stockDBContext;
        public StockRepository(StockDBContext stockDBContext)
        {
            _stockDBContext = stockDBContext;
        }
        public async Task<List<Stock>> GetAllAsync(QueryObject query) { 
            var stocks= _stockDBContext.Stocks.Include(c=>c.Comments).AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks=stocks.Where(s=> s.CompanyName.Contains(query.CompanyName));
            }
            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }
            var skipNumber=(query.PageNumber-1)* query.PageSize;
            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _stockDBContext.Stocks.AddAsync(stockModel);
            await _stockDBContext.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteByIdAsync(Guid id)
        {
            var stockModel = await _stockDBContext.Stocks.FirstOrDefaultAsync(y => y.Id == id);
            if (stockModel == null)
            {
                return null;
            }
            _stockDBContext.Remove(stockModel);
            await _stockDBContext.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> GetByIdAsync(Guid id)
        {
            return await _stockDBContext.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i=>i.Id==id);
        }

        public async Task<Stock?> UpdateAsync(Guid id, UpdateStockReqDTO updateDto)
        {
            var stockModel = await _stockDBContext.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (stockModel == null)
            {
                return null;
            }
            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Price = updateDto.Price;
            stockModel.Divs = updateDto.Divs;
            stockModel.Industry = updateDto.Industry;
            stockModel.MarketCap = updateDto.MarketCap;
            await _stockDBContext.SaveChangesAsync();
            return stockModel;

        }

        public async Task<bool> StockExists(Guid id)
        {
            return await _stockDBContext.Stocks.AnyAsync(s=>s.Id==id);
        }
    }
}
