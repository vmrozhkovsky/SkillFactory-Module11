using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using UtilityBot.Configurations;
using UtilityBot.Controllers;
using UtilityBot.Services;

namespace UtilityBot
{
    public class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services))
                .UseConsoleLifetime()
                .Build();
 
            Console.WriteLine("Сервис запущен.");
            await host.RunAsync();
            Console.WriteLine("Сервис остановлен.");
        }
 
        static void ConfigureServices(IServiceCollection services)
        {
            AppSettings appSettings = BuildAppSettings();
            services.AddTransient<DefaultMessageController>();
            services.AddTransient<TextMessageController>();
            services.AddTransient<InlineKeyboardController>();
            services.AddSingleton(appSettings);
            services.AddSingleton<IStorage, MemoryStorage>();
            services.AddSingleton<ITextHandler, TextHandler>();
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
            services.AddHostedService<Bot>();
        }
        static AppSettings BuildAppSettings()
        {
            return new AppSettings()
            {
                BotToken = "6224444090:AAEdZf8dFQtEnVr53esbl9LymwxkO99RwyE",
            };
        }
    }
}