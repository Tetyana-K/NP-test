namespace WF_OpenDialog
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void btnLoad_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "������� ���� ��� ��������";
            openFileDialog1.Filter = "������� ����� (*.txt)|*.txt|�� ����� (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK) // openFileDialog1.ShowDialog()  - ������� �������� ����, ������� ��������� 䳿 �����������
            {
                string filePath = openFileDialog1.FileName; // �������� ������ ���� �� ��������� �����
                string fileContent = File.ReadAllText(filePath); // ������ ���� �����
                richTextBox1.Text = fileContent; // ���������� ���� � RichTextBox
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "�������� ���� ��";
            saveFileDialog1.Filter = "������� ����� (*.txt)|*.txt|�� ����� (*.*)|*.*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog1.FileName; // �������� ������ ���� �� ����� ��� ����������
                File.WriteAllText(filePath, richTextBox1.Text); // �������� ���� RichTextBox � ����
            }
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.ForeColor = colorDialog1.Color; // ������������ �������� ���� ������
            }
        }
    }
}
