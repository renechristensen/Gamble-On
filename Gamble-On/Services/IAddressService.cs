
using System.Collections.Generic;
using System.Threading.Tasks;
using Gamble_On.Models;

namespace Gamble_On.Services
{
    // This interface exists for testing purposes
    public interface IAddressService
    {
        Task<IEnumerable<Address>> GetAllAddressesAsync();
    }
}
