using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class BroadcastServer
{
    static void Main()
    {
        using (UdpClient udpServer = new UdpClient()) // створиться UDP-сокет без прив`язки до певного порту
        {
            // Дозволяємо цьому сокету надсилати широкомовні повідомлення
            // Без цього рядка система заблокує відправку на адресу 255.255.255.255
            udpServer.EnableBroadcast = true;

            // Створюємо кінцеву точку (адресу й порт), куди будемо надсилати повідомлення
            // IPAddress.Broadcast = 255.255.255.255 — означає "усім у локальній мережі"
            // 8888 — порт, який слухають усі клієнти
            IPEndPoint broadcastEndPoint = new IPEndPoint(IPAddress.Broadcast, 8888);

            while (true)
            {
                string message = $"Time: {DateTime.Now}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                udpServer.Send(data, data.Length, broadcastEndPoint);  // відправляємо UDP-пакет усім клієнтам, які слухають порт 8888
                Console.WriteLine($"Відправлено: {message}");
                Thread.Sleep(2000); // пауза між повідомленнями 2 сек
            }
        }
    }
}
