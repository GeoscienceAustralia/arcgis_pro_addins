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
using System.Windows.Shapes;

// https://blogs.msdn.microsoft.com/jaimer/2009/01/20/styling-microsofts-wpf-datagrid/

namespace V_3D_Addin
{
 

    /// <summary>
    /// Interaction logic for EditSettingsWindow.xaml
    /// </summary>
    public partial class EditSettingsWindow : Window
    {
        public EditSettingsWindow()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            /*
            List<UserSetting> UserSettings = new List<UserSetting>();

            string this_key = "layer_name2";
            string this_value = "XYMT_pen_depth2";

            UserSettings.Add(new UserSetting() { Key = "layer_name", Value = "XYMT_pen_depth" });
            UserSettings.Add(new UserSetting() { Key = this_key, Value = this_value });
            UserSettings.Add(new UserSetting() { Key = "exp", Value = "1" });
            */

            this.Close();
        }

        private void btnSelectShapeFiles_Click(object sender, RoutedEventArgs e)
        {
            V_3D.selectedShapeFiles = new List<ShapeFile>();

            SelectShapeFilesDialog This_SelectShapeFilesDialog = new SelectShapeFilesDialog();
            This_SelectShapeFilesDialog.items = V_3D.selectedShapeFiles;

            This_SelectShapeFilesDialog.ShowDialog();

        }

    }
}
