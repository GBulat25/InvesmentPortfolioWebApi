using Microsoft.AspNetCore.Mvc;
using StocksWebApi.Interfaces;
using StocksWebApi.Mappers;

namespace StocksWebApi.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController:ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepo= commentRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments= await _commentRepo.GetAllAsync();
            var commentDtos = comments.Select(s => s.ToCommentDTO()).ToList();
            return Ok(commentDtos);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDTO());
        }
    }
}
