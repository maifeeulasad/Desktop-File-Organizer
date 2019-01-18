using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace DiskOrganizer
{
    /// <summary>
    /// Interaction logic for DuplicateFileFinder.xaml
    /// </summary>
    public partial class DuplicateFileFinder : Window
    {

        Dictionary<string, List<string>> tem;

        public static string path;

        public static Window window;

        public static DFF dff;

        public DuplicateFileFinder()
        {
            InitializeComponent();

            //path = @"C:\Users\MUA\Downloads";

            path = @"D:\models";

            window = this;

        }

        private void ShowDuplicatesThread()
        {
            tem = dff.GetDuplicates();
            foreach(string keys in tem.Keys)
            {
                window.Dispatcher.Invoke(
                    new Action(() =>
                    this.dffHashes.Items.Add(new HashDetails { Hash = keys })));
            }
        }


        private void NewThread1()
        {

            dff = new DFF(path);
        }
        private void NewThread2()
        {

            while(true)
            {

                window.Dispatcher.Invoke(
                    new Action(() =>
                    this.dffProgress.Value = DFF.progress));
                window.Dispatcher.Invoke(
                    new Action(() =>
                    this.dffDetails.Text = DFF.currentPath));
                try
                {
                    if (dff.done)
                    {
                        DFF.currentPath = "Done ! " + dff.GetDuplicates().Count+ " set of duplicates found";
                        DFF.progress = 0;
                        window.Dispatcher.Invoke(
                            new Action(() =>
                            this.dffProgress.Value = DFF.progress));
                        window.Dispatcher.Invoke(
                            new Action(() =>
                            this.dffDetails.Text = DFF.currentPath));
                        ShowDuplicatesThread();
                        return;
                    }
                }
                catch(Exception e)
                {
                    Debug.WriteLine("ignore" + e.ToString());
                }
                
            }
        }


        private void dffDeleteSelected_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dffSelectLocation_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();


            dlg.ShowNewFolderButton = true;
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                path = dlg.SelectedPath;

                

                dffLocation.Text = Util.pathstring(path);

            }

        }
        
       
        private void dffStart_Click(object sender, RoutedEventArgs e)
        {

            Thread t1 = new Thread(NewThread1);
            t1.Start();
            Thread t2 = new Thread(NewThread2);
            t2.Start();


        }

        private void ShowHashDetails(object sender, MouseButtonEventArgs e)
        {

            var itemHS = ((System.Windows.Controls.ListViewItem)sender).Content 
                as HashDetails;
            var itemLV = sender as System.Windows.Controls.ListViewItem;
            if (itemLV != null && itemLV.IsSelected)
            {
                ShowDuplicatesLocationsThread(itemHS.Hash);
                
            }

        }


        



        private void ShowDuplicatesLocationsThread(string key)
        {
            window.Dispatcher.Invoke(
                new Action(() =>
                this.dffHashFiles.Items.Clear()));


            List<string> x = tem[key];
            foreach (string y in x)
            {
                window.Dispatcher.Invoke(
                    new Action(() =>
                    this.dffHashFiles.Items.Add(new HashLocations { Location = y })));

            }
        }


        private void dffDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine("delete all");
            try
            {
                foreach (KeyValuePair<String, List<String>> kvp in tem)
                {
                    PartialDeletation(kvp);


                    
                }

            }
            catch (Exception exx)
            {

            }
        }


        private void PartialDeletation(KeyValuePair<String, List<String>> kvp)
        {
            try
            {
                List<String> locations = kvp.Value;

                int len = locations.Count;

                if (len < 2)
                {
                    return;
                }
                else
                {

                    for (int i = 1; i < len; i++)
                    {
                        //Debug.WriteLine("deleting ---" + locations.ElementAt(i));
                        File.Delete(locations.ElementAt(i));
                    }
                }
            }
            catch(Exception ex1)
            {

            }
            
        }




    }
}
