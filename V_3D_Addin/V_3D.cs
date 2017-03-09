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
using System.Windows.Input;
using ArcGIS.Core.Geometry;
using ArcGIS.Core.Data;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;

using System.Timers;
using System.Configuration;
using System.Diagnostics;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;

using ArcGIS.Core.CIM;

namespace V_3D_Addin
{
    /// <summary>
    /// User setting: key and Value
    /// </summary>
    public class UserSetting
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }

    /// <summary>
    /// Selected shapefile info
    /// </summary>
    public class ShapeFile
    {
        public bool selected { get; set; }
        public string filename { get; set; }
        public string full_path { get; set; }

        public override string ToString()
        {
            return this.selected + " " + this.filename;
        }
    }

    internal class V_3D : MapTool
    {
        public static Map map;
        public static int activation_count  = 1;
        //public static string scale_factor = "1";
        public static String layer_name;
        public static System.Timers.Timer aTimer = new System.Timers.Timer(2000);
        public static FeatureLayer featureLayer;

        // field3 to fields82 are z values
        private const int Start_Z_Field = 3;
        private const int End_Z_Field = 82;
        private static int z_field_index = 3;

        public static List<ShapeFile> selectedShapeFiles;
        public static int shapefile_index = 0;

        private static void SetTimer()
        {
            // Create a timer with a two second interval.
            //aTimer = new System.Timers.Timer(2000);

            // Hook up the Elapsed event for the timer. 
            // do it only once
            if (activation_count == 1) { aTimer.Elapsed += OnTimedEvent_Task_1; }
            aTimer.AutoReset = true;
            //aTimer.Enabled = true;
        }

        private static async Task My_CreatFeatureLayer_From_ShapeFile_Async(ShapeFile ShpFile, Map map)
        {
            string uriShp = @ShpFile.full_path;
            await QueuedTask.Run(() =>
            {
               LayerFactory.CreateFeatureLayer(new Uri(uriShp), map);
            });

        }

        private static async Task My_RemoveFeatureLayer_Async(FeatureLayer flyr, Map map)
        {
            await QueuedTask.Run(() =>
            {
                map.RemoveLayer(flyr);
            });
        }

        private static async void OnTimedEvent_Task_1(Object source, ElapsedEventArgs e)
        {
            // make sure there are selected shapefiles
            if (selectedShapeFiles != null)
            {

                aTimer.Stop();

                // remove previous feature layer
                if (featureLayer != null) await My_RemoveFeatureLayer_Async(featureLayer, map);

                MapView mapView = MapView.Active;
                ShapeFile This_ShpFile = selectedShapeFiles[shapefile_index];

                // create a featurelayer from a shapefile
                await My_CreatFeatureLayer_From_ShapeFile_Async(This_ShpFile, map);

                // select the newly created layer
                featureLayer = map.FindLayers(This_ShpFile.filename, true).OfType<FeatureLayer>().FirstOrDefault();

                // set transparent level
                // this is silly, should be one line 
                await QueuedTask.Run(() => {
                    // get the CIM definition from the layer
                    var cimFeatureDefinition = featureLayer.GetDefinition() as ArcGIS.Core.CIM.CIMBasicFeatureLayer;
                   
                    cimFeatureDefinition.Transparency = Convert.ToDouble(Properties.Settings.Default["polygon_layer_transparency"]);
                    featureLayer.SetDefinition(cimFeatureDefinition);

                    //Get simple renderer from feature layer
                    CIMSimpleRenderer currentRenderer = featureLayer.GetRenderer() as CIMSimpleRenderer;

                    currentRenderer.Symbol.Symbol.SetColor(ArcGIS.Core.CIM.CIMColor.CreateRGBColor(255, 131, 0)); // bright orange
                    //currentRenderer.Symbol.Symbol.SetColor(ColorFactory.CreateRGBColor(1,2,2));
                    //Update feature layer renderer with new symbol
                    featureLayer.SetRenderer(currentRenderer);

                });

              

                // zoom to newly created layer
                List<FeatureLayer> featurelayer_list = new List<FeatureLayer> { featureLayer };
                mapView.SelectLayers(featurelayer_list);

                // zoom
                await QueuedTask.Run(() =>
                {
                    // zoom to the first layer only
                    if (shapefile_index == 0)
                        mapView.ZoomTo(V_3D.featureLayer);
                });

                //String Path = @"c:\temp\arcgis_pro_export\" + This_ShpFile.filename + ".pdf";
                String Path = @"c:\temp\arcgis_pro_export\" + This_ShpFile.filename + "";

                // export mapView
                //await QueuedTask.Run(() =>
                //{
                //ExportFormat this_exportformat = new ArcGIS.Desktop.Mapping.PDFFormat
                PDFFormat this_exportformat = new PDFFormat();
                    this_exportformat.Resolution = 300;
                    this_exportformat.OutputFileName = @"c:\temp\arcgis_pro_export\" + This_ShpFile.filename + ".pdf";
                    //this_exportformat.Password = "xxx";
                    this_exportformat.Height = 960*2;
                    this_exportformat.Width = 960*2;
                    //mapView.Export(this_exportformat);

                // export JPEG
                // https://pro.arcgis.com/en/pro-app/sdk/api-reference/index.html#topic11696.html
                //Create JPEG format with appropriate settings
                JPEGFormat JPEG = new JPEGFormat();
                JPEG.Resolution = 300;
                JPEG.Height = 1500;
                JPEG.Width = 1500;
                Path = Path.Replace('.', '-');
                JPEG.OutputFileName = Path;
                mapView.Export(JPEG);

                //});

                aTimer.Start();

                shapefile_index += 1;
                if (shapefile_index >= selectedShapeFiles.Count)
                {
                    shapefile_index = 0;
                    aTimer.Stop();
                }
            }
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {

            //aTimer.Stop();

            //string extrusion_exp = "[Field3]*-1*" + System.Convert.ToString(scale / 10);
            //scale += 1.0;
            //if (scale > 11.0)
            //{
            //    scale = 1.0;
            //}

            string extrusion_exp = "[Field" + z_field_index.ToString() + "]*" + GlobalCls.scale_factor;

           // FeatureClass feat_cls = featureLayer.GetFeatureClass();

            ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
                // get the CIM definition from the layer
                var cimFeatureDefinition = featureLayer.GetDefinition() as ArcGIS.Core.CIM.CIMBasicFeatureLayer;

                cimFeatureDefinition.Extrusion = new ArcGIS.Core.CIM.CIMFeatureExtrusion();
                cimFeatureDefinition.Extrusion.ExtrusionType = ArcGIS.Core.CIM.ExtrusionType.Base;
                cimFeatureDefinition.Extrusion.ExtrusionUnit = ArcGIS.Core.Geometry.LinearUnit.Meters;
                cimFeatureDefinition.Extrusion.ExtrusionExpression = extrusion_exp;
                featureLayer.SetDefinition(cimFeatureDefinition);
            });

            z_field_index += 1;
            if (z_field_index > End_Z_Field)
            {
                z_field_index = Start_Z_Field;
            }

            //aTimer.Start();
            // ========== Zoom ===================================================

            layer_name = Properties.Settings.Default.layer_name;
            featureLayer = V_3D.map.FindLayers("XYMT_pen_depth", true).OfType<FeatureLayer>().FirstOrDefault();

            MapView mapView = MapView.Active;

            List<FeatureLayer> featurelayer_list = new List<FeatureLayer> { V_3D.featureLayer };
            mapView.SelectLayers(featurelayer_list);
            // ?? mapView.ZoomToAsync(V_3D.featureLayer);

            QueuedTask.Run(() =>
            {
                //mapView.SelectLayers(featurelayer_list);
                mapView.ZoomTo(V_3D.featureLayer); // working
            });
            // ==============================================================
        }

        public V_3D()
        {
        }

        protected override Task OnToolActivateAsync(bool active)
        {
            
            GlobalCls.scale_factor = Properties.Settings.Default.scale_factor;
            layer_name = Properties.Settings.Default.layer_name;

            map = MapView.Active.Map;
            SetTimer();

            featureLayer = map.FindLayers(layer_name, true).OfType<FeatureLayer>().FirstOrDefault();

            // add layers
            // open shapefile
            // http://gis.stackexchange.com/questions/181562/how-to-open-shapefile-dataset-using-arcgis-pro-sdk-net
            // https://github.com/Esri/arcgis-pro-sdk/wiki/ProConcepts-Map-Authoring#working-with-layers
           

            ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
                //create a layer from a shapefile
                string uriShp = @"C:\WorkSpace\V_3D\XYMT_pen_depth_orig.shp";
                Layer lyr = LayerFactory.CreateFeatureLayer(new Uri(uriShp), map);
            });
            //featureLayer = map.FindLayers("XYMT_pen_depth_orig", true).OfType<FeatureLayer>().FirstOrDefault();

            activation_count += 1;
            Debug.Print("activation_count: " + activation_count);

            // ToDo
            // make sure scale_factor and featureLayer are valid

            // activate the prerequisite state that the animation is ready to run
            FrameworkApplication.State.Activate("ready_to_run");

            return base.OnToolActivateAsync(active);
        }

        protected override void OnToolMouseDown(MapViewMouseButtonEventArgs e)
        {
            // On mouse down check if the mouse button pressed is:
            // the left mouse button to handle zoom in
            // or the right mouse button to handle zoom out
            // If it is handle the event.
            switch (e.ChangedButton)
            {
                case MouseButton.Right:
                    e.Handled = false;
                    break;
                case MouseButton.Left:
                    e.Handled = false;
                    break;
            }
        }

        protected override Task HandleMouseDownAsync(MapViewMouseButtonEventArgs e)
        {
            // Get the map coordinates from the click point and change the Camera to zoom in or out.
            return QueuedTask.Run(() =>
            {
                var mapClickPnt = MapView.Active.ClientToMap(e.ClientPoint);                
                ActiveMapView.LookAt(mapClickPnt, TimeSpan.FromSeconds(1));
                // zoom out
                if (e.ChangedButton == MouseButton.Right)
                {
                    //aTimer.Enabled = false;
                    ActiveMapView.ZoomOutFixed(TimeSpan.FromSeconds(1));
                }
                // zoom in
                else if (e.ChangedButton == MouseButton.Left)
                {
                    //SetTimer();
                    ActiveMapView.ZoomInFixed(TimeSpan.FromSeconds(1));
                }
            });
        }

        protected override void OnToolKeyDown(MapViewKeyEventArgs k)
        {
            // using key up and down in order to zoom out and in
            // if those keys are used we need to mark them as handled
            if (k.Key == Key.Up || k.Key == Key.Down || k.Key == Key.Q || k.Key == Key.W || k.Key == Key.S)
                k.Handled = true;

            if (k.Key == Key.Q)
            {
                aTimer.Start();
            }
            else if (k.Key == Key.W)
            {
                aTimer.Stop();

                //if (aTimer != null)
                //{
                //    aTimer.Stop();
                //    aTimer.Dispose();
                //}

            }
            else if (k.Key == Key.S)
            {
                Properties.Settings.Default.Reload();

                List<UserSetting> UserSettings = new List<UserSetting>();

                foreach (SettingsProperty currentProperty in Properties.Settings.Default.Properties)
                {
                    string this_key = currentProperty.Name;
                    string this_value = Properties.Settings.Default[this_key].ToString();
                    UserSettings.Add(new UserSetting() { Key = this_key, Value = this_value });
                }

                EditSettingsWindow This_EditSettingWindow = new EditSettingsWindow();

                This_EditSettingWindow.UserSettings.ItemsSource = UserSettings;

                This_EditSettingWindow.ShowDialog();

                foreach (UserSetting each_usersetting in UserSettings)
                {
                    Properties.Settings.Default[each_usersetting.Key] = each_usersetting.Value;
                }

                Properties.Settings.Default.Save();

                GlobalCls.scale_factor = Properties.Settings.Default.scale_factor;
                layer_name = Properties.Settings.Default.layer_name;

            }

            base.OnToolKeyDown(k);
        }

        protected override Task HandleKeyDownAsync(MapViewKeyEventArgs k)
        {
            // only called when 'handled' in OnToolKeyDown
            if (k.Key == Key.Up)
            {
                // Key.Up => zoom out
                return ActiveMapView.ZoomOutFixedAsync(TimeSpan.FromSeconds(1));
            }
            // Key.Down => zoom in
            //else if (k.Key == Key.Down)
            {
                return ActiveMapView.ZoomInFixedAsync(TimeSpan.FromSeconds(1));
            }
        }
    }
}
