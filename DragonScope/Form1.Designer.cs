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
            this.btnOpenCsv = new System.Windows.Forms.Button();
            this.btnOpenXml = new System.Windows.Forms.Button();
            this.lblCsvFile = new System.Windows.Forms.Label();
            this.lblXmlFile = new System.Windows.Forms.Label();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnOpenCsv
            // 
            this.btnOpenCsv.Location = new System.Drawing.Point(12, 12);
            this.btnOpenCsv.Name = "btnOpenCsv";
            this.btnOpenCsv.Size = new System.Drawing.Size(75, 23);
            this.btnOpenCsv.TabIndex = 0;
            this.btnOpenCsv.Text = "Open CSV";
            this.btnOpenCsv.UseVisualStyleBackColor = true;
            this.btnOpenCsv.Click += new System.EventHandler(this.btnOpenCsv_Click);
            // 
            // btnOpenXml
            // 
            this.btnOpenXml.Location = new System.Drawing.Point(12, 41);
            this.btnOpenXml.Name = "btnOpenXml";
            this.btnOpenXml.Size = new System.Drawing.Size(75, 23);
            this.btnOpenXml.TabIndex = 1;
            this.btnOpenXml.Text = "Open XML";
            this.btnOpenXml.UseVisualStyleBackColor = true;
            this.btnOpenXml.Click += new System.EventHandler(this.btnOpenXml_Click);
            // 
            // lblCsvFile
            // 
            this.lblCsvFile.AutoSize = true;
            this.lblCsvFile.Location = new System.Drawing.Point(93, 17);
            this.lblCsvFile.Name = "lblCsvFile";
            this.lblCsvFile.Size = new System.Drawing.Size(0, 15);
            this.lblCsvFile.TabIndex = 2;
            // 
            // lblXmlFile
            // 
            this.lblXmlFile.AutoSize = true;
            this.lblXmlFile.Location = new System.Drawing.Point(93, 46);
            this.lblXmlFile.Name = "lblXmlFile";
            this.lblXmlFile.Size = new System.Drawing.Size(0, 15);
            this.lblXmlFile.TabIndex = 3;
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Location = new System.Drawing.Point(12, 70);
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.Size = new System.Drawing.Size(776, 368);
            this.textBoxOutput.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBoxOutput);
            this.Controls.Add(this.lblXmlFile);
            this.Controls.Add(this.lblCsvFile);
            this.Controls.Add(this.btnOpenXml);
            this.Controls.Add(this.btnOpenCsv);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}