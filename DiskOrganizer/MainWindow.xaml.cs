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
            //InitializeComponent();

            DuplicateFileFinder d = new DuplicateFileFinder();
            d.Show();

            this.Close();

            /*


            Util.MoveFileSafe(@"C:\Users\MUA\Music\videoplayback.mp4",
                @"C:\Users\MUA\Music\MEmu Music");
            Util.MoveFileSafe(@"C:\Users\MUA\Music\Tumi Jano Na Song  X Girlfriend Bangla Natok Song 2019  Afran Nisho & Tanjin Tisha.mp3",
                @"C:\Users\MUA\Music\MEmu Music");
            */
        }

        /*
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
        */

    }
}
