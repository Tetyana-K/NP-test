using System.Net;
using System.Net.Sockets;
using System.Text;

class UdpClientApp
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.Write("Введіть ім'я: ");
        string name = Console.ReadLine() ?? "Noname client";
        
        // Створюємо UDP-клієнт
        UdpClient client = new UdpClient();
        try
        {
            // Вказуємо адресу та порт сервера (тут — localhost:5000)
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Loopback, 5000);
           // IPEndPoint serverEP2 = new IPEndPoint(IPAddress.Loopback, 5001);

            // Підключення до сервера логічне (не фізичне, не встановлюється з'єднання, тільки запам'ятовується serverEP )
            // Після цього UdpClient дозволяє надсилати пакети тільки на цей сервер
           //  client.Connect(serverEP); // надалі у методах Send() можна не вказувати адресу сервера (serverEP)

            // Повідомлення, яке відправляємо серверу
            string message = $"[{name}] Привіт сервер!";

            // Кодуємо повідомлення у байти
            byte[] data = Encoding.UTF8.GetBytes(message);

            // Надсилаємо дані серверу
            client.Send(data, data.Length, serverEP); 
           // client.Send(data, data.Length, serverEP2); 
            //client.Send(data, data.Length);// так можна, якщо був Connect()

            Console.WriteLine("[Клієнт] Повідомлення відправлено.");

            // Очікуємо відповідь від сервера
            var receivedData = client.Receive(ref serverEP);
            //IPEndPoint? ss = null;
            //var receivedData = client.Receive(ref ss); // так можна якщо було використано Connect()

            // Декодуємо відповідь з байтів у текст
            string response = Encoding.UTF8.GetString(receivedData);

            // Виводимо відповідь на екран
            Console.WriteLine($"[Сервер] відповів {response}");

            // Закриваємо клієнт
            client.Close();
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
    }
}
