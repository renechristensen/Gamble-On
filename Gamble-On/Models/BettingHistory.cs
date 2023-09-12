using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace Gamble_On.Models
{
    public class BettingHistory
    {
        public int id { get; set; }
        public int walletID { get; set; }
        public float bettingAmount { get; set; }
        public int playedGameId { get; set; }
        public DateTime createdTime { get; set; }
        public bool outcome { get; set; }
        public float bettingResult { get; set; }
    }
}

