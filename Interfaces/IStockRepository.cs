using api.Dtos.Stock;
using api.helpers;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllStocksAsync(QueryObject query);
        Task<Stock> GetStockByIdsAsync(int id);
        Task<Stock> CreateStockAsync(Stock stockModel);
        Task<Stock> UpdateStockAsync(int id, UpdateStockDto stockDto);
        Task<Stock> DeleteByIdsAsync(int id);

        Task<bool> StockExists(int stockId);
    }
}