namespace XMLtoXLSXcvt
{
    partial class ConverterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.OpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveDialog = new System.Windows.Forms.SaveFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.XMLTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.XLSXTextBox = new System.Windows.Forms.TextBox();
            this.ConvertButton = new System.Windows.Forms.Button();
            this.ResolveImagesButton = new System.Windows.Forms.Button();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.ConverterBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.TopMenuStrip = new System.Windows.Forms.MenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ChooseXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ChooseXLSXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.видToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TestDebugOnlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AbortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ImageTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ResolverBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.ProgressLabel = new System.Windows.Forms.Label();
            this.BrowseXMLButton = new System.Windows.Forms.Button();
            this.BrowseXLSXButton = new System.Windows.Forms.Button();
            this.StopImagesButton = new System.Windows.Forms.Button();
            this.TemplateTextBox = new System.Windows.Forms.RichTextBox();
            this.TopMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // OpenDialog
            // 
            this.OpenDialog.DefaultExt = "xml";
            this.OpenDialog.Title = "Select XML file";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Шаблон поиска";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Путь к XML";
            // 
            // XMLTextBox
            // 
            this.XMLTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.XMLTextBox.Location = new System.Drawing.Point(12, 51);
            this.XMLTextBox.Name = "XMLTextBox";
            this.XMLTextBox.Size = new System.Drawing.Size(700, 20);
            this.XMLTextBox.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Путь к XLSX";
            // 
            // XLSXTextBox
            // 
            this.XLSXTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.XLSXTextBox.Location = new System.Drawing.Point(12, 90);
            this.XLSXTextBox.Name = "XLSXTextBox";
            this.XLSXTextBox.Size = new System.Drawing.Size(700, 20);
            this.XLSXTextBox.TabIndex = 9;
            // 
            // ConvertButton
            // 
            this.ConvertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ConvertButton.Location = new System.Drawing.Point(12, 502);
            this.ConvertButton.Name = "ConvertButton";
            this.ConvertButton.Size = new System.Drawing.Size(107, 23);
            this.ConvertButton.TabIndex = 13;
            this.ConvertButton.Text = "Сконвертировать";
            this.ConvertButton.UseVisualStyleBackColor = true;
            this.ConvertButton.Click += new System.EventHandler(this.ConvertButton_Click);
            // 
            // ResolveImagesButton
            // 
            this.ResolveImagesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ResolveImagesButton.Location = new System.Drawing.Point(125, 502);
            this.ResolveImagesButton.Name = "ResolveImagesButton";
            this.ResolveImagesButton.Size = new System.Drawing.Size(140, 23);
            this.ResolveImagesButton.TabIndex = 14;
            this.ResolveImagesButton.Text = "Загрузить изображения";
            this.ResolveImagesButton.UseVisualStyleBackColor = true;
            this.ResolveImagesButton.Click += new System.EventHandler(this.ResolveImagesButton_Click);
            // 
            // ProgressBar
            // 
            this.ProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressBar.Location = new System.Drawing.Point(404, 502);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(407, 23);
            this.ProgressBar.TabIndex = 15;
            // 
            // ConverterBackgroundWorker
            // 
            this.ConverterBackgroundWorker.WorkerReportsProgress = true;
            this.ConverterBackgroundWorker.WorkerSupportsCancellation = true;
            this.ConverterBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ConverterBackgroundWorker_DoWork);
            this.ConverterBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.ConverterBackgroundWorker_ProgressChanged);
            this.ConverterBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ConverterBackgroundWorker_RunWorkerCompleted);
            // 
            // TopMenuStrip
            // 
            this.TopMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.видToolStripMenuItem,
            this.ToolsToolStripMenuItem});
            this.TopMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.TopMenuStrip.Name = "TopMenuStrip";
            this.TopMenuStrip.Size = new System.Drawing.Size(823, 24);
            this.TopMenuStrip.TabIndex = 16;
            this.TopMenuStrip.Text = "menuStrip1";
            // 
            // FileToolStripMenuItem
            // 
            this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ChooseXMLToolStripMenuItem,
            this.ChooseXLSXToolStripMenuItem,
            this.LoadConfigToolStripMenuItem,
            this.SaveConfigToolStripMenuItem});
            this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            this.FileToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.FileToolStripMenuItem.Text = "Файл";
            // 
            // ChooseXMLToolStripMenuItem
            // 
            this.ChooseXMLToolStripMenuItem.Name = "ChooseXMLToolStripMenuItem";
            this.ChooseXMLToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.ChooseXMLToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.ChooseXMLToolStripMenuItem.Text = "Выбрать XML";
            this.ChooseXMLToolStripMenuItem.Click += new System.EventHandler(this.ChooseXMLToolStripMenuItem_Click);
            // 
            // ChooseXLSXToolStripMenuItem
            // 
            this.ChooseXLSXToolStripMenuItem.Name = "ChooseXLSXToolStripMenuItem";
            this.ChooseXLSXToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.ChooseXLSXToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.ChooseXLSXToolStripMenuItem.Text = "Выбрать XLSX";
            this.ChooseXLSXToolStripMenuItem.Click += new System.EventHandler(this.ChooseXLSXToolStripMenuItem_Click);
            // 
            // LoadConfigToolStripMenuItem
            // 
            this.LoadConfigToolStripMenuItem.Name = "LoadConfigToolStripMenuItem";
            this.LoadConfigToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.LoadConfigToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.LoadConfigToolStripMenuItem.Text = "Загрузить конфиг";
            this.LoadConfigToolStripMenuItem.Click += new System.EventHandler(this.LoadConfigToolStripMenuItem_Click);
            // 
            // SaveConfigToolStripMenuItem
            // 
            this.SaveConfigToolStripMenuItem.Name = "SaveConfigToolStripMenuItem";
            this.SaveConfigToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.SaveConfigToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.SaveConfigToolStripMenuItem.Text = "Сохранить конфиг";
            this.SaveConfigToolStripMenuItem.Click += new System.EventHandler(this.SaveConfigToolStripMenuItem_Click);
            // 
            // видToolStripMenuItem
            // 
            this.видToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PropertiesToolStripMenuItem,
            this.TestDebugOnlyToolStripMenuItem});
            this.видToolStripMenuItem.Name = "видToolStripMenuItem";
            this.видToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.видToolStripMenuItem.Text = "Вид";
            // 
            // PropertiesToolStripMenuItem
            // 
            this.PropertiesToolStripMenuItem.Name = "PropertiesToolStripMenuItem";
            this.PropertiesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.PropertiesToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.PropertiesToolStripMenuItem.Text = "Настройки";
            this.PropertiesToolStripMenuItem.Click += new System.EventHandler(this.PropertiesToolStripMenuItem_Click);
            // 
            // TestDebugOnlyToolStripMenuItem
            // 
            this.TestDebugOnlyToolStripMenuItem.Name = "TestDebugOnlyToolStripMenuItem";
            this.TestDebugOnlyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.TestDebugOnlyToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.TestDebugOnlyToolStripMenuItem.Text = "Test (Debug only)";
            this.TestDebugOnlyToolStripMenuItem.Click += new System.EventHandler(this.TestDebugOnlyToolStripMenuItem_Click);
            // 
            // ToolsToolStripMenuItem
            // 
            this.ToolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AbortToolStripMenuItem});
            this.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem";
            this.ToolsToolStripMenuItem.Size = new System.Drawing.Size(95, 20);
            this.ToolsToolStripMenuItem.Text = "Инструменты";
            // 
            // AbortToolStripMenuItem
            // 
            this.AbortToolStripMenuItem.Name = "AbortToolStripMenuItem";
            this.AbortToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.AbortToolStripMenuItem.Text = "Прервать";
            this.AbortToolStripMenuItem.Click += new System.EventHandler(this.AbortToolStripMenuItem_Click);
            // 
            // ImageTextBox
            // 
            this.ImageTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ImageTextBox.Location = new System.Drawing.Point(560, 129);
            this.ImageTextBox.Multiline = true;
            this.ImageTextBox.Name = "ImageTextBox";
            this.ImageTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ImageTextBox.Size = new System.Drawing.Size(251, 367);
            this.ImageTextBox.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(557, 113);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(254, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Названия столбцов для подгрузки изображений";
            // 
            // ResolverBackgroundWorker
            // 
            this.ResolverBackgroundWorker.WorkerReportsProgress = true;
            this.ResolverBackgroundWorker.WorkerSupportsCancellation = true;
            this.ResolverBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ResolverBackgroundWorker_DoWork);
            this.ResolverBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.ResolverBackgroundWorker_ProgressChanged);
            this.ResolverBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ResolverBackgroundWorker_RunWorkerCompleted);
            // 
            // ProgressLabel
            // 
            this.ProgressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ProgressLabel.AutoSize = true;
            this.ProgressLabel.BackColor = System.Drawing.Color.Transparent;
            this.ProgressLabel.Location = new System.Drawing.Point(411, 507);
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(0, 13);
            this.ProgressLabel.TabIndex = 19;
            // 
            // BrowseXMLButton
            // 
            this.BrowseXMLButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseXMLButton.Location = new System.Drawing.Point(718, 51);
            this.BrowseXMLButton.Name = "BrowseXMLButton";
            this.BrowseXMLButton.Size = new System.Drawing.Size(93, 20);
            this.BrowseXMLButton.TabIndex = 25;
            this.BrowseXMLButton.Text = "Выбрать файл";
            this.BrowseXMLButton.UseVisualStyleBackColor = true;
            this.BrowseXMLButton.Click += new System.EventHandler(this.ChooseXMLToolStripMenuItem_Click);
            // 
            // BrowseXLSXButton
            // 
            this.BrowseXLSXButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseXLSXButton.Location = new System.Drawing.Point(718, 90);
            this.BrowseXLSXButton.Name = "BrowseXLSXButton";
            this.BrowseXLSXButton.Size = new System.Drawing.Size(93, 20);
            this.BrowseXLSXButton.TabIndex = 26;
            this.BrowseXLSXButton.Text = "Выбрать файл";
            this.BrowseXLSXButton.UseVisualStyleBackColor = true;
            this.BrowseXLSXButton.Click += new System.EventHandler(this.ChooseXLSXToolStripMenuItem_Click);
            // 
            // StopImagesButton
            // 
            this.StopImagesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.StopImagesButton.Enabled = false;
            this.StopImagesButton.Location = new System.Drawing.Point(271, 502);
            this.StopImagesButton.Name = "StopImagesButton";
            this.StopImagesButton.Size = new System.Drawing.Size(127, 23);
            this.StopImagesButton.TabIndex = 27;
            this.StopImagesButton.Text = "Остановить загрузку";
            this.StopImagesButton.UseVisualStyleBackColor = true;
            this.StopImagesButton.Click += new System.EventHandler(this.StopImagesButton_Click);
            // 
            // TemplateTextBox
            // 
            this.TemplateTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TemplateTextBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TemplateTextBox.Location = new System.Drawing.Point(12, 129);
            this.TemplateTextBox.Name = "TemplateTextBox";
            this.TemplateTextBox.Size = new System.Drawing.Size(542, 367);
            this.TemplateTextBox.TabIndex = 29;
            this.TemplateTextBox.Text = "";
            this.TemplateTextBox.TextChanged += new System.EventHandler(this.TemplateTextBox_TextChanged);
            // 
            // ConverterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 537);
            this.Controls.Add(this.TemplateTextBox);
            this.Controls.Add(this.StopImagesButton);
            this.Controls.Add(this.BrowseXLSXButton);
            this.Controls.Add(this.BrowseXMLButton);
            this.Controls.Add(this.ProgressLabel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ImageTextBox);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.ResolveImagesButton);
            this.Controls.Add(this.ConvertButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.XLSXTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.XMLTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TopMenuStrip);
            this.MainMenuStrip = this.TopMenuStrip;
            this.Name = "ConverterForm";
            this.Text = "XMLtoXLSXcvt";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConverterForm_FormClosing);
            this.Load += new System.EventHandler(this.ConverterForm_Load);
            this.TopMenuStrip.ResumeLayout(false);
            this.TopMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog OpenDialog;
        private System.Windows.Forms.SaveFileDialog SaveDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox XMLTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox XLSXTextBox;
        private System.Windows.Forms.Button ConvertButton;
        private System.Windows.Forms.Button ResolveImagesButton;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.ComponentModel.BackgroundWorker ConverterBackgroundWorker;
        private System.Windows.Forms.MenuStrip TopMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ChooseXMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ChooseXLSXToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LoadConfigToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveConfigToolStripMenuItem;
        private System.Windows.Forms.TextBox ImageTextBox;
        private System.Windows.Forms.Label label6;
        private System.ComponentModel.BackgroundWorker ResolverBackgroundWorker;
        private System.Windows.Forms.Label ProgressLabel;
        private System.Windows.Forms.ToolStripMenuItem видToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PropertiesToolStripMenuItem;
        private System.Windows.Forms.Button BrowseXMLButton;
        private System.Windows.Forms.Button BrowseXLSXButton;
        private System.Windows.Forms.Button StopImagesButton;
        private System.Windows.Forms.ToolStripMenuItem TestDebugOnlyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AbortToolStripMenuItem;
        private System.Windows.Forms.RichTextBox TemplateTextBox;
    }
}

