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
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateStockReqDTO updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var stockModel = _stockDBContext.Stocks.FirstOrDefault(x => x.Id == id);
            if (stockModel == null)
            {
                return NotFound();
            }
            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Price = updateDto.Price;
            stockModel.Divs = updateDto.Divs;
            stockModel.Industry = updateDto.Industry;
            stockModel.MarketCap = updateDto.MarketCap;
            await _stockDBContext.SaveChangesAsync(); 
            return Ok(stockModel.ToStockDto());

        } 
    }
}