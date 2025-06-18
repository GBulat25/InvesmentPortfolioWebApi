using Microsoft.EntityFrameworkCore;
using StocksWebApi.Data;
using StocksWebApi.Interfaces;
using StocksWebApi.Models;

namespace StocksWebApi.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly StockDBContext _dbContext;
        public PortfolioRepository(StockDBContext context)
        {
            _dbContext = context;
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _dbContext.Portfolios.AddAsync(portfolio);
            await _dbContext.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol)
        {
            var portfolioModel= await _dbContext.Portfolios.FirstOrDefaultAsync(x=> x.AppUserId== appUser.Id && x.Stock.Symbol.ToLower()== symbol.ToLower());
            if (portfolioModel == null)
            {
                return null;
            }
            _dbContext.Portfolios.Remove(portfolioModel);
            await _dbContext.SaveChangesAsync();
            return portfolioModel;
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await _dbContext.Portfolios.Where(u=> u.AppUserId==user.Id).Select(stock=> new Stock
            {
                Id=stock.StockId,
                Symbol=stock.Stock.Symbol,
                CompanyName=stock.Stock.CompanyName,
                Price=stock.Stock.Price,
                Divs=stock.Stock.Divs,
                Industry=stock.Stock.Industry,
                MarketCap=stock.Stock.MarketCap
            }).ToListAsync();
        }
    }
}
