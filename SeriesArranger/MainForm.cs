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
            if (Directory.Exists(directoryPath))
            {
                string[] seriesNames = File.ReadAllLines("seriesNames.txt");
                string[] fileEntries = Directory.GetFiles(directoryPath)
                                        .Select(file => Path.GetFileName(file)).ToArray();
                string[] directories = Directory.GetDirectories(directoryPath)
                                        .Select(dir => Path.GetFileName(dir)).ToArray();
                foreach(var name in seriesNames)
                {
                    string nameDotted = name.Replace(' ', '.');
                    string nameLower = name.ToLower();
                    string nameDottedLower = name.Replace(' ', '.').ToLower();
                    string namePrepositionsToLow = name.Replace("The", "the").Replace("In", "in").Replace("At", "at");
                    string nameDottedPrepositionsToLow = name.Replace(' ', '.').Replace("The", "the").Replace("In", "in").Replace("At", "at");
                    Parallel.ForEach(fileEntries, file =>
                    {
                        if (file.EndsWith(".mp4") || file.EndsWith(".avi") || file.EndsWith(".mkv"))
                            if (file.Contains(name) || file.Contains(nameDotted)
                                || file.Contains(nameLower) || file.Contains(nameDottedLower)
                                || file.Contains(namePrepositionsToLow) || file.Contains(nameDottedPrepositionsToLow))
                            {
                                if (File.Exists(directoryPath + "\\" + file))       // else some seriesName is part of another seriesName
                                {                                                   // exmpl "Fear The Walking Dead" & "The Walking Dead"
                                    if (!Directory.Exists(directoryPath + "\\" + name))
                                        Directory.CreateDirectory(directoryPath + "\\" + name);
                                    File.Move(directoryPath + "\\" + file, directoryPath + "\\" + name + "\\" + file);
                                }
                            }
                    });
                    Parallel.ForEach(directories, directory =>
                    {
                        if (directory.Contains(name) || directory.Contains(nameDotted)
                                || directory.Contains(nameLower) || directory.Contains(nameDottedLower)
                                || directory.Contains(namePrepositionsToLow) || directory.Contains(nameDottedPrepositionsToLow))
                        {
                            if (Directory.Exists(directoryPath + "\\" + directory)) // else some seriesName is part of another seriesName
                            {                                                       // exmpl "Fear The Walking Dead" & "The Walking Dead"
                                if (!Directory.Exists(directoryPath + "\\" + name))
                                    Directory.CreateDirectory(directoryPath + "\\" + name);
                                if (directory != name)
                                    Directory.Move(directoryPath + "\\" + directory, directoryPath + "\\" + name + "\\" + directory);
                            }
                        }
                    });
                }
                MessageBox.Show("Done!", "Series arranged");
            }
            else
                MessageBox.Show("Incorrect directory!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
