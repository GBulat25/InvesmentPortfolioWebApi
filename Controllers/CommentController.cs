using Microsoft.AspNetCore.Mvc;
using StocksWebApi.DTOs.Comment;
using StocksWebApi.Interfaces;
using StocksWebApi.Mappers;

namespace StocksWebApi.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepo)
        {
            _commentRepo = commentRepository;
            _stockRepo = stockRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepo.GetAllAsync();
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
        [HttpPost("{stockId}")]
        public async Task<IActionResult> Create([FromRoute] Guid stockId, CreateCommentDTO commentDTO)
        {
            if (_stockRepo.StockExists(stockId) == null)
            {
                return BadRequest("Stock does not exist");
            }
            var commetModel=commentDTO.ToCommentFromCreate(stockId);
            await _commentRepo.CreateAsync(commetModel);
            return CreatedAtAction(nameof(GetById), new { id = commetModel }, commetModel.ToCommentDTO());
        }
    }
}
