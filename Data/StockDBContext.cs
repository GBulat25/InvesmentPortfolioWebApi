using Microsoft.EntityFrameworkCore;
using StocksWebApi.Models;
using System.Security.Cryptography.X509Certificates;
namespace StocksWebApi.Data
{
    public class StockDBContext: DbContext
    {
        public StockDBContext(DbContextOptions options): base(options) 
        {
            
        }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
