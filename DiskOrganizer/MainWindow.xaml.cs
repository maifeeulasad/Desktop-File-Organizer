using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DiskOrganizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }


        private void start_Click(object sender,RoutedEventArgs e)
        {

            Util.CreateDirectory(Util.targetPath + "\\"+Util.sName);
            Util.removeEmptyDirectory(Util.targetPath);


            Util.toProcess = Util.textBoxToArray(To_Organize.Text);
            Util.notToProcess = Util.textBoxToArray(Not_To_Organize.Text);


            Util.preOrganize(Util.targetPath);
            Util.organize(Util.targetPath);
        }

        private void browse_Click(object sender, RoutedEventArgs e)
        {
            

            FolderBrowserDialog dlg = new FolderBrowserDialog();

            
            dlg.ShowNewFolderButton = true;
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string cPath = dlg.SelectedPath;


                Util.targetPath = cPath;

                path_textbox.Text = Util.pathstring(cPath);

            }
        }

    }
}
