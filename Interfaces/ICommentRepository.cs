using StocksWebApi.Models;

namespace StocksWebApi.Interfaces
{
    public interface ICommentRepository
    {
        public Task<List<Comment>> GetAllAsync();
    }
}
