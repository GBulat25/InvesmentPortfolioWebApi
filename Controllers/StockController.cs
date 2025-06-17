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
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stock_repo;

        public StockController(IStockRepository stock_repo)
        {
            _stock_repo=stock_repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            var stocks = await _stock_repo.GetAllAsync(query);

            var stockDtos = stocks.Select(stock => stock.ToStockDto()).ToList();

            return Ok(stockDtos);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var stock = await _stock_repo.GetByIdAsync(id);
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
            await _stock_repo.CreateAsync(stockModel);

            return CreatedAtAction(
                nameof(GetById),
                new { id = stockModel.Id },
                stockModel.ToStockDto()
            );
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateStockReqDTO updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var stockModel = await _stock_repo.UpdateAsync(id, updateDto);
            if (stockModel == null)
            {
                return NotFound("Stock not exist");
            } 
            return Ok(stockModel.ToStockDto());
        } 
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var stockModel=await _stock_repo.DeleteByIdAsync(id);
            if(stockModel == null)
            {
                return NotFound("Stock not exist");
            }
            
            return NoContent();
        }
    }
}