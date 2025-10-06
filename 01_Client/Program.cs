using System;
using System.Net.Sockets;
using System.Text;

class Client
{
    static void Main()
    {
        // створюємо об’єкт TcpClient і підключаємося до сервера на localhost:5000
        //"127.0.0.1" – IP - адреса сервера. Тут вона локальна(тобто сервер працює на тій самій машині).
        // 5000 – порт, на якому сервер слухає підключення.
        TcpClient client = new TcpClient("127.0.0.1", 5000); 
        
        NetworkStream stream = client.GetStream(); //Отримуємо потік даних з сокета, через який будемо надсилати і отримувати повідомлення.
        //  stream працює як “труба”, через яку передаються байти.

        string message = "Привіт сервере!"; //це повідомлення, яке ми хочемо відправити серверу
        byte[] data = Encoding.UTF8.GetBytes(message); //конвертуємо рядок у масив байтів у кодуванні UTF-8
        stream.Write(data, 0, data.Length); //відправляємо масив байтів серверу через потік
        Console.WriteLine($"Надіслано серверу {DateTime.Now}: {message}"); //виводимо на консоль те, що відправили серверу

        byte[] buffer = new byte[1024]; //створюємо буфер для зберігання отриманих від сервера даних
        int bytesRead = stream.Read(buffer, 0, buffer.Length); //читаємо дані з потоку у буфер
        string reply = Encoding.UTF8.GetString(buffer, 0, bytesRead); //конвертуємо отримані байти у рядок
        Console.WriteLine($"Сервер відповів {DateTime.Now}: {reply}");//виводимо відповідь сервера на консоль

        client.Close();
    }
}
