using Microsoft.EntityFrameworkCore;
using StocksWebApi.Data;
using StocksWebApi.Interfaces;
using StocksWebApi.Models;

namespace StocksWebApi.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly StockDBContext _stockDBContext;
        public CommentRepository(StockDBContext stockDBContext)
        {
            _stockDBContext=stockDBContext;
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _stockDBContext.AddAsync(commentModel);
            await _stockDBContext.SaveChangesAsync();
            return commentModel;    
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _stockDBContext.Comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(Guid id)
        {
            return await _stockDBContext.Comments
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
