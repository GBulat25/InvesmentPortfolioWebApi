using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StocksWebApi.DTOs.Comment;
using StocksWebApi.Extensions;
using StocksWebApi.Interfaces;
using StocksWebApi.Mappers;
using StocksWebApi.Models;

namespace StocksWebApi.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<AppUser> _userManager;
        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepo, UserManager<AppUser> userManager)
        {
            _commentRepo = commentRepository;
            _stockRepo = stockRepo;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepo.GetAllAsync();
            var commentDtos = comments.Select(s => s.ToCommentDTO()).ToList();
            return Ok(commentDtos);
        }
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDTO());
        }
        [HttpPost("{stockId:Guid}")]
        public async Task<IActionResult> Create([FromRoute] Guid stockId, CreateCommentDTO commentDTO)
        {
            if (_stockRepo.StockExists(stockId) == null)
            {
                return BadRequest("Stock does not exist");
            }
            var userName=User.GetUsername();
            var appUser= await _userManager.FindByNameAsync(userName);
            var commetModel=commentDTO.ToCommentFromCreate(stockId);
            commetModel.AppUserId = appUser.Id;
            await _commentRepo.CreateAsync(commetModel);
            return CreatedAtAction(nameof(GetById), new { id = commetModel.Id }, commetModel.ToCommentDTO());
        }
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCommentReqDTO commentDto)
        {
            var comment = await _commentRepo.UpdateAsync(id,commentDto.ToCommentFromUpdate());
            if (comment == null)
            {
                return NotFound("Comment not exist");
            }
            return Ok(comment.ToCommentDTO());
        }
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var commentModel = await _commentRepo.DeleteAsync(id);
            if (commentModel == null)
            {
                return NotFound("Comment not exist");
            }

            return NoContent();
        }
    }
}
