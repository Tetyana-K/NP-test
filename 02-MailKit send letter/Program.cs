// Використання MailKit для відправки електронної пошти
//dotnet add package MailKit

using MailKit.Net.Smtp;
using MimeKit;

class Program
{
    static void Main()
    {
        var message = new MimeMessage();// Створення нового повідомлення
        message.From.Add(new MailboxAddress("Тестовий відправник", "your@gmail.com"));
        message.To.Add(new MailboxAddress("Тестовий отримувач", "your@gmail.com"));
        message.Subject = "Тестовий лист (MailKit)";

        // Тіло листа
        // Використовуємо TextPart для створення текстової частини листа
        message.Body = new TextPart("plain") // plain - простий текст
        {
            Text = "Привіт!\nЦе тестове повідомлення з C# (MailKIt - MimeKit)"
        };

        using (var client = new SmtpClient())
        {
            client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            client.Authenticate("your@gmail.com", GenerateAppPassword());
            client.Send(message);
            client.Disconnect(true);
            Console.WriteLine("Лист відправлено!");
        }
    }

    static string GenerateAppPassword()
    {
        // This is a placeholder function to represent the generation of an app password.
        return "your app password";
    }
}
