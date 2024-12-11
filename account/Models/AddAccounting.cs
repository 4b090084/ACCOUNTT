using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace account.Models
{
    public class AddAccounting
    {
        public string Key { get; set; }
        public string? Type { get; set; }
        public decimal? Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Category { get; set; }
        public string? Note { get; set; }
    }
}
