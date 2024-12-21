using Microsoft.Extensions.DependencyInjection;
using System;

public interface INotificationSender
{
    void Send(string message);
}

public class EmailNotificationSender : INotificationSender
{
    public void Send(string message)
    {
        Console.WriteLine($"Email sent: {message}");
    }
}

public class SmsNotificationSender : INotificationSender
{
    public void Send(string message)
    {
        Console.WriteLine($"SMS sent: {message}");
    }
}

public class TelegramNotificationSender : INotificationSender
{
    public void Send(string message)
    {
        Console.WriteLine($"Telegram message sent: {message}");
    }
}

public class NotificationService
{
    private readonly INotificationSender _notificationSender;

    // Конструктор принимает зависимость INotificationSender
    public NotificationService(INotificationSender notificationSender)
    {
        _notificationSender = notificationSender;
    }

    // Метод для отправки уведомления
    public void Notify(string message)
    {
        _notificationSender.Send(message);
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Настройка DI-контейнера
        var serviceProvider = new ServiceCollection()
            .AddTransient<INotificationSender, EmailNotificationSender>()  // Внедрение EmailSender
            .AddTransient<NotificationService>()  // Внедрение NotificationService
            .BuildServiceProvider();

        // Получаем сервис через контейнер
        var notificationService = serviceProvider.GetService<NotificationService>();

        Console.WriteLine("Choose notification method: 1. Email, 2. SMS, 3. Telegram");
        int choice = int.Parse(Console.ReadLine() ?? "1");

        // Меняем реализацию NotificationService на основе выбора
        INotificationSender sender = choice switch
        {
            1 => new EmailNotificationSender(),
            2 => new SmsNotificationSender(),
            3 => new TelegramNotificationSender(),
            _ => throw new ArgumentException("Invalid choice")
        };

        notificationService = new NotificationService(sender);

        Console.WriteLine("Enter your message:");
        string message = Console.ReadLine();

        // Отправка уведомления через выбранный сервис
        notificationService.Notify(message);
    }
}
