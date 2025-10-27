using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

class Program
{
    static void Main()
    {
        try
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("your@gmail.com");
            mail.To.Add("to@gmail.com");
            mail.Subject = "Новий тестовий лист";
            mail.Body = "Привіт! Це нове тестове повідомлення з C# (SMTP).";

            // Налаштування SMTP сервера
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587); // Порт 587 використовується для TLS
            // smtp.gmail.com - це SMTP сервер Gmail.
            // outlook.office365.com - це SMTP сервер Outlook.
            // yahoo smtp.mail.yahoo.com - це SMTP сервер Yahoo Mail.
            // ukr.net.ua - це SMTP сервер Ukr.net.

            // Credentials - це властивість, яка встановлює облікові дані для автентифікації на SMTP сервері.
            smtp.Credentials = new NetworkCredential("your@gmail.com", GenerateAppPassword());
            
            // EnableSsl - це властивість, яка вказує, чи слід використовувати захищене з'єднання SSL/TLS для відправки листа.
            smtp.EnableSsl = true;

            smtp.Send(mail); // Відправка листа
            Console.WriteLine("Лист надіслано!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Помилка: " + ex.Message);
        }
    }
    static string GenerateAppPassword()
    {
        var config = new ConfigurationBuilder()
          .AddUserSecrets<Program>()
          .Build();
        
        return config["appPassword"];
    }
}
// для зберігання секретів використовуємо User Secrets
// у контестному меню проекту вибираємо "Manage User Secrets" і додаємо туди:
// {
//   "appPassword": "ваш app пароль"
// }