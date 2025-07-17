using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StocksWebApi.DTOs.Comment;
using StocksWebApi.Extensions;
using StocksWebApi.Interfaces;
using StocksWebApi.Mappers;
using StocksWebApi.Models;

namespace StocksWebApi.Controllers
{
    /// <summary>
    /// Контроллер для управления комментариями.
    /// Поддерживает CRUD-операции (создание, чтение, обновление, удаление).
    /// </summary>
    [Route("api/comment")] // Базовый маршрут для всех эндпоинтов
    [ApiController] // Указывает, что контроллер работает как API
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo; // Репозиторий для работы с комментариями
        private readonly IStockRepository _stockRepo; // Репозиторий для проверки существования акций
        private readonly UserManager<AppUser> _userManager; // Для получения текущего пользователя

        /// <summary>
        /// Конструктор контроллера. Инициализирует необходимые сервисы.
        /// </summary>
        public CommentController(
            ICommentRepository commentRepository,
            IStockRepository stockRepo,
            UserManager<AppUser> userManager)
        {
            _commentRepo = commentRepository;
            _stockRepo = stockRepo;
            _userManager = userManager;
        }

        /// <summary>
        /// Получает все комментарии.
        /// </summary>
        /// <returns>Список комментариев в формате DTO</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepo.GetAllAsync(); // Получаем все комментарии из БД
            var commentDtos = comments.Select(s => s.ToCommentDTO()).ToList(); // Преобразуем в DTO
            return Ok(commentDtos); // Возвращаем список комментариев
        }

        /// <summary>
        /// Получает комментарий по его ID.
        /// </summary>
        /// <param name="id">ID комментария</param>
        /// <returns>Комментарий в формате DTO или 404, если не найден</returns>
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var comment = await _commentRepo.GetByIdAsync(id); // Ищем комментарий по ID
            if (comment == null)
            {
                return NotFound(); // Возвращаем 404, если комментарий не найден
            }
            return Ok(comment.ToCommentDTO()); // Возвращаем комментарий в формате DTO
        }

        /// <summary>
        /// Создаёт новый комментарий для конкретной акции.
        /// </summary>
        /// <param name="stockId">ID акции</param>
        /// <param name="commentDTO">DTO с данными для создания комментария</param>
        /// <returns>Созданный комментарий в формате DTO или ошибку</returns>
        [HttpPost("{stockId:Guid}")]
        public async Task<IActionResult> Create([FromRoute] Guid stockId, CreateCommentDTO commentDTO)
        {
            // Проверяем, существует ли акция
            if (_stockRepo.StockExists(stockId) == null)
            {
                return BadRequest("Stock does not exist"); // Возвращаем ошибку, если акция не найдена
            }

            // Получаем текущего пользователя
            var userName = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(userName);

            // Преобразуем DTO в модель комментария
            var commentModel = commentDTO.ToCommentFromCreate(stockId);
            commentModel.AppUserId = appUser.Id; // Привязываем комментарий к пользователю

            // Сохраняем комментарий в БД
            await _commentRepo.CreateAsync(commentModel);

            // Возвращаем созданный комментарий
            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDTO());
        }

        /// <summary>
        /// Обновляет существующий комментарий.
        /// </summary>
        /// <param name="id">ID комментария</param>
        /// <param name="commentDto">DTO с данными для обновления</param>
        /// <returns>Обновлённый комментарий в формате DTO или ошибку</returns>
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCommentReqDTO commentDto)
        {
            // Обновляем комментарий в БД
            var comment = await _commentRepo.UpdateAsync(id, commentDto.ToCommentFromUpdate());
            if (comment == null)
            {
                return NotFound("Comment not exist"); // Возвращаем 404, если комментарий не найден
            }
            return Ok(comment.ToCommentDTO()); // Возвращаем обновлённый комментарий
        }

        /// <summary>
        /// Удаляет комментарий по его ID.
        /// </summary>
        /// <param name="id">ID комментария</param>
        /// <returns>204 No Content или ошибку</returns>
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            // Удаляем комментарий из БД
            var commentModel = await _commentRepo.DeleteAsync(id);
            if (commentModel == null)
            {
                return NotFound("Comment not exist"); // Возвращаем 404, если комментарий не найден
            }
            return NoContent(); // Возвращаем 204 при успешном удалении
        }
    }
}