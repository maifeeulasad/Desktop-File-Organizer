using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskOrganizer
{
    class Util
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string sName = "dosmua";

        public static void moveFile(string path)
        {
            string fileName = path.Substring(path.LastIndexOf('\\')+1);

            string type = fileName.Substring(fileName.IndexOf('.')+1);
            string dest = targetPath + "\\" + sName + "\\" + type + "\\" + fileName;
            if(File.Exists(dest))
            {

                string nName = fileName.Replace(".", RandomString(50) + ".");

                Debug.WriteLine(targetPath + "\\" + sName + "\\" + type + "\\" + nName);

                File.Move(path, targetPath + "\\" + sName + "\\" + type + "\\" +nName);

            }
            else
            {
                File.Move(path, targetPath + "\\" + sName + "\\" + type + "\\" + fileName);

            }
            //Debug.WriteLine(targetPath + "\\" + sName + "\\" + type + "\\" + fileName);


        }


        public static string targetPath = "";


        public static string[] textBoxToArray(string data)
        {

            data = data.ToLower();
            string[] tem = data.Split();
            string[] res = new string[0];
            

            foreach(string zz in tem)
            {
                if(zz.Equals(""))
                {
                    continue;
                }
                else
                {
                    Array.Resize(ref res, res.Length + 1);
                    res[res.Length - 1] = zz;
                }
            }

            return res;

        }


        public static long Processed = 0;
        public static long TotalSize = 0;

        public static int PowerCount = 0;

        public static string[] toProcess;
        public static string[] notToProcess;

        public static void preOrganize(string path)
        {
            foreach (string type in toProcess)
            {
                string p = path + "\\dosmua\\" + type;

                Debug.WriteLine(p);
                CreateDirectory(p);
            }
        }
        


        public static void organize(string path)
        {

            

            //try
            {

                string[] files = Directory.GetFiles(path);
                
                foreach(string xxxx in files)
                {
                    string x = xxxx.ToLower();
                    foreach(string type in toProcess)
                    {
                        if(x.EndsWith("."+type))
                        {

                            Debug.WriteLine(x);
                            moveFile(x);
                            //moveFile(x, x + "\\dosmua\\" + type);

                            //File.Move(x, path + "\\dosmua\\" + type);
                            break;
                        }
                    }
                }


                string[] direc = Directory.GetDirectories(path);
                foreach(string d in direc)
                {
                    organize(d);
                }

            }
            /*
            catch(Exception e)
            {

            }
            */
        }



        public static string pathstring(string x)
        {
            if (x.Length < 25)
                return x;
            return x.Substring(0, 8) +"..."+ x.Substring(x.Length - 8);
        }
        

        public static long DirSize(string path)
        {
            PowerCount = 0;
            Processed = 0;
            TotalSize = DirSize(new DirectoryInfo(path));
            return TotalSize;
        }

        public static long DirSize(DirectoryInfo d)
        {
            long size = 0;
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
            return size;
        }


        public static bool CreateDirectory(string pathToCreate)
        {
            //string path = @"C:\MRPower";

            try
            {
                if (Directory.Exists(pathToCreate))
                {
                    return true;
                }
                DirectoryInfo di = Directory.CreateDirectory(pathToCreate);
                
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }



        public static bool isEmptyDirectory(string path)
        {
            if (Directory.GetFiles(path).Length == 0 && Directory.GetDirectories(path).Length == 0)
                return true;
            return false;
        }


        public static void removeEmptyDirectory(string path)
        {
            string[] paths = Directory.GetDirectories(path);
            foreach(string x in paths)
            {
                removeEmptyDirectory(x);
            }
            if (isEmptyDirectory(path))
                Directory.Delete(path);
        }


    }
}
