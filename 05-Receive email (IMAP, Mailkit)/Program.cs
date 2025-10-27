using MailKit.Net.Imap;
using MailKit;
using MailKit.Security;
using MimeKit;

class Program
{
    static async Task Main()
    {
        string email = "your_email@gmail.com";          // Ваш Gmail
        string appPassword = "your_app_password";       // Пароль додатка з Google
        
        string imapServer = "imap.gmail.com"; // IMAP сервер Gmail
        int imapPort = 993; // Порт IMAP з SSL

        using (var client = new ImapClient())
        {
            try
            {
                Console.WriteLine("Підключення до Gmail...");
                await client.ConnectAsync(imapServer, imapPort, SecureSocketOptions.SslOnConnect);
                Console.WriteLine("Підключено!");

                Console.WriteLine("Аутентифікація...");
                await client.AuthenticateAsync(email, appPassword);
                Console.WriteLine("Успішний вхід!");

                // Виводимо список папок
                Console.WriteLine("\nПапки у Gmail:");
                foreach (var folder in client.GetFolders(client.PersonalNamespaces[0]))
                {
                    Console.WriteLine(folder.FullName);
                }
                Console.ReadKey();

                // Відкриваємо вхідні листи
                var inbox = client.Inbox;

                await inbox.OpenAsync(FolderAccess.ReadOnly);
                Console.WriteLine($"Назва папки: {inbox.FullName}");
                Console.WriteLine($"Кількість листів у Вхідних (Inbox): {inbox.Count}");


                // Отримаємо кілька останніх листів

                int fetchCount = Math.Min(2, inbox.Count);
                for (int i = inbox.Count - fetchCount; i < inbox.Count; i++)
                {
                    var message = await inbox.GetMessageAsync(i);
                    Console.WriteLine($"\nВід: {message.From}");
                    Console.WriteLine($"Тема: {message.Subject}");
                    Console.WriteLine($"Дата: {message.Date}");
                    Console.WriteLine($"--- Тіло листа ---");
                    //Console.WriteLine(message.TextBody ?? "(немає тексту)"); // хочемо бачити чистий текст
                    // Якщо лист у HTML — показати як текст
                    if (!string.IsNullOrEmpty(message.HtmlBody))
                        Console.WriteLine(message.HtmlBody); // якщо хочемо бачити html
                    else if (!string.IsNullOrEmpty(message.TextBody))
                        Console.WriteLine(message.TextBody);
                    else
                        Console.WriteLine("(Без тексту)");
                    await CheckAttachment(message);
                    Console.WriteLine(new string('-', 50));
                }
                //var messages = inbox.Fetch(inbox.Count - fetchCount, -1, MessageSummaryItems.Envelope);

                //Console.WriteLine("\nОстанні листи у Inbox:\n");

                //foreach (var msg in messages.Reverse()) // від новіших до старіших
                //{
                //    Console.WriteLine($"{msg.Envelope.Date?.ToLocalTime():dd.MM.yyyy HH:mm}");
                //    Console.WriteLine($"Від: {msg.Envelope.From}");
                //    Console.WriteLine($"Тема: {msg.Envelope.Subject}");
                //    Console.WriteLine($"--- Тіло листа ---");
                //    Console.WriteLine(new string('-', 50));
                //}

                await client.DisconnectAsync(true);
                Console.WriteLine("\nЗ'єднання закрито.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }
    static async Task CheckAttachment(MimeMessage message)
    {
        if (!message.Attachments.Any()) // 
        {
            Console.WriteLine("Вкладень немає.");
            return;
        }

        Console.WriteLine($"Вкладень: {message.Attachments.Count()}");
        string savePath = "../../../attachments"; // папка для збереження файлів, що будемо завантажувати
        foreach (var attachment in message.Attachments)
        {
            // Якщо це звичайний файл (PDF, DOCX, JPG)
            if (attachment is MimePart part)
            {
                var fileName = part.FileName ?? "unnamed_file";
                var filePath = Path.Combine(savePath, fileName); //Path     c:\Users\     a.txt

                using (var stream = File.Create(filePath))
                    await part.Content.DecodeToAsync(stream);

                Console.WriteLine($"Збережено: {fileName}");
            }
        }
    }
    static string GenerateAppPassword()
    {
        // This is a placeholder function to represent the generation of an app password.
        return "your app password";
    }
}
