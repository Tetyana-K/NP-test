using System.Net;

namespace ShowImages_WF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string[] imageUrls = [
            "https://www.gutenberg.org/cache/epub/43/pg43.cover.medium.jpg",
            "https://miro.medium.com/0*APFT6x8btsdvYIVX.jpg",
            "https://www.cdnetworks.com/wos/static-resource/9e836fbe17c141689830b64157c0ba9d/QUIC-PICTURE-05-1024x560.jpg?t=1740733716166"
        ];
        private void Form1_Load(object sender, EventArgs e)
        {
            //listBox1.DataSource = imageUrls;
            listBox1.Items.AddRange(imageUrls);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom; // ������������� � ����������� ���������
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string imageUrl = listBox1.SelectedItem.ToString(); // �������� ������� URL-������ ����������
                using WebClient client = new WebClient(); // ��������� ��������� WebClient ��� ������������ ����������
                byte[] data = client.DownloadData(imageUrl); // ����������� ���������� �� ����� �����
                using MemoryStream ms = new MemoryStream(data);
                pictureBox1.Image = Image.FromStream(ms); //
                
                //pictureBox1.ImageLocation = imageUrl; // ����� �������� ������ URL �������
            }
        }
    }
}
