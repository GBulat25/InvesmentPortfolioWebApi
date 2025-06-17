using StocksWebApi.Models;

namespace StocksWebApi.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(Guid id);
    }
}
