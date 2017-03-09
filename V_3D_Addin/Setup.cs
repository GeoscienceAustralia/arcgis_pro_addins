/*

   Copyright 2017 Esri

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

   See the License for the specific language governing permissions and
   limitations under the License.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using System.Configuration;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace V_3D_Addin
{
    internal class Setup : Button
    {
        protected override void OnClick()
        {
            Properties.Settings.Default.Reload();

            // Create UserSettings as list
            List<UserSetting> UserSettings = new List<UserSetting>();

            foreach (SettingsProperty currentProperty in Properties.Settings.Default.Properties)
            {
                string this_key = currentProperty.Name;
                string this_value = Properties.Settings.Default[this_key].ToString();
                UserSettings.Add(new UserSetting() { Key = this_key, Value = this_value });
            }

            // Allow user to change user settings
            EditSettingsWindow This_EditSettingWindow = new EditSettingsWindow();

            This_EditSettingWindow.UserSettings.ItemsSource = UserSettings;

            This_EditSettingWindow.ShowDialog();

            // Save the user settings
            foreach (UserSetting each_usersetting in UserSettings)
            {
                Properties.Settings.Default[each_usersetting.Key] = each_usersetting.Value;
            }

            Properties.Settings.Default.Save();

            // initialise variables
            // ToDo: make sure variables have valid values
            //
            // to stage
            // test

            //
            V_3D.shapefile_index = 0;

            GlobalCls.scale_factor = Properties.Settings.Default.scale_factor;
            V_3D.layer_name = Properties.Settings.Default.layer_name;

            V_3D.featureLayer = V_3D.map.FindLayers("XYMT_pen_depth", true).OfType<FeatureLayer>().FirstOrDefault();

            if (V_3D.featureLayer != null)
            {
                MapView mapView = MapView.Active;

                List<FeatureLayer> featurelayer_list = new List<FeatureLayer> { V_3D.featureLayer };
                mapView.SelectLayers(featurelayer_list);
                //mapView.ZoomToAsync(V_3D.featureLayer);

                QueuedTask.Run(() =>
                {
                    //mapView.SelectLayers(featurelayer_list);
                    mapView.ZoomTo(V_3D.featureLayer);
                });
            }
          
        }
    }
}
