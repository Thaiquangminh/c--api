using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Dtos
{
    public class StockDto
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        public decimal Purchase { get; set; }

        public long MarketCap { get; set; }

        public List<CommentDto> Comments { get; set; }
    }
}