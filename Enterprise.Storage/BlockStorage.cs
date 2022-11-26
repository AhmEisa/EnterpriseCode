using System;
using System.IO;
using System.Threading;

namespace Enterprise.Storage
{
    public class BlockStorage
    {
        public void SplitFile(string inputFile, int chunkSize, string path)
        {
            const int BUFFER_SIZE = 20 * 1024;
            byte[] buffer = new byte[BUFFER_SIZE];

            using (Stream input = File.OpenRead(inputFile))
            {
                int index = 0;
                while (input.Position < input.Length)
                {
                    using (Stream output = File.Create(path + "\\" + index))
                    {
                        int remaining = chunkSize, bytesRead;
                        while (remaining > 0 && (bytesRead = input.Read(buffer, 0,
                                Math.Min(remaining, BUFFER_SIZE))) > 0)
                        {
                            output.Write(buffer, 0, bytesRead);
                            remaining -= bytesRead;
                        }
                    }
                    index++;
                    Thread.Sleep(500); // experimental; perhaps try it
                }
            }
        }
        public bool SplitFile(string sourceFile, int nNoofFiles)
        {
            bool Split = false;
            try
            {
                FileStream fs = new FileStream(sourceFile, FileMode.Open, FileAccess.Read);
                int SizeofEachFile = (int)Math.Ceiling((double)fs.Length / nNoofFiles);
                for (int i = 0; i < nNoofFiles; i++)
                {
                    string baseFileName = Path.GetFileNameWithoutExtension(sourceFile);
                    string Extension = Path.GetExtension(sourceFile);
                    FileStream outputFile = new FileStream(Path.GetDirectoryName(sourceFile) + "\\" + baseFileName + "." +
                        i.ToString().PadLeft(5, Convert.ToChar("0")) + Extension + ".tmp", FileMode.Create, FileAccess.Write);
                    string mergeFolder = Path.GetDirectoryName(sourceFile);
                    int bytesRead = 0;
                    byte[] buffer = new byte[SizeofEachFile];
                    if ((bytesRead = fs.Read(buffer, 0, SizeofEachFile)) > 0)
                    {
                        outputFile.Write(buffer, 0, bytesRead);
                    }
                    outputFile.Close();
                }
                fs.Close();
            }
            catch (Exception Ex)
            {
                throw new ArgumentException(Ex.Message);
            }
            return Split;
        }

        public bool MergeFile(string inputfoldername1, string saveFileFolder)
        {
            bool Output = false;
            try
            {
                string[] tmpfiles = Directory.GetFiles(inputfoldername1, "*.tmp");
                FileStream outPutFile = null;
                string PrevFileName = "";
                foreach (string tempFile in tmpfiles)
                {
                    string fileName = Path.GetFileNameWithoutExtension(tempFile);
                    string baseFileName = fileName.Substring(0, fileName.IndexOf(Convert.ToChar(".")));
                    string extension = Path.GetExtension(fileName);
                    if (!PrevFileName.Equals(baseFileName))
                    {
                        if (outPutFile != null)
                        {
                            outPutFile.Flush();
                            outPutFile.Close();
                        }
                        outPutFile = new FileStream(saveFileFolder + "\\" + baseFileName + extension, FileMode.OpenOrCreate, FileAccess.Write);
                    }
                    int bytesRead = 0;
                    byte[] buffer = new byte[1024];
                    FileStream inputTempFile = new FileStream(tempFile, FileMode.OpenOrCreate, FileAccess.Read);
                    while ((bytesRead = inputTempFile.Read(buffer, 0, 1024)) > 0)
                        outPutFile.Write(buffer, 0, bytesRead);
                    inputTempFile.Close();
                    File.Delete(tempFile);
                    PrevFileName = baseFileName;
                }
                outPutFile.Close();
            }
            catch { }
            return Output;
        }

        //Compress and extract files: https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-compress-and-extract-files

    }
}
