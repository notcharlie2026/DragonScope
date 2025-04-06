using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DragonScope
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnOpenCsv_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ParseCsvFile(openFileDialog.FileName);
                    lblCsvFile.Text = openFileDialog.FileName;
                }
            }
        }

        private void btnOpenXml_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ParseXmlFile(openFileDialog.FileName);
                    lblXmlFile.Text = openFileDialog.FileName;
                }
            }
        }

        private void ParseCsvFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            for (int it = 0; it < lines.Length; it++)
            {
                string line = lines[it];
                var values = line.Split(',');
                for (int i = 0; i < values.Length; i++)
                {
                    if (i == 1) // Assuming the second column is the one to check
                    {
                        if (values[i].Contains("Fault_"))
                        {
                            WriteToTextBox(values[i] + " has value: ");
                        }
                    }
                }
                progressBar1.Value = (int)((float)it / lines.Length * 100); // Update progress bar
            }
        }

        private void ParseXmlFile(string filePath)
        {
            // Implement XML parsing logic here
            var xmlDoc = XDocument.Load(filePath);
            foreach (var element in xmlDoc.Descendants())
            {
                // Process XML elements
            }
        }

        private void WriteToTextBox(string text)
        {
            textBoxOutput.AppendText(text + $"{Environment.NewLine}");
        }
    }
}