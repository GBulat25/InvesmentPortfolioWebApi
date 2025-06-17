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

        public async  Task<Comment?> DeleteAsync(Guid id)
        {
            var commentModel= await _stockDBContext.Comments.FirstOrDefaultAsync(c => c.Id==id);
            if (commentModel == null)
            {
                return null;
            }
            _stockDBContext.Remove(commentModel);
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

        public async Task<Comment?> UpdateAsync(Guid id, Comment commentModel)
        {
            var existingComment= await _stockDBContext.Comments.FindAsync(id);
            if (existingComment == null)
            {
                return null;
            }
            existingComment.Title = commentModel.Title;
            existingComment.Content = commentModel.Content;
            await _stockDBContext.SaveChangesAsync();
            return existingComment;
        }
    }
}
