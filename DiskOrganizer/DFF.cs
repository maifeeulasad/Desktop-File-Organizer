using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows;

namespace DiskOrganizer
{
    public class DFF
    {
        private long totalSize = 0;
        private long currentSize = 1;
        

        public DFF(string path, System.Windows.Controls.ProgressBar progressBar,Window window)
        {
            totalSize = DirSize(path);
            Travarse(path,progressBar,window);
        }

        
        public Dictionary<string, List<string>> GetDuplicates()
        {
            return duplicateHashes;
        }

        public void PrintResult()
        {

            foreach(string key in duplicateHashes.Keys)
            {
                Debug.WriteLine(key+"\n-----------\n");
                foreach(string y in duplicateHashes[key])
                {
                    Debug.WriteLine("---" + y);
                }
                
            }

        }


        private long DirSize(string path)
        {
            return DirSize(new DirectoryInfo(path));
        }

        private long DirSize(DirectoryInfo d)
        {

            long size = 0;
            try
            {
                FileInfo[] fis = d.GetFiles();
                foreach (FileInfo fi in fis)
                {
                    size += fi.Length;
                }
                DirectoryInfo[] dis = d.GetDirectories();
                foreach (DirectoryInfo di in dis)
                {
                    size += DirSize(di);
                }
            }
            catch(Exception e)
            {

            }
            return size;
        }

        private string myCurrentKey = "";


        private Dictionary<string, List<string>> hashes = new Dictionary<string, List<string>> { };
        private Dictionary<string, List<string>> duplicateHashes = new Dictionary<string, List<string>> { };


        private void MoveIndex(int dir)
        {

            List<string> keys = new List<string>(duplicateHashes.Keys);
            int newIndex = keys.IndexOf(myCurrentKey) - dir;
            if (newIndex < 0)
            {
                newIndex = duplicateHashes.Count - 1;
            }
            else if (newIndex > duplicateHashes.Count - 1)
            {
                newIndex = 0;
            }

            myCurrentKey = keys[newIndex];
        }


        private long GetFileSize(string file)
        {

            return new System.IO.FileInfo(file).Length;
        }

        private void Travarse(string workingDirectory, System.Windows.Controls.ProgressBar progressBar,Window window)
        {
            try
            {

                string[] files = Directory.GetFiles(workingDirectory);
                string[] subDirectories = Directory.GetDirectories(workingDirectory);


                foreach (string file in files)
                {


                    CalculateMD5(file);

                    currentSize += GetFileSize(file);


                    window.Dispatcher.Invoke((Action)(() =>
                    {
                        progressBar.Value = 50;
                    }));




                }


                foreach (string subDirectory in subDirectories)
                {

                    
                    Travarse(subDirectory,progressBar,window);
                }
            }
            catch (UnauthorizedAccessException uae)
            {

            }


        }


        string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);

                    

                    string hashstring = BitConverter.ToString(hash);


                    try
                    {
                        hashes[hashstring].Add(filename);


                    }
                    catch (NullReferenceException nre)
                    {
                        hashes[hashstring] = new List<string> { };
                        hashes[hashstring].Add(filename);
                    }
                    catch (KeyNotFoundException knfe)
                    {

                        hashes[hashstring] = new List<string> { };
                        hashes[hashstring].Add(filename);
                    }

                    return hashstring;


                }
            }
        }



    }
}
