namespace DragonScope
{
    partial class Form1
    {
        private System.Windows.Forms.Button btnOpenCsv;
        private System.Windows.Forms.Button btnOpenXml;
        private System.Windows.Forms.Label lblCsvFile;
        private System.Windows.Forms.Label lblXmlFile;

        private void InitializeComponent()
        {
            btnOpenCsv = new Button();
            btnOpenXml = new Button();
            lblCsvFile = new Label();
            lblXmlFile = new Label();
            progressBar1 = new ProgressBar();
            textBoxOutput = new RichTextBox();
            panel1 = new Panel();
            textBox1 = new TextBox();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // btnOpenCsv
            // 
            btnOpenCsv.Location = new Point(12, 50);
            btnOpenCsv.Margin = new Padding(3, 4, 3, 4);
            btnOpenCsv.Name = "btnOpenCsv";
            btnOpenCsv.Size = new Size(86, 31);
            btnOpenCsv.TabIndex = 1;
            btnOpenCsv.Text = "Open CSV";
            btnOpenCsv.UseVisualStyleBackColor = true;
            btnOpenCsv.Click += btnOpenCsv_Click;
            // 
            // btnOpenXml
            // 
            btnOpenXml.Location = new Point(3, 39);
            btnOpenXml.Margin = new Padding(3, 4, 3, 4);
            btnOpenXml.Name = "btnOpenXml";
            btnOpenXml.Size = new Size(86, 31);
            btnOpenXml.TabIndex = 0;
            btnOpenXml.Text = "Open XML";
            btnOpenXml.UseVisualStyleBackColor = true;
            btnOpenXml.Click += btnOpenXml_Click;
            // 
            // lblCsvFile
            // 
            lblCsvFile.AutoSize = true;
            lblCsvFile.Location = new Point(106, 61);
            lblCsvFile.Name = "lblCsvFile";
            lblCsvFile.Size = new Size(0, 20);
            lblCsvFile.TabIndex = 2;
            // 
            // lblXmlFile
            // 
            lblXmlFile.AutoSize = true;
            lblXmlFile.Location = new Point(106, 31);
            lblXmlFile.Name = "lblXmlFile";
            lblXmlFile.Size = new Size(0, 20);
            lblXmlFile.TabIndex = 3;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(698, 61);
            progressBar1.Margin = new Padding(3, 4, 3, 4);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(202, 31);
            progressBar1.TabIndex = 5;
            // 
            // textBoxOutput
            // 
            textBoxOutput.Location = new Point(12, 99);
            textBoxOutput.Name = "textBoxOutput";
            textBoxOutput.Size = new Size(890, 489);
            textBoxOutput.TabIndex = 6;
            textBoxOutput.Text = "";
            // 
            // panel1
            // 
            panel1.Controls.Add(btnOpenXml);
            panel1.Controls.Add(textBox1);
            panel1.Location = new Point(908, 11);
            panel1.Name = "panel1";
            panel1.Size = new Size(250, 577);
            panel1.TabIndex = 7;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(3, 5);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(244, 27);
            textBox1.TabIndex = 8;
            textBox1.Text = "Config";
            textBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1254, 600);
            Controls.Add(panel1);
            Controls.Add(textBoxOutput);
            Controls.Add(progressBar1);
            Controls.Add(lblXmlFile);
            Controls.Add(lblCsvFile);
            Controls.Add(btnOpenCsv);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Dragon Scope";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
        private ProgressBar progressBar1;
        private RichTextBox textBoxOutput;
        private Panel panel1;
        private TextBox textBox1;
    }
}