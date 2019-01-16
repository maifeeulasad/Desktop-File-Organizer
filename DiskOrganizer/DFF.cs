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

        private static Dictionary<string, List<string>> hashes;// = new Dictionary<string, List<string>> { };
        private static Dictionary<string, List<string>> duplicateHashes;// = new Dictionary<string, List<string>> { };


        private static long totalSize = 0;
        private static long currentSize = 1;
        

        public DFF(string path, System.Windows.Controls.ProgressBar progressBar,Window window)
        {

            hashes = new Dictionary<string, List<string>> { };
            duplicateHashes = new Dictionary<string, List<string>> { };

            totalSize = DirSize(path);
            Travarse(path,progressBar,window);
            //GetDuplicates();
        }

        
        public static Dictionary<string, List<string>> GetDuplicates()
        {
            return duplicateHashes;
        }

        public static void PrintResult()
        {

            Debug.WriteLine("++++++++++++++++++++++++++++++");
            foreach (string key in hashes.Keys)
            {
                Debug.WriteLine(key);
                
                try
                {
                    List<string> xxx = duplicateHashes[key];
                    foreach (string y in xxx)
                    {
                        Debug.WriteLine("---" + y);
                    }
                }
                catch(Exception e)
                {

                }
                
                
            }

            Debug.WriteLine("++++++++++++++++++++++++++++++");
        }


        private static long DirSize(string path)
        {
            return DirSize(new DirectoryInfo(path));
        }

        private static long DirSize(DirectoryInfo d)
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

        private static string myCurrentKey = "";


        

        private static void MoveIndex(int dir)
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


        private static long GetFileSize(string file)
        {

            return new FileInfo(file).Length;
        }

        private static void GetOnlyDuplicates()
        {
            foreach (KeyValuePair<String, List<String>> kvp in hashes)
            {


                if (kvp.Value.Count > 1)
                {

                    List<String> locations = kvp.Value;
                    Console.WriteLine("\n" + kvp.Key + "\n");

                    foreach (String location in locations)
                    {

                        Console.WriteLine("          -------------   " + location);

                    }


                    duplicateHashes[kvp.Key] = kvp.Value;

                }


            }
        }


        private static void Travarse(string workingDirectory, System.Windows.Controls.ProgressBar progressBar,Window window)
        {
            try
            {

                string[] files = Directory.GetFiles(workingDirectory);
                string[] subDirectories = Directory.GetDirectories(workingDirectory);


                foreach (string file in files)
                {


                    CalculateMD5(file);

                    Debug.WriteLine(file);

                    currentSize += GetFileSize(file);


                    window.Dispatcher.Invoke((Action)(() =>
                    {
                        progressBar.Value = (100 * currentSize) / totalSize;
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


        private static void CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);

                    

                    string hashString = BitConverter.ToString(hash);


                    try
                    {
                        Debug.WriteLine(hashString);
                        hashes[hashString].Add(filename);


                    }
                    catch (NullReferenceException nre)
                    {
                        Debug.WriteLine("^^^^^^^^^^^^^^null");
                        hashes[hashString] = new List<string> { };
                        hashes[hashString].Add(filename);
                    }
                    catch (KeyNotFoundException knfe)
                    {
                        Debug.WriteLine("^^^^^^^^^keyError");
                        hashes[hashString] = new List<string> { };
                        hashes[hashString].Add(filename);

                        Debug.WriteLine("\n++++++++++++++++++++\n");
                        /*
                        try
                        {
                            List<string> xxx = hashes[hashString];
                            foreach (string y in xxx)
                            {
                                Debug.WriteLine("---" + y);
                            }
                        }
                        
                        catch (Exception e)
                        {

                        }

                        Debug.WriteLine("\n++++++++++++++++++++\n");
                        */
                    }


                    PrintResult();

                    /*
                    try
                    {
                        List<string> xxx = hashes[hashString];
                        foreach (string y in xxx)
                        {
                            Debug.WriteLine("---" + y);
                        }
                    }
                    catch (Exception e)
                    {

                    }
                    */


                }
            }
        }



    }
}
