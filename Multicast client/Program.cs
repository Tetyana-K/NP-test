using System.Net;
using System.Net.Sockets;
using System.Text;

/*
 * Кожен клієнт може мати свій сокет, але всі вони підписані на той самий порт і multicast-групу.
Для Multicast це стандартний підхід — багато отримувачів на одному порту.
*/
class MulticastClient
{
    static void Main()
    {
        string multicastIP = "224.1.1.1"; // Multicast-адреса групи
        int port = 5000;                  // Порт, на якому слухаємо повідомлення

        // Створюємо сокет вручну (для кращого контролю над параметрами)
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        // Дозволяємо повторне використання адреси (ReuseAddress)
        // Це необхідно, щоб кілька клієнтів могли слухати один і той же порт одночасно
        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

        // Прив'язуємо сокет до порту (IPAddress.Any = будь-який локальний інтерфейс)
        socket.Bind(new IPEndPoint(IPAddress.Any, port));

        // Створюємо UdpClient поверх цього сокета
        UdpClient udpClient = new UdpClient { Client = socket };

        // Приєднуємося до multicast групи, на яку будуть надсилатися повідомлення
        // Тепер сокет отримуватиме всі пакети, надіслані на цю групу і порт
        udpClient.JoinMulticastGroup(IPAddress.Parse(multicastIP));

        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, port);
        Console.WriteLine("Multicast клієнт запущено. Очікування повідомлень...");

        while (true)
        {
            // Отримуємо повідомлення з групи
            byte[] data = udpClient.Receive(ref remoteEP);
            string message = Encoding.UTF8.GetString(data);

            // Виводимо отримане повідомлення та адресу відправника
            Console.WriteLine($"[Клієнт] Отримано від {remoteEP}: {message}");
        }
    }
}

//class MulticastClient
//{
//    static void Main()
//    {
//        string multicastIP = "224.1.1.1"; // Та сама Multicast-адреса
//        int port = 5000;

//        UdpClient udpClient = new UdpClient(port);

//        IPAddress multicastAddress = IPAddress.Parse(multicastIP);

//        // Приєднання до multicast-групи
//        udpClient.JoinMulticastGroup(multicastAddress);

//        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, port);
//        Console.WriteLine("Multicast клієнт запущено. Очікування повідомлень...");

//        while (true)
//        {
//            byte[] data = udpClient.Receive(ref remoteEP);
//            string message = Encoding.UTF8.GetString(data);
//            Console.WriteLine($"[Клієнт] Отримано від {remoteEP}: {message}");
//        }
//    }
//}
