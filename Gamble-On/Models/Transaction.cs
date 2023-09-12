using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamble_On.Models
{
    public class Transaction
    {
        public int id { get; set; }
        public int walletId { get; set; }
        public int amount { get; set; }
        public DateTime actionTime { get; set; }
    }
}

