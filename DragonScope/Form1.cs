using System.Reflection;
using System.Xml.Linq;
using System.IO;
using Windows.Graphics.Printing3D;

namespace DragonScope
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DragonScope.icon.ico");
            if (stream != null)
            {
                this.Icon = new Icon(stream);
            }
        }
        private Dictionary<string, (string RangeHigh, string RangeLow, string priority)> xmlDataRange;
        private Dictionary<string, (string FlagState, string priority)> xmlDataBool;
        private Dictionary<string, string> xmlAlias;
        private List<string> m_excludedStrings = new List<string>();
        private bool m_xmlInit = false;

        string m_currentxmlType = "";

        private enum m_xmlDataType
        {
            TYPE_BOOLEAN = 0,
            TYPE_RANGE = 1,
            TYPE_EXCLUDED = 2,
            TYPE_INVALID = -1
        }

        private void btnOpenCsv_Click(object sender, EventArgs e)
        {
            if (!m_xmlInit)
            {
                MessageBox.Show("Please load the XML file first.");
                return;
            }
            textBoxOutput.Text = "";
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;
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
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\GitHub\\DragonScope";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ParseXmlFile(openFileDialog.FileName);
                    lblXmlFile.Text = openFileDialog.FileName;
                    m_xmlInit = true;
                }
            }
        }

        private void ParseCsvFile(string filePath)
        {
            var activeConditions = new Dictionary<string, float>(); // Tracks active faults or out-of-bounds conditions
            var lines = File.ReadAllLines(filePath);
            float robotenable = GetRobotEnableTime(lines);
            string[] xmlRangeNames = xmlDataRange.Keys.ToArray();
            string[] xmlBoolNames = xmlDataBool.Keys.ToArray();

            int linesparsed = 0;
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            for (int it = 0; it < lines.Length; it++)
            {
                string line = lines[it];
                var values = line.Split(',');

                if (values.Length > 2)
                {
                    var currentxmlIndex = GetTypeFromXml(values[1]); // Get the type from XML data
                    string displayName = GetAlias(values[1]); // Get alias or original name
                    switch (currentxmlIndex)
                    {
                        case m_xmlDataType.TYPE_BOOLEAN:
                            var (flagState, boolPriority) = xmlDataBool[m_currentxmlType]; // Renamed 'priority' to 'boolPriority'
                            if (values[2] == flagState)
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
                                    if (float.TryParse(values[0], out float timeValue) && int.TryParse(boolPriority, out int boolPriorityInt))
                                    {
                                        float startTime = activeConditions[values[1]];
                                        float endTime = timeValue - robotenable;
                                        WriteToTextBox($"\"{displayName}\" was true from {startTime} to {endTime}".Remove(0, 6), 1);
                                        activeConditions.Remove(values[1]);
                                    }
                                }
                            }
                            break;
                        case m_xmlDataType.TYPE_RANGE:
                            if (float.TryParse(values[2], out float intValue))
                            {
                                var (rangeHigh, rangeLow, rangePriority) = xmlDataRange[m_currentxmlType]; // Renamed 'priority' to 'rangePriority'
                                if (float.TryParse(rangeLow, out float low) && float.TryParse(rangeHigh, out float high))
                                {
                                    if (intValue < low || intValue > high)
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
                                            if (float.TryParse(values[0], out float timeValue) && int.TryParse(rangePriority, out int rangePriorityInt))
                                            {
                                                float startTime = activeConditions[values[1]];
                                                float endTime = timeValue - robotenable;
                                                WriteToTextBox($"\"{displayName}\" was out of bounds from {startTime} to {endTime}".Remove(0, 6), rangePriorityInt);
                                                activeConditions.Remove(values[1]);
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case m_xmlDataType.TYPE_EXCLUDED:

                            break;
                        default:
                            m_currentxmlType = string.Empty;
                            break;
                    }
                    // Reset the xml name found flag
                    currentxmlIndex = 0;
                    m_currentxmlType = "";

                    progressBar1.Value = (int)((float)it / lines.Length * 100); // Update progress bar
                    linesparsed = it;
                }
            }

            // Handle any remaining active conditions at the end of the file
            foreach (var condition in activeConditions)
            {
                WriteToTextBox($"\"{condition.Key}\" started at {condition.Value} and did not end.".Remove(0, 6), 4);
            }

            progressBar1.Value = 100; // Ensure progress bar is full at the end
            stopwatch.Stop();
            WriteToTextBox(linesparsed + 1.ToString() + " entries parsed in " + stopwatch.Elapsed.TotalSeconds.ToString() + " seconds", 0);
        }
        private m_xmlDataType GetTypeFromXml(string name)
        {
            foreach (var key in m_excludedStrings)
            {
                if (name.Contains(key))
                {
                    m_currentxmlType = key;
                    return m_xmlDataType.TYPE_EXCLUDED;
                }
            }
            foreach (var key in xmlDataRange.Keys)
            {
                if (name.Contains(key))
                {
                    m_currentxmlType = key;
                    return m_xmlDataType.TYPE_RANGE;
                }
            }
            foreach (var key in xmlDataBool.Keys)
            {
                if (name.Contains(key))
                {
                    m_currentxmlType = key;
                    return m_xmlDataType.TYPE_BOOLEAN;
                }
            }
            return m_xmlDataType.TYPE_INVALID;
        }
        private void ParseXmlFile(string filePath)
        {
            xmlDataRange = new Dictionary<string, (string RangeHigh, string RangeLow, string priority)>();
            xmlDataBool = new Dictionary<string, (string FlagState, string priority)>();
            xmlAlias = new Dictionary<string, string>(); // Ensure it's initialized
            var xmlDoc = XDocument.Load(filePath);
            foreach (var element in xmlDoc.Descendants("ExcludedValue"))
            {
                var name = element.Attribute("Name")?.Value;
                if (!string.IsNullOrEmpty(name))
                {
                    m_excludedStrings.Add(name);
                }
            }
            foreach (var element in xmlDoc.Descendants("RangeValue"))
            {
                var name = element.Attribute("Name")?.Value;
                var rangeHigh = element.Attribute("Rangehigh")?.Value ?? string.Empty; // Ensure non-null value
                var rangeLow = element.Attribute("Rangelow")?.Value ?? string.Empty;   // Ensure non-null value
                var priority = element.Attribute("Priority")?.Value ?? string.Empty;  // Ensure non-null value

                if (!string.IsNullOrEmpty(name))
                {
                    xmlDataRange[name] = (rangeHigh, rangeLow, priority);
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
            foreach (var element in xmlDoc.Descendants("CANDiviceAlias"))
            {
                var logName = element.Attribute("LogName")?.Value;
                var alias = element.Attribute("Alias")?.Value;

                if (!string.IsNullOrEmpty(logName) && !string.IsNullOrEmpty(alias))
                {
                    xmlAlias[logName] = alias;
                }
            }
        }
        // 3. Add a helper to get alias or fallback to original name
        private string GetAlias(string deviceName)
        {
            foreach (var kvp in xmlAlias)
            {
                if (deviceName.Contains(kvp.Key))
                {
                    // Replace only the matching part with the alias
                    return deviceName.Replace(kvp.Key, kvp.Value);
                }
            }
            return deviceName;
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
                case 4:
                    textBoxOutput.SelectionColor = System.Drawing.Color.Purple;
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
        private void HootLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\GitHub\\DragonScope\\Data"))
                {
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\GitHub\\DragonScope\\Data");
                }
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Hoot Files (*.hoot)|*.hoot|All files (*.*)|*.*";
                    openFileDialog.RestoreDirectory = true;
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string fileNameOnly = Path.GetFileName(openFileDialog.FileName);
                        string targetPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\GitHub\\DragonScope\\Data", fileNameOnly);
                        targetPath.Replace(".hoot", ".wpilog");
                        ConvertHootLogToWpilog(openFileDialog.FileName, targetPath);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            progressBar1.Value = 0;
            string owletExecutable = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents\\GitHub\\DragonScope\\owlet-25.4.0-windowsx86-64.exe");
            string arguments = $"-f wpilog -F {hootLogPath} {wpilogPath}";
            if (!File.Exists(hootLogPath))
            {
                MessageBox.Show("Provide valid hoot directory, dufus");
            }

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
            Console.WriteLine($"Executing: {owletExecutable} {arguments}");

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();
            progressBar1.Value = 100;

            if (process.ExitCode != 0)
            {
                throw new Exception($"Owlet conversion failed: {error}");
            }
        }
    }
}