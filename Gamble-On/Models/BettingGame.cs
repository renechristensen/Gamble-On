using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamble_On.Models
{
    public class BettingGame
    {
        public int id { get; set; }
        public int gameID { get; set; }
        public DateTime plannedTime { get; set; }
    }
}
