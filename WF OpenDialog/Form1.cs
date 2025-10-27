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
            openFileDialog1.Title = "Виберіть файл для відкриття";
            openFileDialog1.Filter = "Текстові файли (*.txt)|*.txt|Всі файли (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK) // openFileDialog1.ShowDialog()  - відкриває діалогове вікно, повертає результат дії користувача
            {
                string filePath = openFileDialog1.FileName; // отримуємо повний шлях до вибраного файлу
                string fileContent = File.ReadAllText(filePath); // читаємо вміст файлу
                richTextBox1.Text = fileContent; // відображаємо вміст у RichTextBox
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Зберегти файл як";
            saveFileDialog1.Filter = "Текстові файли (*.txt)|*.txt|Всі файли (*.*)|*.*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog1.FileName; // отримуємо повний шлях до файлу для збереження
                File.WriteAllText(filePath, richTextBox1.Text); // записуємо вміст RichTextBox у файл
            }
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.ForeColor = colorDialog1.Color; // встановлюємо вибраний колір тексту
            }
        }
    }
}
