using System;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Gamble_On.Services;
using Gamble_On.ViewModels;
using Gamble_On.Views;

namespace Gamble_On
{
    public static class MauiProgram
    {
        public static string baseUrl = "https://localhost:7138/api/User/";
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();


            // Configure Services
            ConfigureServices(builder.Services);

            builder
                .UseMauiApp<App>()
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

            // Define the retry policy
            var retryPolicy = GetRetryPolicy();

            // HttpClientFactory for User service
            services.AddHttpClient<IUserService, UserServiceOld>(client =>
            {
                //Here we need to set our own url instead of the one for my local api
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(30); // Set the timeout for the HttpClient
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .AddPolicyHandler(retryPolicy);
            // Register services
            //services.AddSingleton<IUserService, UserServiceOld>();

            // add viewmodels here
            services.AddTransient<UserLoginViewModel>();
            services.AddTransient<MainDashboardViewModel>();

            //add pages
            services.AddTransient<MainPage>();
            services.AddTransient<Dashboard>();
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