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
        private string directoryPath;
        private List<FileInformation> fileInfo;
        private bool shownFutureChanges;

        public class FileInformation
        {
            private string fileName;
            private string filePath;
            private DateTime currentModifiedDate;
            private DateTime embeddedFileDate;

            public string FileName
            {
                get; set;
            }

            public string FilePath
            {
                get; set;
            }

            public DateTime CurrentModifiedDate
            {
                get; set;
            }

            public DateTime EmbeddedFileDate
            {
                get; set;
            }
            
            public FileInformation(string filePath)
            {
                FilePath = filePath;
                FileName = filePath.Split('\\').Last();
                currentModifiedDate = DateTime.MinValue;
                embeddedFileDate = DateTime.MinValue;
            }
        }

        public MainForm()
        {
            InitializeComponent();

            shownFutureChanges = false;
            date = "";
            directoryPath = "";
            fileInfo = new List<FileInformation>();

            updateFileBtn.Enabled = false;
        }

        private void openMOVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getFilesFromFileDialog();
            updateFileBtn.Enabled = false;
        }

        private void BrowseFilesBtn_Click(object sender, EventArgs e)
        {
            getFilesFromFileDialog();
            updateFileBtn.Enabled = false;
        }

        private void getFilesFromFileDialog()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "MOV files|*.mov;*.txt";
                dialog.Title = ".MOV Date Corrector";
                dialog.RestoreDirectory = true;

                // Allow the user to select multiple files
                dialog.Multiselect = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    shownFutureChanges = false;

                    // Clear all previous stored fileInformation
                    if (fileInfo.Count > 0)
                    {
                        fileInfo.Clear();
                    }

                    foreach (string file in dialog.FileNames)
                    {
                        // This to ensure if they open a new FileDialog in the same instance of the program
                        // we can grab a new directory if they choose a different one.
                        if (file == dialog.FileNames[0])
                        {
                            directoryPath = file.Split('\\').Reverse().Take(2).Last();
                        }
                        try
                        {
                            fileInfo.Add(new FileInformation(file));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message);
                        }
                    }
                }
            }
            // Append all of the files file information to the InfoDisplayTB 
            infoDisplayTB.Clear();
            addFileInfoToInfoTB();
        }

        /**
         * Takes each file that was chosen from the OpenFileDialog and parses the Date/Time from the
         * file and sets the files Modified Date to the date that was parsed.
         */
        private void updateFileBtn_Click(object sender, EventArgs e)
        {
            if (fileInfo.Count <= 0)
                return;

            foreach (FileInformation file in fileInfo)
            {
                File.SetLastWriteTime(file.FilePath, file.EmbeddedFileDate);
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

        private void getEmbeddedFileDate(FileInformation file, byte[] fileChunk)
        {   
            DateTime embeddedFileDate;

            getFileChunk(file.FilePath, ref fileChunk);
            embeddedFileDate = getDateFromFileChunk(ref fileChunk);

            if (embeddedFileDate != DateTime.MinValue)
            {
                //File.SetLastWriteTime(file.FilePath, fileDate);
                file.EmbeddedFileDate = embeddedFileDate;
            }
            else
            {
                infoDisplayTB.AppendText("Error parsing file: " + file.FileName +". The date in the file does not match \"yyyy-MM-dd HH:mm:ss\" format");
            }
        }

        private void getCurrentModifiedDate(FileInformation file)
        {          
            file.CurrentModifiedDate = File.GetLastWriteTime(file.FilePath);      
        }

        /**
         * Appends a File information to the FileInfo text box. File information includes:
         * File Name, Current Modified DateTime, and if applicable - Embedded File Date.
         */
        private void addFileInfoToInfoTB()
        {
            foreach (FileInformation file in fileInfo)
            {
                infoDisplayTB.AppendText("---------------------------------------------------\n");
                infoDisplayTB.AppendText("File: " + file.FileName + "\n");
                if (file.CurrentModifiedDate != DateTime.MinValue)
                {
                    infoDisplayTB.AppendText("Current Modified DateTime: " + file.CurrentModifiedDate + "\n");
                }
                
                if (file.EmbeddedFileDate != DateTime.MinValue)
                {
                    infoDisplayTB.AppendText("Embedded File Date: " + file.EmbeddedFileDate + "\n");
                }
            }
        }

        private void PreviewChangeBtn_Click(object sender, EventArgs e)
        {

            if (fileInfo.Count <= 0)
            {
                infoDisplayTB.Text = "Please select a file (ctrl+O) or from the dropdown menu first.";
                return;
            }

            infoDisplayTB.Clear();

            if (shownFutureChanges == false)
            {                
                byte[] fileChunk = new byte[FILECHUNKSIZE];

                foreach (FileInformation file in fileInfo)
                {
                    getCurrentModifiedDate(file);
                    getEmbeddedFileDate(file, fileChunk);
                }
                                
                shownFutureChanges = true;
            }
            
            addFileInfoToInfoTB();

            // User must preview changes before they are allowed to update files.
            updateFileBtn.Enabled = true;
        }
    }
}


/**
 * Adds File information to the fileInfo Dictionary. File information includes: File Name,
 * and Current Modified DateTime
 * @param filePath - The absolute path for the file.
*/
/*
private void addCurrentFileInfo(string filePath)
{
   // FOR DIRECTORIES PROBABLY. THIS IS USED IF A FILE/DIRECTORY ENDS WITH / Ex. Something/MyDocs/ WILL RETURN MyDocs
   //string fileName = file.Substring(0, file.LastIndexOf('\\')).Split('\\').Last();

   string fileName = filePath.Split('\\').Last();
   DateTime modifiedDate = File.GetLastWriteTime(filePath);
   FileInformation fi = new FileInformation();

   fileInfo.Add(fileName, fi);
}
*/

//private void addEmbeddedFileDate(string fileName, DateTime embeddedDate)
//{
//    /*
//    if (!fileInfo.Any(file => file.FileName == fileName))
//    {
//        MessageBox.Show("Error: fileName not found while trying to write Embedded File Date", "Error",
//            MessageBoxButtons.OK, MessageBoxIcon.Error);
//        return;
//    }
//    */

//    int index = fileInfo.FindIndex(file => file.FileName == fileName);
//    if (index == -1)
//    {
//        MessageBox.Show("Error: fileName not found while trying to write Embedded File Date", "Error",
//            MessageBoxButtons.OK, MessageBoxIcon.Error);
//        return;
//    }

//    DateTime embeddedDate =
//}


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