using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Server.MirForms.VisualMapInfo.Class
{
    public static class VisualizerGlobal
    {
        public static 
            Server.MirDatabase.MapInfo MapInfo = new Server.MirDatabase.MapInfo();

        public static int
            ZoomLevel = 1;

        public static MapInfo 
            ActiveMap;

        public static Bitmap
            ClippingMap = null;

        public static Tool
            SelectedTool = Tool.Select;

        public enum Tool
        {
            Select,
            Add,
            Move,
            Resize
        }
    }
}
