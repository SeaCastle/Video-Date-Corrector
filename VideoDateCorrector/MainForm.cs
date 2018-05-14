using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace VideoDateCorrector
{
    public partial class MainForm : Form
    {
        private const int FILECHUNKSIZE = 2048;
        private string date;
        private List<string> filePaths = new List<string>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void openMOVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "MOV files|*.mov;*.txt";
                dialog.InitialDirectory = @"C:\";
                dialog.RestoreDirectory = true;
                dialog.Title = ".MOV Date Corrector";

                // Allow the user to select multiple files
                dialog.Multiselect = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string file in dialog.FileNames)
                    {
                        try
                        {
                            filePaths.Add(file);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message);
                        }
                    }
                }
            }
                
        }

        private void openMOVDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string folderName = dialog.SelectedPath;

                    infoDisplayTB.Text = "Directory Opened: " + folderName;
                }
            }
        }

        
        private void updateDirectoryBtn_Click(object sender, EventArgs e)
        {
            
            byte[] fileChunk = new byte[FILECHUNKSIZE];
            DateTime fileDate;

            if (filePaths.Count <= 0)
            {
                infoDisplayTB.Text = "Please select a file (ctrl+O) or from the dropdown menu first.";
            }
            else
            {
                infoDisplayTB.Clear();

                foreach (String file in filePaths)
                {

                    getFileChunk(file, ref fileChunk);
                    fileDate = getDateFromFileChunk(ref fileChunk);

                    if (fileDate != DateTime.MinValue)
                    {
                        File.SetLastWriteTime(file, fileDate);
                    }
                    else
                    {
                        infoDisplayTB.AppendText("Error parsing file. The date in the file does not match \"yyyy-MM-dd HH:mm:ss\" format");
                    }
                }
                
            }                        
        }

        /**
         * Opens a FileStream and gets the last chunk of a file and reads it into a 
         * byte array. The size of the chunk is specified by FILECHUNKSIZE in MainForm.cs
         * @param filePath - The absolute path for the file to be opened in the FileStream.
         * @param fileChunk - Byte array that is used to hold the binary data from the file. 
         */ 
        private void getFileChunk(string filePath, ref byte[] fileChunk)
        {           
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                // We are only interested in the last [FILECHUNKSIZE] of the file.
                // This will likely be 2kb unless otherwise specially specified
                if (stream.Length > FILECHUNKSIZE)
                {
                    stream.Seek(-FILECHUNKSIZE, SeekOrigin.End);
                }

                // Read the last 2kb of the file into memory for faster operations.
                stream.Read(fileChunk, 0, FILECHUNKSIZE);
            }
        }

        // TODO: Error Checking
        /**
         * Reads the fileChunk byte array backwards until we find the second "free" string in the file. Once we 
         * find that we grab the date string that is right in front of it and convert it to a DateTime object.
         * @param fileChunk - byte array of a file we are trying to iterate over to find two "free" strings.
         * @return - DateTime object holding the "yyyy-MM-dd HH-mm-ss" format parsed from the fileChunk.
         */
        private DateTime getDateFromFileChunk(ref byte[] fileChunk)
        {
            DateTime fileDate = new DateTime();
            int count = 0; ///< This is to keep track how many times we find the word "free"
            char[] delimiters = { 'T', '-', ':' };

            for (int i = fileChunk.Length - 1; i > 0; i--)
            {
                if ((char)fileChunk[i] == 'f')
                {
                    if (String.Compare(System.Text.Encoding.UTF8.GetString(fileChunk, i, 4), "free") == 0)
                    {
                        count++;
                    }
                }
                else if (count == 2)
                {
                    date = System.Text.Encoding.UTF8.GetString(fileChunk, (i - 27), 19);

                    // date has a T in between day and hour in the string, replace with a space.
                    // Ex. "yyyy-MM-ddTHH:mm:ss" -> "yyyy-MM-dd HH:mm:ss"
                    date = date.Replace("T", " ");

                    DateTime.TryParseExact(date, "yyyy-MM-dd HH:mm:ss", 
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None,
                        out fileDate);

                    break;
                }
            }
            return fileDate;
        }
    }
}


// Read the file backwards, byte by byte using SeekOrigin.Current.
//long position;
//stream.Seek(0, SeekOrigin.End);
//for (position = 0; position < stream.Length; position++)
//{
//    stream.Seek(-1, SeekOrigin.Current);
//    output[0] = (byte)stream.ReadByte();
//    if (output[0] == 'f')
//    {
//        count++;
//        infoDisplayTB.AppendText(count.ToString() + "\n");
//    }
//    stream.Seek(-1, SeekOrigin.Current);
//}