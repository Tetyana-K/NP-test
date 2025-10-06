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
            listener.Start(); // ��������� ������
            AppendLog("������ �����...");

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync(); // ���������� ������� ���������� �볺���
                _ = Task.Run(() => HandleClientAsync(client)); // ���������� �볺��� � �������� ������� (����������), ��� _ ������, �� �� �� ��������� ��������������� ��������� Task
            }
        }

        // ����� ��� ������� ����������� �볺���
        async Task HandleClientAsync(TcpClient client)
        {
            IPEndPoint? remote = client.Client.RemoteEndPoint as IPEndPoint; // �������� IP-������ � ���� �볺���
           AppendLog( $"�볺�� {remote?.Address}:{remote?.Port} ����������");

            using NetworkStream stream = client.GetStream(); // �������� ���� ��� ����� ������
            byte[] buffer = new byte[1024]; // ����� ��� ��������� ��������� �����
                                            //int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length); // ���������� ������ ��� � ������
                                            //string message = Encoding.UTF8.GetString(buffer, 0, bytesRead); // ���������� ����� ���������� ����� � ����� � �������� UTF-8
                                            //Console.WriteLine($"³� {remote?.Address}: {message}"); // �������� �������� �����������

            //// ������ �� ����������� ������� �볺���
            //string reply = "�����, ����������� �볺��!";
            //byte[] replyBytes = Encoding.UTF8.GetBytes(reply); 
            //await stream.WriteAsync(replyBytes, 0, replyBytes.Length);


            try
            {
                while (true)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        // �볺�� ������ �'�������
                        AppendLog($"�볺�� {remote?.Address}:{remote?.Port} ����������\n");
                        break;
                    }

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    AppendLog($"³� {remote?.Address}: {message}\n");

                    string reply = "�����, ����������� �볺���!";
                    byte[] replyBytes = Encoding.UTF8.GetBytes(reply);
                    await stream.WriteAsync(replyBytes, 0, replyBytes.Length);
                }
            }
            catch (Exception ex)
            {
                AppendLog($"������� � �볺���� {remote?.Address}:{remote?.Port}: {ex.Message}\n");
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

