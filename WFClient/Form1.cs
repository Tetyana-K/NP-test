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
                var serverIp = tbServer.Text; // IP �������
                int port = int.Parse(tbPort.Text);

                TcpClient client = new TcpClient();
                AppendLog($"ϳ���������� �� ������� {serverIp}:{port} ...");
                await client.ConnectAsync(serverIp, port); // ���������� ���������� �� �������

                stream = client.GetStream(); // �������� ���� ��� ����� ������

                // ��������� ����������� �������
                string message = $"����� �� {name}! {tbMessage.Text}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(data, 0, data.Length); //���������� ������ ��� � ����
                AppendLog($"[{DateTime.Now}] ��������: {message}\n");

                // ������ ������� ����������
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string reply = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                AppendLog($"[{DateTime.Now}] ³������ �������: {reply}\n");
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
            AppendLog("³�������� �� �������.\n");
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
