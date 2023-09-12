using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamble_On.Models
{
    public class Character
    {
        public int id { get; set; }
        public int gameId  { get; set; }
        public float odds { get; set; }
        public string name { get; set; }
    }
}
