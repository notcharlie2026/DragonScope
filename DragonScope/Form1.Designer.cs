namespace DragonScope
{
    partial class Form1
    {
        private System.Windows.Forms.Button btnOpenCsv;
        private System.Windows.Forms.Button btnOpenXml;
        private System.Windows.Forms.Label lblCsvFile;
        private System.Windows.Forms.Label lblXmlFile;
        private System.Windows.Forms.TextBox textBoxOutput;

        private void InitializeComponent()
        {
            btnOpenCsv = new Button();
            btnOpenXml = new Button();
            lblCsvFile = new Label();
            lblXmlFile = new Label();
            textBoxOutput = new TextBox();
            progressBar1 = new ProgressBar();
            SuspendLayout();
            // 
            // btnOpenCsv
            // 
            btnOpenCsv.Location = new Point(12, 12);
            btnOpenCsv.Name = "btnOpenCsv";
            btnOpenCsv.Size = new Size(75, 23);
            btnOpenCsv.TabIndex = 0;
            btnOpenCsv.Text = "Open CSV";
            btnOpenCsv.UseVisualStyleBackColor = true;
            btnOpenCsv.Click += btnOpenCsv_Click;
            // 
            // btnOpenXml
            // 
            btnOpenXml.Location = new Point(12, 41);
            btnOpenXml.Name = "btnOpenXml";
            btnOpenXml.Size = new Size(75, 23);
            btnOpenXml.TabIndex = 1;
            btnOpenXml.Text = "Open XML";
            btnOpenXml.UseVisualStyleBackColor = true;
            btnOpenXml.Click += btnOpenXml_Click;
            // 
            // lblCsvFile
            // 
            lblCsvFile.AutoSize = true;
            lblCsvFile.Location = new Point(93, 17);
            lblCsvFile.Name = "lblCsvFile";
            lblCsvFile.Size = new Size(0, 15);
            lblCsvFile.TabIndex = 2;
            // 
            // lblXmlFile
            // 
            lblXmlFile.AutoSize = true;
            lblXmlFile.Location = new Point(93, 46);
            lblXmlFile.Name = "lblXmlFile";
            lblXmlFile.Size = new Size(0, 15);
            lblXmlFile.TabIndex = 3;
            // 
            // textBoxOutput
            // 
            textBoxOutput.Location = new Point(12, 70);
            textBoxOutput.Multiline = true;
            textBoxOutput.Name = "textBoxOutput";
            textBoxOutput.Size = new Size(776, 368);
            textBoxOutput.TabIndex = 4;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(341, 23);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(177, 23);
            progressBar1.TabIndex = 5;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(progressBar1);
            Controls.Add(textBoxOutput);
            Controls.Add(lblXmlFile);
            Controls.Add(lblCsvFile);
            Controls.Add(btnOpenXml);
            Controls.Add(btnOpenCsv);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }
        private ProgressBar progressBar1;
    }
}