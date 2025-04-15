using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DatabaseContext;
using api.Dtos.Stock;
using api.helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;

        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<Stock>> GetAllStocksAsync(QueryObject query)
        {
            var stocks = _context.Stocks.Include(s => s.Comments).AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            }
            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.IsDescending == true)
                {
                    stocks = stocks.OrderByDescending(s => EF.Property<object>(s, query.SortBy));
                }
                else
                {
                    stocks = stocks.OrderBy(s => EF.Property<object>(s, query.SortBy));
                }
            }

            var skipNumber = (query.Page - 1) * query.Size;

            return await stocks.Skip(skipNumber).Take(query.Size).ToListAsync();
        }

        public async Task<Stock> GetStockByIdsAsync(int id)
        {
            var stock = await _context.Stocks.Include(s => s.Comments).FirstOrDefaultAsync(x => x.Id == id);
            if (stock == null)
            {
                throw new Exception("Stock not found");
            }
            return stock;
        }

        public async Task<Stock> CreateStockAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock> DeleteByIdsAsync(int id)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (stock == null)
            {
                throw new Exception("Stock not found");
            }
            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<Stock> UpdateStockAsync(int id, UpdateStockDto stockDto)
        {
            var stock = _context.Stocks.FirstOrDefault(x => x.Id == id);
            if (stock == null)
            {
                throw new Exception("Stock not found");
            }
            stock.Symbol = stockDto.Symbol;
            stock.CompanyName = stockDto.CompanyName;
            stock.Purchase = stockDto.Purchase;
            stock.MarketCap = stockDto.MarketCap;
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<bool> StockExists(int stockId)
        {
            return await _context.Stocks.AnyAsync(x => x.Id == stockId);
        }
    }
}