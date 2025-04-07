using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DragonScope
{
    public partial class Form1 : Form
    {
        private Dictionary<string, (string Type, string RangeHigh, string RangeLow)> xmlData;

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
            var errors = new HashSet<string>();
            var lines = File.ReadAllLines(filePath);

            for (int it = 0; it < lines.Length; it++)
            {
                string line = lines[it];
                var values = line.Split(',');

                if (values.Length > 2 && xmlData.Keys.Any(k => k == values[1])) //TODO fix this to accept values that contain string in xml
                {
                    var (type, rangeHigh, rangeLow) = xmlData[values[1]];

                    if (type == "bool" && values[2] == "1")
                    {
                        if (errors.Add(values[1]))
                        {
                            WriteToTextBox(values[1] + " has value: " + values[2]);
                        }
                    }
                    else if (type == "range" && int.TryParse(values[2], out int intValue))
                    {
                        if (int.TryParse(rangeLow, out int low) && int.TryParse(rangeHigh, out int high))
                        {
                            if (intValue >= low && intValue <= high)
                            {
                                if (errors.Add(values[1]))
                                {
                                    WriteToTextBox(values[1] + " has value: " + values[2]);
                                }
                            }
                        }
                    }
                }

                progressBar1.Value = (int)((float)it / lines.Length * 100); // Update progress bar
            }
        }

        private void ParseXmlFile(string filePath)
        {
            xmlData = new Dictionary<string, (string Type, string RangeHigh, string RangeLow)>();
            var xmlDoc = XDocument.Load(filePath);
            foreach (var element in xmlDoc.Descendants("Value"))
            {
                var name = element.Attribute("Name")?.Value;
                var type = element.Attribute("Type")?.Value;
                var rangeHigh = element.Attribute("RangeHigh")?.Value;
                var rangeLow = element.Attribute("RangeLow")?.Value;

                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(type))
                {
                    xmlData[name] = (type, rangeHigh, rangeLow);
                }
            }
        }

        private void WriteToTextBox(string text)
        {
            textBoxOutput.AppendText(text + $"{Environment.NewLine}");
        }
    }
}