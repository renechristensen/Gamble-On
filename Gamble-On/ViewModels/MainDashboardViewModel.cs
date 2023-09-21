using Gamble_On.Services;
using System.Collections.ObjectModel;
using Gamble_On.Models;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace Gamble_On.ViewModels
{
    public class MainDashboardViewModel : ViewModelBase
    {
        /*
         * This exists for testing with the address service. Its used while working with authentication
         * 
        private readonly IAddressService _addressService;
        public ObservableCollection<Address> Addresses { get; set; }

        public MainDashboardViewModel(IAddressService addressService)
        {
            _addressService = addressService;
            Addresses = new ObservableCollection<Address>();

            // Use the same pattern as in WalletViewModel
            LoadAddresses();
        }

        private async void LoadAddresses()
        {
            try
            {
                var addresses = await _addressService.GetAllAddressesAsync();
                if (addresses != null)
                {
                    foreach (var address in addresses)
                    {
                        Addresses.Add(address);
                    }
                }
                else
                {
                    // Handle situations where addresses aren't available or something went wrong.
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions.
            }
        }
        */
    }
}


