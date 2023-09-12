using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamble_On.Models
{
    public class Game
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string desc { get; set; }
        public int gameTypeId { get; set; }

    }
}
