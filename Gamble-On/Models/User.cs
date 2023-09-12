using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamble_On.Models
{
    public class User
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string username { get; set; } // added this guys :D
        public string password { get; set; }
        public int walletId { get; set; }
        public string email { get; set; }
        public int phoneNumber { get; set; }
        public bool active { get; set; }
        public int userTypeId { get; set; }
        public int addressId { get; set; }
        public string Token { get; set; }
    }
}
