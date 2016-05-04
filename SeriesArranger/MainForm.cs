using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeriesArranger
{
    public partial class MainForm : Form
    {
        private string directoryPath = "";
        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonFolderSelect_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.ShowDialog();
            directoryPath = folderBrowserDialog.SelectedPath;
            textBoxSource.Text = directoryPath;
        }

        private void buttonArrange_Click(object sender, EventArgs e)
        {
            if (directoryPath != "")
            {
                textBoxSource.Text = directoryPath;
                string[] fileEntries = Directory.GetFiles(directoryPath);
                string[] directories = Directory.GetDirectories(directoryPath);
                foreach (String file in fileEntries)
                {
                    if (file.EndsWith(".mp4") || file.EndsWith(".avi") || file.EndsWith(".mkv"))
                    {
                        textBoxSource.Text = "in!";
                    }
                }
            }
        }
    }
}
