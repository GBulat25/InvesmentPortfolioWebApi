using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StocksWebApi.Interfaces;
using StocksWebApi.Models;
using StocksWebApi.Extensions;

namespace StocksWebApi.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPortfolioRepository _portfolioRepo;
        public PortfolioController(IStockRepository stockRepo, UserManager<AppUser> userManager, IPortfolioRepository portfolioRepo)
        {
            _stockRepo = stockRepo;
            _userManager = userManager;
            _portfolioRepo = portfolioRepo;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var userName = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(userName);
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
            return Ok(userPortfolio);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var userName = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(userName);
            var stock = await _stockRepo.GetBySymbolAsync(symbol);
            if (stock == null)
            {
                return BadRequest("Stock not found");
            }
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
            if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower()))
            {
                return BadRequest("Cannot add same stock to portfolio");
            }
            var portfolioModel = new Portfolio
            {
                StockId = stock.Id,
                AppUserId = appUser.Id
            };
            await _portfolioRepo.CreateAsync(portfolioModel);
            if (portfolioModel == null)
            {
                return StatusCode(500, "Could not create");
            }
            else
            {
                return Created();
            }
        }
    }

}
