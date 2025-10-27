using _06_Use_config;
using System.Net;
using System.Net.Mail;

string from = "your_email@gmail.com"; // ваша пошта
string password = "your_app_password"; // ваш пароль додатка
string to = "recepient@gmail.com"; // пошта отримувача
string subject = "Тестовий лист  (SMTP)";
string body = "Привіт! Це тестове повідомлення з C# (SMTP) з використанням конфігурації.";
SendEmail(from, password, to, subject, body);



void SendEmail(string from, string password, string to, string subject, string body)
{
    using var smtp = new SmtpClient
    {
        Host = SmtpSettings.Server,
        Port = SmtpSettings.Port,
        EnableSsl = SmtpSettings.EnableSsl,
        Credentials = new NetworkCredential(from, password)
    };

    var message = new MailMessage(from, to, subject, body);
    smtp.Send(message);
}
