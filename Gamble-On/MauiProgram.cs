using System;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Gamble_On.Services;
using Gamble_On.ViewModels;
using Gamble_On.Views;
using Gamble_On.Views.Modals;
using CommunityToolkit.Maui;

namespace Gamble_On
{
    public static class MauiProgram
    {
        public static string baseUrl = "https://deep-wealthy-roughy.ngrok-free.app";
        // for test api
        //public static string baseUrl = "https://localhost:7138/api/User/";
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            // Configure Services
            ConfigureServices(builder.Services);

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            // set static services 
            //ViewModelLocator.StaticServiceProvider = serviceCollection.BuildServiceProvider();
            return builder.Build();
        }
        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationService, AuthorizationService>();
            // add httpclients for each service
            RegisterHttpClients(services);
            // add viewmodels
            RegisterViewModels(services);
            //add pages
            RegisterPages(services);
        }
        private static void RegisterHttpClients(IServiceCollection services)
        {
            var retryPolicy = GetRetryPolicy();

            var httpClientConfig = new Action<HttpClient>(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            services.AddHttpClient<IUserService, UserService>(httpClientConfig)
                    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                    .AddPolicyHandler(retryPolicy);

            services.AddHttpClient<IWalletService, WalletService>(httpClientConfig)
                    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                    .AddPolicyHandler(retryPolicy);

            services.AddHttpClient<IAddressService, AddressService>(httpClientConfig)
                    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                    .AddPolicyHandler(retryPolicy);

            services.AddHttpClient<IGameService, GameService>(httpClientConfig)
                    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                    .AddPolicyHandler(retryPolicy);

            services.AddHttpClient<IBettingService, BettingService>(httpClientConfig)
                    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                    .AddPolicyHandler(retryPolicy);
        }
        private static void RegisterViewModels(IServiceCollection services)
        {
            var viewModels = new List<Type>
            {
                typeof(UserLoginViewModel),
                typeof(MainDashboardViewModel),
                typeof(UserRegisterViewModel),
                typeof(ProfilePageViewModel),
                typeof(WalletViewModel),
                typeof(DepositPopupViewModel),
                typeof(WithdrawPopupViewModel),
                typeof(WalletBettingHistoryViewModel),
                typeof(WalletTransactionHistoryViewModel),
                typeof(CurrentBettingsForGameViewModel),
                typeof(BettingViewModel)
            };

            viewModels.ForEach(viewModelType => services.AddTransient(viewModelType));
        }

        private static void RegisterPages(IServiceCollection services)
        {
            var pages = new List<Type>
            {
                typeof(LoginPage),
                typeof(Dashboard),
                typeof(RegisterPage),
                typeof(ProfilePage),
                typeof(WalletPage),
                typeof(DepositPopupPage),
                typeof(WithdrawPopupPage),
                typeof(WalletBettingHistory),
                typeof(WalletTransactionHistory),
                typeof(CurrentBettingsForGamePage),
                typeof(BettingPage)
            };

            pages.ForEach(pageType => services.AddTransient(pageType));
        }




        // add in polly to help configure http client factory
        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
        
    }
}