using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Desktop.Mapping;

namespace V_3D_Addin
{
    
    class GlobalCls
    {
        private static Map _map;
        public static Map map
        {
            get { return _map; }
            set { _map = value; }
        }

        private static string _scale_factor = "1";
        public static string scale_factor
        {
            get { return _scale_factor; }
            set { _scale_factor = value; }
        }

    }
}
