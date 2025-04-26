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
            SuspendLayout();
            // 
            // btnOpenCsv
            // 
            btnOpenCsv.Location = new Point(10, 8);
            btnOpenCsv.Name = "btnOpenCsv";
            btnOpenCsv.Size = new Size(75, 23);
            btnOpenCsv.TabIndex = 1;
            btnOpenCsv.Text = "Open CSV";
            btnOpenCsv.UseVisualStyleBackColor = true;
            btnOpenCsv.Click += btnOpenCsv_Click;
            // 
            // btnOpenXml
            // 
            btnOpenXml.Location = new Point(10, 37);
            btnOpenXml.Name = "btnOpenXml";
            btnOpenXml.Size = new Size(75, 23);
            btnOpenXml.TabIndex = 0;
            btnOpenXml.Text = "Open XML";
            btnOpenXml.UseVisualStyleBackColor = true;
            btnOpenXml.Click += btnOpenXml_Click;
            // 
            // lblCsvFile
            // 
            lblCsvFile.Location = new Point(90, 46);
            lblCsvFile.Name = "lblCsvFile";
            lblCsvFile.Size = new Size(0, 23);
            lblCsvFile.TabIndex = 2;
            // 
            // lblXmlFile
            // 
            lblXmlFile.AutoSize = true;
            lblXmlFile.Location = new Point(90, 40);
            lblXmlFile.Name = "lblXmlFile";
            lblXmlFile.Size = new Size(0, 15);
            lblXmlFile.TabIndex = 3;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(611, 46);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(177, 23);
            progressBar1.TabIndex = 5;
            // 
            // textBoxOutput
            // 
            textBoxOutput.Location = new Point(10, 74);
            textBoxOutput.Margin = new Padding(3, 2, 3, 2);
            textBoxOutput.Name = "textBoxOutput";
            textBoxOutput.Size = new Size(779, 368);
            textBoxOutput.TabIndex = 6;
            textBoxOutput.Text = "";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(798, 450);
            Controls.Add(btnOpenXml);
            Controls.Add(textBoxOutput);
            Controls.Add(progressBar1);
            Controls.Add(lblXmlFile);
            Controls.Add(lblCsvFile);
            Controls.Add(btnOpenCsv);
            Name = "Form1";
            Text = "Dragon Scope";
            ResumeLayout(false);
            PerformLayout();
        }
        private ProgressBar progressBar1;
        private RichTextBox textBoxOutput;
    }
}