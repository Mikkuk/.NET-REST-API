

using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepository;
        private readonly IPortfolioRepository _portfolioRepository;
        public PortfolioController(UserManager<AppUser> userManager,
                 IStockRepository stockRepository, IPortfolioRepository portfolioRepository)
        {
            _userManager = userManager;
            _stockRepository = stockRepository;
            _portfolioRepository = portfolioRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var user = User.GetUsername();
            var AppUser = await _userManager.FindByNameAsync(user);
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(AppUser);
            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(String Symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var stock = await _stockRepository.GetBySymbolAsync(Symbol);

            if(stock == null)
            {
                return BadRequest("Stock not found");
            }

            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);

            if(userPortfolio.Any(s => s.Symbol.ToLower() == Symbol.ToLower()))
            {
                return BadRequest("Stock already in portfolio");
            }

            var portfolio = new Portfolio
            {
                AppUserId = appUser.Id,
                StockId = stock.Id
            };

            await _portfolioRepository.CreateAsync(portfolio);

            if(portfolio == null)
            {
                return BadRequest("Failed to add stock to portfolio");
            }

            return Created();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemovePortfolio(String Symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);

            var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == Symbol.ToLower()).ToList();

            if(filteredStock.Count == 1)
            {
                await _portfolioRepository.DeleteAsync(appUser, Symbol);
            } 
            else 
            {
                return BadRequest("Stock not found in portfolio");
            }

            return Ok();
        }
    }
}