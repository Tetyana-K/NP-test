using System.Net;
using System.Net.Sockets;
using System.Text;

class BroadcastClient
{
    static void Main()
    {
        using (UdpClient udpClient = new UdpClient()) // UdpClient() - не вказуємо порт, хочемо налаштувати параметри сокету вручну
        {
            // Створюємо локальну кінцеву точку (endpoint),
            // на якій клієнт буде приймати повідомлення.
            // IPAddress.Any — означає, що приймаємо пакети з будь-якої мережевої карти.
            // 8888 — порт, на якому слухаємо broadcast-повідомлення.
            IPEndPoint localEP = new IPEndPoint(IPAddress.Any, 8888);

            // Дозволяємо кільком сокетам(тобто клієнтам) слухати один порт
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpClient.Client.Bind(localEP); // прив`язуємо клієнта до локального порту localEP(8888)

            // Створюємо endpoint для зберігання інформації про відправника (хто надіслав пакет)
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("Очікування широкомовних повідомлень...");

            while (true)
            {
                byte[] data = udpClient.Receive(ref remoteEP);
                string message = Encoding.UTF8.GetString(data);
                Console.WriteLine($"Отримано від {remoteEP}: {message}");
            }
        }
    }
}
