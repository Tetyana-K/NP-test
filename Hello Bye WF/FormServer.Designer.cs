namespace Hello_Bye_WF
{
    partial class FormServer
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
            rtbLog = new RichTextBox();
            tbMessage = new TextBox();
            btnStart = new Button();
            btnStop = new Button();
            btnSend = new Button();
            SuspendLayout();
            // 
            // rtbLog
            // 
            rtbLog.Location = new Point(59, 32);
            rtbLog.Name = "rtbLog";
            rtbLog.Size = new Size(368, 212);
            rtbLog.TabIndex = 0;
            rtbLog.Text = "";
            // 
            // tbMessage
            // 
            tbMessage.Location = new Point(59, 301);
            tbMessage.Name = "tbMessage";
            tbMessage.Size = new Size(368, 23);
            tbMessage.TabIndex = 1;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(472, 32);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(174, 23);
            btnStart.TabIndex = 2;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(472, 86);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(174, 23);
            btnStop.TabIndex = 3;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // btnSend
            // 
            btnSend.Location = new Point(472, 300);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(174, 23);
            btnSend.TabIndex = 4;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // FormServer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(723, 450);
            Controls.Add(btnSend);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            Controls.Add(tbMessage);
            Controls.Add(rtbLog);
            Name = "FormServer";
            Text = "Server";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox rtbLog;
        private TextBox tbMessage;
        private Button btnStart;
        private Button btnStop;
        private Button btnSend;
    }
}
