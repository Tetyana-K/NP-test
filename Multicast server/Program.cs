using System.Net;
using System.Net.Sockets;
using System.Text;

class MulticastServer
{
    static void Main()
    {
        string multicastIP = "224.1.1.1"; // Multicast-адреса
        int port = 5000;

        UdpClient udpClient = new UdpClient();
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(multicastIP), port);

        Console.WriteLine("Multicast сервер запущено. Відправка повідомлень кожні 2 секунди...");

        int counter = 1;
        while (true)
        {
            string message = $"Повідомлення #{counter}";
            byte[] data = Encoding.UTF8.GetBytes(message);
            udpClient.Send(data, data.Length, endPoint);

            Console.WriteLine($"[Сервер] Відправлено: {message}");
            counter++;
            Thread.Sleep(2000); // відправка кожні 2 секунди
        }
    }
}
