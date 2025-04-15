using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stock
{
    public class CreateStockDto
    {
        public string Symbol { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        public decimal Purchase { get; set; }

        public long MarketCap { get; set; }
    }
}