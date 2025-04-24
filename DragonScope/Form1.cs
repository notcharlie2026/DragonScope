using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.LinkLabel;

namespace DragonScope
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private Dictionary<string, (string RangeHigh, string RangeLow, string priority)> xmlDataRange;
        private Dictionary<string, (string FlagState, string priority)> xmlDataBool;

        private bool xmlinit = false;

        private void btnOpenCsv_Click(object sender, EventArgs e)
        {
            if(!xmlinit)
            {
                MessageBox.Show("Please load the XML file first.");
                return;
            }
            textBoxOutput.Text = "";
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
                if (Directory.Exists(Properties.Settings.Default.LastXmlPath))
                {
                    openFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
                    openFileDialog.InitialDirectory = Path.GetDirectoryName(Properties.Settings.Default.LastXmlPath);
                }
                else
                {
                    openFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
                }
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ParseXmlFile(openFileDialog.FileName);
                    lblXmlFile.Text = openFileDialog.FileName;
                    xmlinit = true;

                    // Save the path to application settings
                    Properties.Settings.Default.LastXmlPath = openFileDialog.FileName;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void ParseCsvFile(string filePath)
        {
            var activeConditions = new Dictionary<string, float>(); // Tracks active faults or out-of-bounds conditions
            var lines = File.ReadAllLines(filePath);
            float robotenable = GetRobotEnableTime(lines);
            bool namefoundxml = false;
            int currentxmlIndex = 0;
            string currentxmlType = "";
            string[] xmlRangeNames = xmlDataRange.Keys.ToArray();
            string[] xmlBoolNames = xmlDataBool.Keys.ToArray();

            List<string[]> xmlList = new List<string[]>();

            xmlList.Add(xmlBoolNames);
            xmlList.Add(xmlRangeNames);

            int linesparsed = 0;
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            for (int it = 0; it < lines.Length; it++)
            {
                string line = lines[it];
                var values = line.Split(',');

                if (values.Length > 2)
                {
                    foreach (var xml in xmlList)
                    {
                        int i = 0;
                        foreach (var name in xml)
                        { 
                        if (values[1].Contains(name))
                        {
                            namefoundxml = true;
                            currentxmlIndex = i;
                            currentxmlType = name;
                            break;
                        }
                            i++;
                        }
                    }
                    if (namefoundxml)
                    {
                        if (currentxmlIndex == 1)
                        {
                            if (values[2] == "1")
                            {
                                if (!activeConditions.ContainsKey(values[1]))
                                {
                                    if (float.TryParse(values[0], out float timeValue))
                                    {
                                        activeConditions[values[1]] = timeValue - robotenable; // Start time
                                    }
                                }
                            }
                            else
                            {
                                if (activeConditions.ContainsKey(values[1]))
                                {
                                    if (float.TryParse(values[0], out float timeValue))
                                    {
                                        float startTime = activeConditions[values[1]];
                                        float endTime = timeValue - robotenable;
                                        WriteToTextBox($"\"{values[1]}\" was true from {startTime} to {endTime}".Remove(0, 6), 1);
                                        activeConditions.Remove(values[1]);
                                    }
                                }
                            }
                        }
                        else if (currentxmlIndex == 0 && float.TryParse(values[2], out float intValue))
                        {
                            var (rangeHigh, rangeLow, priority) = xmlDataRange[currentxmlType];
                            if (float.TryParse(rangeLow, out float low) && float.TryParse(rangeHigh, out float high))
                            {
                                if (intValue <= low || intValue >= high)
                                {
                                    if (!activeConditions.ContainsKey(values[1]))
                                    {
                                        if (float.TryParse(values[0], out float timeValue))
                                        {
                                            activeConditions[values[1]] = timeValue - robotenable; // Start time
                                        }
                                    }
                                }
                                else
                                {
                                    if (activeConditions.ContainsKey(values[1]))
                                    {
                                        if (float.TryParse(values[0], out float timeValue))
                                        {
                                            float startTime = activeConditions[values[1]];
                                            float endTime = timeValue - robotenable;
                                            WriteToTextBox($"\"{values[1]}\" was out of bounds from {startTime} to {endTime}".Remove(0, 6), 1);
                                            activeConditions.Remove(values[1]);
                                        }
                                    }
                                }
                            }
                        }
                        //reset the xml name found flag
                        namefoundxml = false;
                        currentxmlIndex = 0;
                        currentxmlType = "";
                    }
                    //else if (values[1].Contains("/StickyFault_") && values[2] == "1")
                    //{
                    //    if (!activeConditions.ContainsKey(values[1]))
                    //    {
                    //        if (float.TryParse(values[0], out float timeValue))
                    //        {
                    //            activeConditions[values[1]] = timeValue - robotenable; // Start time
                    //        }
                    //    }
                    //}
                    //else if (values[1].Contains("/StickyFault_") && values[2] == "0")
                    //{
                    //    if (activeConditions.ContainsKey(values[1]))
                    //    {
                    //        if (float.TryParse(values[0], out float timeValue))
                    //        {
                    //            float startTime = activeConditions[values[1]];
                    //            float endTime = timeValue - robotenable;
                    //            WriteToTextBox($"\"{values[1]}\" was true from {startTime} to {endTime}".Remove(0, 6), 1);
                    //            activeConditions.Remove(values[1]);
                    //        }
                    //    }
                    //}
                    //else if (values[1].Contains("/Fault_") && values[2] == "1")
                    //{
                    //    if (!activeConditions.ContainsKey(values[1]))
                    //    {
                    //        if (float.TryParse(values[0], out float timeValue))
                    //        {
                    //            activeConditions[values[1]] = timeValue - robotenable; // Start time
                    //        }
                    //    }
                    //}
                    //else if (values[1].Contains("/Fault_") && values[2] == "0")
                    //{
                    //    if (activeConditions.ContainsKey(values[1]))
                    //    {
                    //        if (float.TryParse(values[0], out float timeValue))
                    //        {
                    //            float startTime = activeConditions[values[1]];
                    //            float endTime = timeValue - robotenable;
                    //            WriteToTextBox($"\"{values[1]}\" was true from {startTime} to {endTime}".Remove(0,6), 1);
                    //            activeConditions.Remove(values[1]);
                    //        }
                    //    }
                    //}

                    progressBar1.Value = (int)((float)it / lines.Length * 100); // Update progress bar
                    linesparsed = it;
                }
            }

            // Handle any remaining active conditions at the end of the file
            foreach (var condition in activeConditions)
            {
                WriteToTextBox($"\"{condition.Key}\" started at {condition.Value} and did not end.".Remove(0, 6), 2);
            }

            progressBar1.Value = 100; // Ensure progress bar is full at the end
            stopwatch.Stop();
            WriteToTextBox( linesparsed+1.ToString()+" entries parsed in " + stopwatch.Elapsed.TotalSeconds.ToString() + " seconds", 0);
        }
        private void ParseXmlFile(string filePath)
        {
            xmlDataRange = new Dictionary<string, (string RangeHigh, string RangeLow, string priority)>();
            xmlDataBool = new Dictionary<string, (string FlagState, string priority)>();
            var xmlDoc = XDocument.Load(filePath);
            foreach (var element in xmlDoc.Descendants("RangeValue"))
            {
                var name = element.Attribute("Name")?.Value;
                var rangeHigh = element.Attribute("Rangehigh")?.Value ?? string.Empty; // Ensure non-null value
                var rangeLow = element.Attribute("Rangelow")?.Value ?? string.Empty;   // Ensure non-null value
                var priority = element.Attribute("Priority")?.Value ?? string.Empty;  // Ensure non-null value

                if (!string.IsNullOrEmpty(name))
                {
                    xmlDataRange[name] = ( rangeHigh, rangeLow, priority);
                }
            }
            foreach (var element in xmlDoc.Descendants("BoolValue"))
            {
                var name = element.Attribute("Name")?.Value;
                var flagState = element.Attribute("FlagState")?.Value ?? string.Empty; // Ensure non-null value
                var priority = element.Attribute("Priority")?.Value ?? string.Empty;  // Ensure non-null value

                if (!string.IsNullOrEmpty(name))
                {
                    xmlDataBool[name] = (flagState, priority);
                }
            }
        }
        private float GetRobotEnableTime(string[] lines)
        {
            for (int it = 0; it < lines.Length; it++)
            {
                string line = lines[it];
                var values = line.Split(',');
                if (values.Length > 2 && values[1].Contains("RobotEnable"))
                {
                    if (values[2] == "true")
                    {
                        if (float.TryParse(values[0], out float parsedValue))
                        {
                            return parsedValue;
                        }
                    }
                }
            }
            return 0; // Placeholder return value
        }
        private void WriteToTextBox(string text, int priority)
        {
            // Apply the color based on priority
            switch (priority)
            {
                case 1:
                    textBoxOutput.SelectionColor = System.Drawing.Color.Red;
                    break;
                case 2:
                    textBoxOutput.SelectionColor = System.Drawing.Color.Orange;
                    break;
                case 3:
                    textBoxOutput.SelectionColor = System.Drawing.Color.Yellow;
                    break;
                default:
                    textBoxOutput.SelectionColor = System.Drawing.Color.Black;
                    break;
            }
            if (!textBoxOutput.Text.Contains(text))
            {
                textBoxOutput.AppendText(text + $"{Environment.NewLine}");
            }
        }
        //TODO add a method to loop through each line and find the start of robotenable so there is less on the fly calculation and we can
        //get errors taht happend before robotenable
        private void ConvertWpilogToCsv(string wpilogPath, string csvPath)
        {
            var lines = File.ReadAllLines(wpilogPath);
            using (var writer = new StreamWriter(csvPath))
            {
                // Write CSV header
                writer.WriteLine("Timestamp,Key,Value");

                foreach (var line in lines)
                {
                    // Assuming wpilog is in a key-value format with timestamps
                    var parts = line.Split(' '); // Adjust delimiter based on wpilog format
                    if (parts.Length >= 3)
                    {
                        string timestamp = parts[0];
                        string key = parts[1];
                        string value = string.Join(" ", parts.Skip(2));
                        writer.WriteLine($"{timestamp},{key},{value}");
                    }
                }
            }
        }

        private void ConvertHootLogToWpilog(string hootLogPath, string wpilogPath)
        {
            // Assuming Owlet is a command-line tool
            string owletExecutable = @"path\to\owlet.exe"; // Update with the actual path to Owlet
            string arguments = $"convert \"{hootLogPath}\" \"{wpilogPath}\"";

            var process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = owletExecutable,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception($"Owlet conversion failed: {error}");
            }
        }
    }
}