using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Win32;

namespace V_3D_Addin
{
   
    /// <summary>
    /// Interaction logic for SelectShapeFiles.xaml
    /// </summary>
    public partial class SelectShapeFilesDialog : Window
    {
        public List<ShapeFile> items;

        public SelectShapeFilesDialog()
        {
            InitializeComponent();
        }

        private void btnSelectShapeFiles_Click(object sender, RoutedEventArgs e)
        {
            //List<ShapeFiles> items = new List<ShapeFiles>();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Shape files (*.shp)|*.shp";
            if (openFileDialog.ShowDialog() == true)
                foreach (string filename in openFileDialog.FileNames)
                {
                    items.Add(new ShapeFile() { selected = true, filename = Path.GetFileNameWithoutExtension(filename), full_path = Path.GetFullPath(filename) });
                }

            // list view           
            lvShapeFiles.ItemsSource = items;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
