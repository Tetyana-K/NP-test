using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class SimpleClient
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        try
        {
            // 1️ Створюємо кінцеву точку — IP і порт сервера
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, 5000);

            // 2️ Створюємо TCP-сокет
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // 3️ Підключаємося до сервера
            client.Connect(endPoint);
            Console.WriteLine("Підключено до сервера!");

            // 4️ Готуємо повідомлення
            string message = "Привіт, сервер!";
            byte[] data = Encoding.UTF8.GetBytes(message); // перетворюэмо дані (рядок) у послідовність байтів

            // 5️ Надсилаємо дані
            client.Send(data);
            Console.WriteLine("Повідомлення надіслано: " + message);

            // 6️ Закриваємо з’єднання
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }
        catch (SocketException ex){
        
            Console.WriteLine("Помилка (SocketException): " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Помилка: " + ex.Message);
        }
    }
}
