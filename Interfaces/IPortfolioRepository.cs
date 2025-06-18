using StocksWebApi.Models;

namespace StocksWebApi.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<List<Stock>> GetUserPortfolio(AppUser user);
    }
}
