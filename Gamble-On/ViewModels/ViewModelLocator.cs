//This is not in use guys
namespace Gamble_On.ViewModels {
    using Gamble_On.ViewModels;
    public class ViewModelLocator
    {
        private readonly IServiceProvider _serviceProvider;

        public ViewModelLocator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public UserLoginViewModel UserLoginViewModel => _serviceProvider.GetRequiredService<UserLoginViewModel>();
    }

}