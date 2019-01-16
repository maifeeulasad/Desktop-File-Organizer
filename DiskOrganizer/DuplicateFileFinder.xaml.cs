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

            


            DFF dFF= new DFF("D:\\models",dffProgress,this);
            //DFF dFF= new DFF("D:\\models",dffProgress,this);
            //Debug.WriteLine(dFF.GetDuplicates()) ;




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


        }
    }
}
