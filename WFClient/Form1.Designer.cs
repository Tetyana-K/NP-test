namespace WFClient
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
            tbServer = new TextBox();
            tbPort = new TextBox();
            button1 = new Button();
            tbMessage = new TextBox();
            tbName = new TextBox();
            tbInfo = new TextBox();
            button2 = new Button();
            SuspendLayout();
            // 
            // tbServer
            // 
            tbServer.Location = new Point(250, 49);
            tbServer.Name = "tbServer";
            tbServer.PlaceholderText = "server";
            tbServer.Size = new Size(190, 23);
            tbServer.TabIndex = 0;
            // 
            // tbPort
            // 
            tbPort.Location = new Point(250, 99);
            tbPort.Name = "tbPort";
            tbPort.PlaceholderText = "port";
            tbPort.Size = new Size(190, 23);
            tbPort.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new Point(250, 253);
            button1.Name = "button1";
            button1.Size = new Size(190, 23);
            button1.TabIndex = 2;
            button1.Text = "Connect";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // tbMessage
            // 
            tbMessage.Location = new Point(250, 155);
            tbMessage.Name = "tbMessage";
            tbMessage.PlaceholderText = "message";
            tbMessage.Size = new Size(190, 23);
            tbMessage.TabIndex = 3;
            // 
            // tbName
            // 
            tbName.Location = new Point(250, 203);
            tbName.Name = "tbName";
            tbName.PlaceholderText = "message";
            tbName.Size = new Size(190, 23);
            tbName.TabIndex = 4;
            tbName.Text = "name";
            // 
            // tbInfo
            // 
            tbInfo.Location = new Point(512, 49);
            tbInfo.Multiline = true;
            tbInfo.Name = "tbInfo";
            tbInfo.Size = new Size(203, 227);
            tbInfo.TabIndex = 5;
            // 
            // button2
            // 
            button2.Location = new Point(250, 304);
            button2.Name = "button2";
            button2.Size = new Size(190, 23);
            button2.TabIndex = 6;
            button2.Text = "Disconnect";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button2);
            Controls.Add(tbInfo);
            Controls.Add(tbName);
            Controls.Add(tbMessage);
            Controls.Add(button1);
            Controls.Add(tbPort);
            Controls.Add(tbServer);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbServer;
        private TextBox tbPort;
        private Button button1;
        private TextBox tbMessage;
        private TextBox tbName;
        private TextBox tbInfo;
        private Button button2;
    }
}
