using StackExchange.Redis;

namespace Valuator;

public class Program
{
    //настраиваем точку входа
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //регистрируем сервисы RazorPages и Redis
        builder.Services.AddRazorPages();
        //создаём singleton подключение
        builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var configuration = ConfigurationOptions.Parse("localhost:6379");
            return ConnectionMultiplexer.Connect(configuration);
        });
        //Билдим приложение и теперь все сервисы доступны
        var app = builder.Build();
        //Настраиваем конвейер обработки HTTP запросов
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            //включаем HTTPS
            app.UseHsts();
        }
        //Включаем статические файлы
        app.UseStaticFiles();
        //Подключаем маршрутизацию
        app.UseRouting();
        //ограничение доступа на основе прав доступа
        app.UseAuthorization();
        //настраиваем обработку запросов к с страницам
        app.MapRazorPages();
        //запускаем приложение
        app.Run();
    }
}