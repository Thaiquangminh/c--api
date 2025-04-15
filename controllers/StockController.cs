using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DatabaseContext;
using api.Dtos.Stock;
using api.helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.controllers
{
    [Route("api/stock")]
    [ApiController]

    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepository;
        public StockController(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllStocks([FromQuery] QueryObject query)
        {
            var stockEntities = await _stockRepository.GetAllStocksAsync(query);
            var stocks = stockEntities.Select(s => s.ToStockDto()).ToList();
            return Ok(stockEntities);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStockById(int id)
        {
            var stock = await _stockRepository.GetStockByIdsAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockDto stock)
        {
            var stockModel = stock.ToStockModel();
            await _stockRepository.CreateStockAsync(stockModel);
            return CreatedAtAction(nameof(GetStockById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id:int}")]

        public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] UpdateStockDto stock)
        {
            var stockModel = await _stockRepository.UpdateStockAsync(id, stock);
            if (stockModel == null)
            {
                return NotFound();
            }
            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteStock(int id)
        {
            var stock = await _stockRepository.DeleteByIdsAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}