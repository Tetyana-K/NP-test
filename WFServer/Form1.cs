using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WFServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_LoadAsync(object sender, EventArgs e)
        {
            
            TcpListener listener = new TcpListener(IPAddress.Any, 5001);
            listener.Start(); // запускаємо сервер
            AppendLog("Сервер слухає...");

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync(); // асинхронно очікуємо підключення клієнта
                _ = Task.Run(() => HandleClientAsync(client)); // обробляємо клієнта в окремому завданні (паралельно), тут _ означає, що ми не збираємося використовувати результат Task
            }
        }

        // метод для обробки підключеного клієнта
        async Task HandleClientAsync(TcpClient client)
        {
            IPEndPoint? remote = client.Client.RemoteEndPoint as IPEndPoint; // отримуємо IP-адресу і порт клієнта
           AppendLog( $"Клієнт {remote?.Address}:{remote?.Port} підключився");

            using NetworkStream stream = client.GetStream(); // отримуємо потік для обміну даними
            byte[] buffer = new byte[1024]; // буфер для зберігання отриманих даних
                                            //int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length); // асинхронно читаємо дані з потоку
                                            //string message = Encoding.UTF8.GetString(buffer, 0, bytesRead); // конвертуємо масив прочитаних байтів в рядок у кодуванні UTF-8
                                            //Console.WriteLine($"Від {remote?.Address}: {message}"); // виводимо отримане повідомлення

            //// готуємо та відправляємо відповідь клієнту
            //string reply = "Привіт, асинхронний клієнт!";
            //byte[] replyBytes = Encoding.UTF8.GetBytes(reply); 
            //await stream.WriteAsync(replyBytes, 0, replyBytes.Length);


            try
            {
                while (true)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        // клієнт закрив з'єднання
                        AppendLog($"Клієнт {remote?.Address}:{remote?.Port} відключився\n");
                        break;
                    }

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    AppendLog($"Від {remote?.Address}: {message}\n");

                    string reply = "Привіт, асинхронний клієнте!";
                    byte[] replyBytes = Encoding.UTF8.GetBytes(reply);
                    await stream.WriteAsync(replyBytes, 0, replyBytes.Length);
                }
            }
            catch (Exception ex)
            {
                AppendLog($"Помилка з клієнтом {remote?.Address}:{remote?.Port}: {ex.Message}\n");
            }
            finally
            {
                client.Close();
            }
        }
        private void AppendLog(string text)
        {
            if (tbLog.InvokeRequired)
            {
                tbLog.Invoke(new Action(() => tbLog.AppendText($"{text}\r\n")));
            }
            else
            {
                tbLog.AppendText($"{text}{Environment.NewLine}");

            }
        }
    }
}

