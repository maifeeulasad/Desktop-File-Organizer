using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        
        public static string path;

        

        public DuplicateFileFinder()
        {
            InitializeComponent();

            path = @"C:\Users\MUA\Downloads";

            

        }

        private void NewThread1()
        {

            DFF dff = new DFF(path, dffProgress, this);
        }
        private void NewThread2()
        {
            while(true)
            {
                this.Dispatcher.BeginInvoke(
                    new Action(() => 
                    this.dffProgress.Value = 50));
                
            }
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
        private void dffDeleteSelected_Click(object sender, RoutedEventArgs e)
        {


        }
        private void dffDeleteAll_Click(object sender, RoutedEventArgs e)
        {


        }
        private void dffStart_Click(object sender, RoutedEventArgs e)
        {

            Thread t1 = new Thread(NewThread1);
            t1.Start();
            Thread t2 = new Thread(NewThread2);
            t2.Start();


        }
    }
}
