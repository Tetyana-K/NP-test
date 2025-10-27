
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

class Program
{
    static void Main()
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Відправник", "your@gmail.com"));
        message.To.Add(new MailboxAddress("Отримувач", "your@gmail.com"));
        message.Subject = "HTML-лист із вкладенням через MailKit";

        var builder = new BodyBuilder();

        // HTML-варіант тіла
        builder.HtmlBody =
            @"<h2 style='color:green;'>Вітаю!</h2>
              <p>Це HTML-лист, створений через <b>MailKit</b>.</p>
              <ul>
                <li>Підтримує HTML</li>
                <li>Має вкладення</li>
                <li>Можна додавати зображення</li>
              </ul>";

        // Альтернативний текстовий варіант (опціонально)
        builder.TextBody = "Вітаю! Це HTML-лист із вкладенням.";

        // Додаємо файл-вкладення
        builder.Attachments.Add(@"C:\Users\Ryzen\Pictures\Cities\austria.jpg");

        message.Body = builder.ToMessageBody();

        using (var client = new SmtpClient())
        {
            client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            client.Authenticate("your@gmail.com", "your app password");
            client.Send(message);
            client.Disconnect(true);
        }

        Console.WriteLine("HTML-лист із вкладенням успішно відправлено!");
    }
}


