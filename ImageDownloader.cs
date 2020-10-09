using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XMLtoXLSXcvt
{
    class ImageDownloader
    {
        private static bool SaveImage(string url, string path)
        {
            try
            {
                using (WebClient client = new WebClient())
                    client.DownloadFile(new Uri(url), path);
                return true;
            }
            catch (Exception ex) 
            {
                //MessageBox.Show(ex.ToString());
                return false; 
            }
        }

        private string Prefix = "https:";
        private string XLSXPath;
        private string[] Columns;
        private float CRR, ICWR;

        public event EventHandler ProgressChanged;
        public event CancelEventHandler Cancel;

        public string Downloading { get; private set; } = "";
        public int Progress { get; private set; } = 0;

        public ImageDownloader(string xlsx, string[] columns, float crr, float icwr)
        {
            XLSXPath = xlsx;
            Columns = columns;
            CRR = crr;
            ICWR = icwr;
        }

        public void Download()
        {
            using (var Document = new ExcelDocument(XLSXPath))
            {
                Document.ColumnRow = CRR;
                Document.ImageColumnWidth = ICWR;

                Downloading = "";
                Progress = 0;

                int total_rows = Document.RowCount;
                int total_columns = Document.ColumnCount;

                List<int> column_indexes = new List<int>();
                for (int i = 0; i < total_columns; i++)
                    if (Columns.Contains(Document[i, 0]))
                        column_indexes.Add(i);

                int total_images = total_rows * column_indexes.Count;
                int current_count = 0;

                for (int y = 1; y < total_rows; y++)
                {
                    foreach (int x in column_indexes)
                    {
                        string url = Document[x, y];
                        if (url.Length > 0)
                        {
                            url = Prefix + url;
                            Downloading = url;
                            ProgressChanged?.Invoke(this, EventArgs.Empty);

                            string image_path = Path.GetFullPath("image_buffer" + Path.GetExtension(url));
                            if (SaveImage(url, image_path))
                            {
                                Document.AddImage(x, y, image_path);
                                Document[x, y] = "";
                            }
                        }
                        
                        current_count++;
                        int progress = current_count * 100 / total_images;
                        if (progress > Progress)
                        {
                            Progress = progress;
                            ProgressChanged?.Invoke(this, EventArgs.Empty);
                        }

                        var event_args = new CancelEventArgs();
                        Cancel?.Invoke(this, event_args);
                        if (event_args.Cancel)
                        {
                            Document.Save();
                            return;
                        }
                    }
                }

                Document.Save();
            }
        }
    }
}
