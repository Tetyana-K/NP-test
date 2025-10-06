
namespace Hello_Bye_WF_Client
{
    partial class FormClient
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
            rtbClientLogs = new RichTextBox();
            tbClientMessage = new TextBox();
            btnClientConnect = new Button();
            tbIP = new TextBox();
            tbPort = new TextBox();
            btnClientSend = new Button();
            SuspendLayout();
            // 
            // rtbClientLogs
            // 
            rtbClientLogs.Location = new Point(409, 25);
            rtbClientLogs.Name = "rtbClientLogs";
            rtbClientLogs.Size = new Size(258, 172);
            rtbClientLogs.TabIndex = 0;
            rtbClientLogs.Text = "";
            // 
            // tbClientMessage
            // 
            tbClientMessage.Location = new Point(409, 267);
            tbClientMessage.Name = "tbClientMessage";
            tbClientMessage.PlaceholderText = "Enter message";
            tbClientMessage.Size = new Size(258, 23);
            tbClientMessage.TabIndex = 1;
            // 
            // btnClientConnect
            // 
            btnClientConnect.Location = new Point(62, 128);
            btnClientConnect.Name = "btnClientConnect";
            btnClientConnect.Size = new Size(179, 23);
            btnClientConnect.TabIndex = 2;
            btnClientConnect.Text = "Connect";
            btnClientConnect.UseVisualStyleBackColor = true;
            btnClientConnect.Click += btnClientConnect_Click;
            // 
            // tbIP
            // 
            tbIP.Location = new Point(62, 38);
            tbIP.Name = "tbIP";
            tbIP.Size = new Size(179, 23);
            tbIP.TabIndex = 3;
            // 
            // tbPort
            // 
            tbPort.Location = new Point(62, 82);
            tbPort.Name = "tbPort";
            tbPort.Size = new Size(179, 23);
            tbPort.TabIndex = 4;
            // 
            // btnClientSend
            // 
            btnClientSend.Location = new Point(409, 334);
            btnClientSend.Name = "btnClientSend";
            btnClientSend.Size = new Size(258, 23);
            btnClientSend.TabIndex = 5;
            btnClientSend.Text = "Send";
            btnClientSend.UseVisualStyleBackColor = true;
            btnClientSend.Click += btnClientSend_Click;
            // 
            // FormClient
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnClientSend);
            Controls.Add(tbPort);
            Controls.Add(tbIP);
            Controls.Add(btnClientConnect);
            Controls.Add(tbClientMessage);
            Controls.Add(rtbClientLogs);
            Name = "FormClient";
            Text = "Client";
            ResumeLayout(false);
            PerformLayout();
        }



        #endregion

        private RichTextBox rtbClientLogs;
        private TextBox tbClientMessage;
       private Button btnClientConnect;
        private TextBox tbIP;
        private TextBox tbPort;
        private Button btnClientSend;
    }
}
