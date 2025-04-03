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
                    lblCsvFile.Text = openFileDialog.FileName;
                    ParseCsvFile(openFileDialog.FileName);
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
                    lblXmlFile.Text = openFileDialog.FileName;
                    ParseXmlFile(openFileDialog.FileName);
                }
            }
        }

        private void ParseCsvFile(string filePath)
        {
            // Implement CSV parsing logic here
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var values = line.Split(',');
                // Process CSV values
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
            textBoxOutput.Text = text;
        }
    }
}