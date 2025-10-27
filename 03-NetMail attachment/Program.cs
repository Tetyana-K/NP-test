using System.Net;
using System.Net.Mail;

class Program
{
    static void Main()
    {
        try
        {
            MailMessage mail = new MailMessage();
            //mail.From = new MailAddress("your_email@gmail.com");
            //mail.To.Add("recipient@example.com");
            mail.From = new MailAddress("your@gmail.com");
            mail.To.Add("receiver@gmail.com");
            mail.Subject = "Знову тест з вкладенням (NetMail - SMTP)";
            // mail.Body = "Привіт! Це лист із вкладеним файлом."; // буде звичайний текстовий лист
            
            // HTML тіло
            mail.IsBodyHtml = true; // Вказуємо, що тіло листа містить HTML
            mail.Body =
                @"<h2 style='color:navy;'>Привіт!</h2>
                  <p>Це <b>HTML-лист</b>, відправлений з <i>C#</i>.</p>
                  <table border='1' cellpadding='5'>
                    <tr><th>#</th><th>Товар</th><th>Ціна</th></tr>
                    <tr><td>1</td><td>Монітор</td><td>8000 грн</td></tr>
                    <tr><td>2</td><td>Мишка</td><td>600 грн</td></tr>
                  </table>";

            // Додаємо вкладення (шлях до файлу)
            string filePath = @"C:\Users\Ryzen\Pictures\For Web\tech girl AI.png";
            Attachment attachment = new Attachment(filePath);
            mail.Attachments.Add(attachment); // Додаємо вкладення до листа
            // можна додати кілька вкладень

            // SMTP налаштування
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("your@gmail.com", GenerateAppPassword());
            smtp.EnableSsl = true;

            smtp.Send(mail);
            Console.WriteLine("Лист із вкладенням надіслано!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Помилка: " + ex.Message);
        }
    }


    static string GenerateAppPassword()
    {
        // This is a placeholder function to represent the generation of an app password.
        return "your app password";
    }
}
