using System.Net.Sockets;
using System.Text;

namespace WFClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.tbInfo.Multiline = true;
            tbInfo.ScrollBars = ScrollBars.Both;
        }
        private TcpClient client;
        private NetworkStream stream;
        private async void button1_Click(object sender, EventArgs e)
        {

            try
            {
                string name = tbName.Text.Trim();
                var serverIp = tbServer.Text; // IP сервера
                int port = int.Parse(tbPort.Text);

                TcpClient client = new TcpClient();
                AppendLog($"Підключаємось до сервера {serverIp}:{port} ...");
                await client.ConnectAsync(serverIp, port); // асинхронне підключення до сервера

                stream = client.GetStream(); // отримуємо потік для обміну даними

                // Надсилаємо повідомлення серверу
                string message = $"Привіт від {name}! {tbMessage.Text}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(data, 0, data.Length); //асинхронно пишемо дані в потік
                AppendLog($"[{DateTime.Now}] Надіслано: {message}\n");

                // Читаємо відповідь асинхронно
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string reply = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                AppendLog($"[{DateTime.Now}] Відповідь сервера: {reply}\n");
            }
            catch (Exception ex)
            {
                AppendLog($"Error: {ex.Message}\n");
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e) // diconnect
        {
            //if (stream != null)
            {
                stream?.Close();
            }
            //if (client != null)
            {
                client?.Close();
            }
            AppendLog("Відключено від сервера.\n");
        }
        private void AppendLog(string text)
        {
            tbInfo.AppendText($"{text}\r\n");
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
