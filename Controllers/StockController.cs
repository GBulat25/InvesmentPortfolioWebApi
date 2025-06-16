using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StocksWebApi.Data;
using StocksWebApi.DTOs.Stock;
using StocksWebApi.Mappers;

namespace StocksWebApi.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly StockDBContext _stockDBContext;

        public StockController(StockDBContext stockDBContext)
        {
            _stockDBContext = stockDBContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _stockDBContext.Stocks.ToListAsync();

            var stockDtos = stocks.Select(stock => stock.ToStockDto()).ToList();

            return Ok(stockDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var stock = await _stockDBContext.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockReqDTO createStock)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var stockModel = createStock.ToCreateStockReqDto();
            await _stockDBContext.Stocks.AddAsync(stockModel);
            await _stockDBContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = stockModel.Id },
                stockModel.ToStockDto()
            );
        }
    }
}