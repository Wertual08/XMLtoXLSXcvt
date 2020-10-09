using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace XMLtoXLSXcvt
{
    public partial class ConverterForm : Form
    {
        private readonly static string Version = "0.1.2.8";
        private ConverterProperties Properties = new ConverterProperties();

        private void LoadConfig(string path)
        {
            ImageTextBox.Text = "";
            TemplateTextBox.Text = "";
            using (var file = File.OpenText(path))
            {
                while (!file.EndOfStream)
                {
                    var line = file.ReadLine();
                    var split_point = line.IndexOf(':');
                    var key = line.Substring(0, split_point).Trim();
                    var value = line.Substring(split_point + 1);

                    switch (key)
                    {
                        case "XMLPath": XMLTextBox.Text = value; break;
                        case "XLSXPath": XLSXTextBox.Text = value; break;
                        case "Image": ImageTextBox.AppendText(value + Environment.NewLine); break;
                        case "CRR": Properties.CRRValue = float.Parse(value); break;
                        case "ICWR": Properties.ICWRValue = float.Parse(value); break;
                        case "Template": TemplateTextBox.AppendText(value + Environment.NewLine); break;
                    }
                }
            }

            TemplateTextBox.Text = TemplateTextBox.Text.TrimEnd();
            TemplateTextBox.SelectAll();
            TemplateTextBox_TextChanged(TemplateTextBox, EventArgs.Empty);
            TemplateTextBox.Select(0, 0);
        }
        private void SaveConfig(string path)
        {
            using (var file = File.CreateText(path))
            {
                file.WriteLine("XMLPath:" + XMLTextBox.Text);
                file.WriteLine("XLSXPath:" + XLSXTextBox.Text);
                foreach (var line in ImageTextBox.Lines)
                    if (line.Length > 0) file.WriteLine("Image:" + line);
                file.WriteLine("CRR:" + Properties.CRRValue);
                file.WriteLine("ICWR:" + Properties.ICWRValue);
                foreach (var line in TemplateTextBox.Lines)
                    file.WriteLine("Template:" + line);
            }
        }

        private Dictionary<string, string> ParseConfig(string[] lines)
        {
            var result = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                if (line == "") continue;
                var split_point = line.IndexOf(':');
                if (split_point < 0)
                {
                    if (!result.ContainsKey("")) result.Add("", line);
                    return result;
                }
                else
                {
                    var key = line.Substring(0, split_point).Trim();
                    var value = line.Substring(split_point + 1).Trim();
                    if (!result.ContainsKey(key)) result.Add(key, value);
                }
            }
            return result;
        }

        public ConverterForm()
        {
            InitializeComponent();
            Text += " V" + Version;
        }

        private void ConvertButton_Click(object sender, EventArgs e)
        {
            try
            {
                var xml_path = XMLTextBox.Text;
                if (!File.Exists(xml_path))
                {
                    ErrorMessenger.ShowNoFileError(this, xml_path);
                    return;
                }
                var xlsx_path = XLSXTextBox.Text;
                if (!xlsx_path.ToLower().EndsWith(".xlsx")) xlsx_path += ".xlsx";
                try { xlsx_path = Path.GetFullPath(xlsx_path); }
                catch { ErrorMessenger.ShowBadFileNameError(this, xlsx_path); return; }

                TemplateNode template;
                try { template = new TemplateNode(TemplateTextBox.Lines); }
                catch (Exception ex) { ErrorMessenger.ShowBadTemplateError(this, ex.Message); return; }

                var converter = new Converter(xml_path, xlsx_path, template);

                ConvertButton.Enabled = false;

                ConverterBackgroundWorker.RunWorkerAsync(converter);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    ex.ToString(),
                    "Error: Can not convert XML file.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private void ConverterBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            var converter = e.Argument as Converter;
            if (worker == null || converter == null) return;

            converter.ProgressChanged += (object sr, EventArgs ea) =>
                worker.ReportProgress(converter.Progress);

            e.Result = converter.Convert();
        }
        private void ConverterBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
        }
        private void ConverterBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(this,
                    e.Error.ToString(),
                    "Error: Conversion terminated with exception.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else if (!(bool)e.Result)
            {
                ErrorMessenger.ShowCanNotSaveError(this);
            }

            ProgressBar.Value = 0;
            ConvertButton.Enabled = true;
        }

        private void ResolveImagesButton_Click(object sender, EventArgs e)
        {
            try
            {
                var xlsx_path = XLSXTextBox.Text;
                if (!File.Exists(xlsx_path))
                {
                    ErrorMessenger.ShowNoFileError(this, xlsx_path);
                    return;
                }

                var columns = ImageTextBox.Lines.Where((string str) => str.Length > 0).ToArray();
                if (columns.Length < 1)
                {
                    ErrorMessenger.ShowNoColumnsError(this);
                    return;
                }

                var downloader = new ImageDownloader(
                    xlsx_path, 
                    columns,
                    Properties.CRRValue,
                    Properties.ICWRValue);

                ResolveImagesButton.Enabled = false;
                StopImagesButton.Enabled = true;

                ResolverBackgroundWorker.RunWorkerAsync(downloader);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    ex.ToString(),
                    "Error: Can not resolve images in XLSX file.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private void StopImagesButton_Click(object sender, EventArgs e)
        {
            ResolverBackgroundWorker.CancelAsync();
        }
        private void ResolverBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            var downloader = e.Argument as ImageDownloader;
            if (worker == null || downloader == null) return;
            
            downloader.ProgressChanged += (object sr, EventArgs ea) =>
                worker.ReportProgress(downloader.Progress, downloader.Downloading);
            downloader.Cancel += (object sr, CancelEventArgs ea) => 
                ea.Cancel = worker.CancellationPending;

            try
            {
                downloader.Download();
                e.Result = true;
            }
            catch (FileLoadException ex)
            {
                e.Result = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ResolverBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
            ProgressLabel.Text = e.UserState as string;
        }
        private void ResolverBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(this,
                    e.Error.ToString(),
                    "Error: Resolving terminated with exception.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else if (!(bool)e.Result)
            {
                ErrorMessenger.ShowCanNotOpenError(this);
            }

            ProgressLabel.Text = "";
            ProgressBar.Value = 0;
            ResolveImagesButton.Enabled = true;
            StopImagesButton.Enabled = false;
        }

        private void ChooseXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenDialog.ShowDialog(this) != DialogResult.OK) return;
            XMLTextBox.Text = OpenDialog.FileName;
        }
        private void ChooseXLSXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SaveDialog.ShowDialog(this) != DialogResult.OK) return;
            XLSXTextBox.Text = SaveDialog.FileName;
        }
        private void LoadConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenDialog.Filter = "(*.ini)|*.ini";
                if (OpenDialog.ShowDialog(this) != DialogResult.OK) return;
                OpenDialog.Filter = "";

                LoadConfig(OpenDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    ex.ToString(),
                    "Error: Can not load config.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private void SaveConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (SaveDialog.ShowDialog(this) != DialogResult.OK) return;
                var filename = SaveDialog.FileName;
                if (!filename.ToLower().EndsWith(".ini")) filename += ".ini";

                SaveConfig(filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    ex.ToString(),
                    "Error: Can not save config.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConverterForm_Load(object sender, EventArgs e)
        {
            try { LoadConfig("config.ini"); }
            catch { }
        }
        private void ConverterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try { SaveConfig("config.ini"); }
            catch { }
        }

        private void PropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sub_form = new PropertiesForm(Properties);
            sub_form.ShowDialog(this);
        }
        
        private void TestDebugOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void AbortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConverterBackgroundWorker.CancelAsync();
        }

        private void TemplateTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int os = TemplateTextBox.SelectionStart;
                int ol = TemplateTextBox.SelectionLength;

                for (int index = TemplateTextBox.GetLineFromCharIndex(os); index <=
                    TemplateTextBox.GetLineFromCharIndex(os + ol); index++)
                {
                    if (index < 0 || index >= TemplateTextBox.Lines[index].Length) continue;
                    var line = TemplateTextBox.Lines[index].Trim();
                    int start = TemplateTextBox.GetFirstCharIndexFromLine(index);
                    int length = TemplateTextBox.Lines[index].Length;


                    if (line.StartsWith("#"))
                    {
                        TemplateTextBox.SelectionStart = start;
                        TemplateTextBox.SelectionLength = length;
                        TemplateTextBox.SelectionColor = Color.Green;
                    }
                    else
                    {
                        TemplateTextBox.SelectionStart = start;
                        TemplateTextBox.SelectionLength = length;
                        TemplateTextBox.SelectionColor = Color.Black;
                    }
                }

                TemplateTextBox.SelectionStart = os;
                TemplateTextBox.SelectionLength = ol;
            }
            catch { }
        }
    }
}
