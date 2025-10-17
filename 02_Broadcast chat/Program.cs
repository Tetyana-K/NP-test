using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class BroadcastChat
{
    static int port = 8888;

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        using (UdpClient udpClient = new UdpClient())
        {
            // Дозволяємо кільком програмам використовувати один і той самий порт
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, port));

            Console.Write("Введіть своє ім'я: ");
            string name = Console.ReadLine();

            // Запускаємо окремий потік для прийому повідомлень
            Task.Run(() => ReceiveMessages(udpClient));

            // Основний цикл відправки повідомлень
            IPEndPoint broadcastEP = new IPEndPoint(IPAddress.Broadcast, port);
            while (true)
            {
                string? message = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(message))
                    continue;

                string fullMessage = $"{name}: {message}";
                byte[] data = Encoding.UTF8.GetBytes(fullMessage);
                udpClient.Send(data, data.Length, broadcastEP);
            }
        }
    }

    static void ReceiveMessages(UdpClient udpClient)
    {
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, port);
        while (true)
        {
            try
            {
                byte[] data = udpClient.Receive(ref remoteEP);
                string message = Encoding.UTF8.GetString(data);
                Console.WriteLine($"\n{message}");
                Console.Write("> "); // повертаємо курсор для введення
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }
}
