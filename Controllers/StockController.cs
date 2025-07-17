using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StocksWebApi.Data;
using StocksWebApi.DTOs.Stock;
using StocksWebApi.Helpers;
using StocksWebApi.Interfaces;
using StocksWebApi.Mappers;
using StocksWebApi.Repository;

namespace StocksWebApi.Controllers
{
    /// <summary>
    /// Контроллер для управления акциями.
    /// Поддерживает CRUD-операции (создание, чтение, обновление, удаление).
    /// </summary>
    [Route("api/stock")] // Базовый маршрут для всех эндпоинтов
    [ApiController] // Указывает, что контроллер работает как API
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stock_repo; // Репозиторий для работы с акциями

        /// <summary>
        /// Конструктор контроллера. Инициализирует необходимые сервисы.
        /// </summary>
        public StockController(IStockRepository stock_repo)
        {
            _stock_repo = stock_repo;
        }

        /// <summary>
        /// Получает все акции с возможностью фильтрации и пагинации.
        /// </summary>
        /// <param name="query">Параметры запроса (фильтрация, сортировка, пагинация)</param>
        /// <returns>Список акций в формате DTO</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            var stocks = await _stock_repo.GetAllAsync(query); // Получаем акции из репозитория
            var stockDtos = stocks.Select(stock => stock.ToStockDto()).ToList(); // Преобразуем в DTO
            return Ok(stockDtos); // Возвращаем список акций
        }

        /// <summary>
        /// Получает акцию по её ID.
        /// </summary>
        /// <param name="id">ID акции</param>
        /// <returns>Акция в формате DTO или 404, если не найдена</returns>
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var stock = await _stock_repo.GetByIdAsync(id); // Ищем акцию по ID
            if (stock == null)
            {
                return NotFound(); // Возвращаем 404, если акция не найдена
            }
            return Ok(stock.ToStockDto()); // Возвращаем акцию в формате DTO
        }

        /// <summary>
        /// Создаёт новую акцию.
        /// </summary>
        /// <param name="createStock">DTO с данными для создания акции</param>
        /// <returns>Созданная акция в формате DTO или ошибку</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockReqDTO createStock)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Возвращаем ошибки валидации
            }

            var stockModel = createStock.ToCreateStockReqDto(); // Преобразуем DTO в модель
            await _stock_repo.CreateAsync(stockModel); // Сохраняем акцию в БД

            return CreatedAtAction(
                nameof(GetById), // Ссылка на метод получения акции
                new { id = stockModel.Id }, // Параметры маршрута
                stockModel.ToStockDto() // Возвращаем созданную акцию в формате DTO
            );
        }

        /// <summary>
        /// Обновляет существующую акцию.
        /// </summary>
        /// <param name="id">ID акции</param>
        /// <param name="updateDto">DTO с данными для обновления</param>
        /// <returns>Обновлённая акция в формате DTO или ошибку</returns>
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateStockReqDTO updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Возвращаем ошибки валидации
            }

            var stockModel = await _stock_repo.UpdateAsync(id, updateDto); // Обновляем акцию в БД
            if (stockModel == null)
            {
                return NotFound("Stock not exist"); // Возвращаем 404, если акция не найдена
            }
            return Ok(stockModel.ToStockDto()); // Возвращаем обновлённую акцию
        }

        /// <summary>
        /// Удаляет акцию по её ID.
        /// </summary>
        /// <param name="id">ID акции</param>
        /// <returns>204 No Content или ошибку</returns>
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var stockModel = await _stock_repo.DeleteByIdAsync(id); // Удаляем акцию из БД
            if (stockModel == null)
            {
                return NotFound("Stock not exist"); // Возвращаем 404, если акция не найдена
            }
            return NoContent(); // Возвращаем 204 при успешном удалении
        }
    }
}