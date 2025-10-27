namespace WF_OpenDialog
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            openFileDialog1 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            btnLoad = new Button();
            richTextBox1 = new RichTextBox();
            colorDialog1 = new ColorDialog();
            btnSave = new Button();
            btnColor = new Button();
            SuspendLayout();
            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(42, 54);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(127, 52);
            btnLoad.TabIndex = 0;
            btnLoad.Text = "Open";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(223, 54);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(434, 342);
            richTextBox1.TabIndex = 1;
            richTextBox1.Text = "";
            // 
            // btnSave
            // 
            btnSave.Location = new Point(42, 156);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(127, 52);
            btnSave.TabIndex = 2;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnColor
            // 
            btnColor.Location = new Point(42, 262);
            btnColor.Name = "btnColor";
            btnColor.Size = new Size(127, 56);
            btnColor.TabIndex = 3;
            btnColor.Text = "Color";
            btnColor.UseVisualStyleBackColor = true;
            btnColor.Click += btnColor_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(709, 450);
            Controls.Add(btnColor);
            Controls.Add(btnSave);
            Controls.Add(richTextBox1);
            Controls.Add(btnLoad);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
        private Button btnLoad;
        private RichTextBox richTextBox1;
        private ColorDialog colorDialog1;
        private Button btnSave;
        private Button btnColor;
    }
}
