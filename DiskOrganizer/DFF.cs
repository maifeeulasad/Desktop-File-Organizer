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

        public bool done = false;

        private Dictionary<string, List<string>> hashes;
        private Dictionary<string, List<string>> duplicateHashes;

        public Dictionary<string, List<string>> GetDuplicates()
        {
            return duplicateHashes;
        }


        private long totalSize = 0;
        private long currentSize = 1;


        public static float progress = 0;
        public static string currentPath = "";


        public void PerformOperation(string path)
        {

            currentPath = "";
            totalSize = 1;
            currentSize = 0;
            progress = 0;
            done = false;

            hashes = new Dictionary<string, List<string>> { };
            duplicateHashes = new Dictionary<string, List<string>> { };

            totalSize = DirSize(path);


            Travarse(path);
            done = true;
            //PrintResult();
            GetOnlyDuplicates();
            //PrintDuplicates();
        }

        public DFF(string path)
        {
            PerformOperation(path);
        }

        

        public void PrintResult()
        {

            Debug.WriteLine("-------------------------------");

            Debug.WriteLine("\nGenuine\n");


            foreach (KeyValuePair<String, List<String>> kvp in hashes)
            {
                List<String> locations = kvp.Value;
                Console.WriteLine("\n" + kvp.Key + "\n");

                foreach (String location in locations)
                {

                    Console.WriteLine("!---" + location);

                }
            }
            Debug.WriteLine("++++++++++++++++++++++++++++++");
        }


        public void PrintDuplicates()
        {



            Debug.WriteLine("-------------------------------");

            Debug.WriteLine("\nDuplicates\n");

            foreach (KeyValuePair<String, List<String>> kvp in duplicateHashes)

            {

                List<String> locations = kvp.Value;
                Console.WriteLine("\n" + kvp.Key + "\n");

                foreach (String location in locations)
                {

                    Console.WriteLine("|***" + location);

                }


            }

            Debug.WriteLine("++++++++++++++++++++++++++++++");
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
            catch (Exception e)
            {

            }
            return size;
        }

        private string myCurrentKey = "";




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

            return new FileInfo(file).Length;
        }

        private void GetOnlyDuplicates()
        {
            foreach (KeyValuePair<String, List<String>> kvp in hashes)
            {


                if (kvp.Value.Count > 1)
                {

                    List<String> locations = kvp.Value;

                    duplicateHashes[kvp.Key] = kvp.Value;

                }


            }
        }


        private void Travarse(string workingDirectory)
        {
            try
            {

                string[] files = Directory.GetFiles(workingDirectory);
                string[] subDirectories = Directory.GetDirectories(workingDirectory);


                foreach (string file in files)
                {

                    currentPath = file;
                    CalculateMD5(file);

                    Debug.WriteLine(file);

                    currentSize += GetFileSize(file);


                    progress = (100 * currentSize) / totalSize;

                    



                }


                foreach (string subDirectory in subDirectories)
                {


                    Travarse(subDirectory);
                }
            }
            catch (UnauthorizedAccessException uae)
            {

            }


        }

        private void CalculateMD5(string filename)
        {




            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);



                    string hashString = BitConverter.ToString(hash);


                    try
                    {
                        //Debug.WriteLine(hashString);
                        hashes[hashString].Add(filename);


                    }
                    catch (NullReferenceException nre)
                    {
                        //Debug.WriteLine("^^^^^^^^^^^^^^null");
                        hashes[hashString] = new List<string> { };
                        hashes[hashString].Add(filename);
                    }
                    catch (KeyNotFoundException knfe)
                    {
                        //Debug.WriteLine("^^^^^^^^^keyError");

                        hashes[hashString] = new List<string> { };
                        hashes[hashString].Add(filename);



                    }



                }
            }

        }



    }
}
