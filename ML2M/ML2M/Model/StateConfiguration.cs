using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace ML2M.Model
{
    public class StateConfiguration
    {
        public SongList SongList { get; set; }
        public string Keywords { get; set; }
        public int PresentationWindowLeft { get; set; }
        public int PresentationWindowTop { get; set; }

        private static string GetCurrentStateConfigurationPath()
        {
            var dataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            if (!Directory.Exists(dataPath))
                Directory.CreateDirectory(dataPath);
            return Path.Combine(dataPath, "States.json");
        }

        public void Save()
        {
            string content = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(GetCurrentStateConfigurationPath(), content);
        }

        public static StateConfiguration CreateInstance()
        {
            var path = GetCurrentStateConfigurationPath();
            if (!File.Exists(path))
                return new StateConfiguration();
            var content = File.ReadAllText(path);
            StateConfiguration configuration = JsonConvert.DeserializeObject<StateConfiguration>(content);
            return configuration;
        }

        public bool IsPresentationWindowPositionUnsure()
        {
            Console.WriteLine(string.Format("{0}, {1}, {2}, {3}, {4}, {5}", SystemParameters.VirtualScreenLeft, 
                SystemParameters.VirtualScreenTop, SystemParameters.VirtualScreenWidth, SystemParameters.VirtualScreenHeight,
                PresentationWindowLeft, PresentationWindowTop));
            if (PresentationWindowLeft < SystemParameters.VirtualScreenLeft)
                return true;
            if (PresentationWindowTop < SystemParameters.VirtualScreenTop)
                return true;
            if (PresentationWindowLeft > SystemParameters.VirtualScreenLeft + SystemParameters.VirtualScreenWidth)
                return true;
            if (PresentationWindowTop > SystemParameters.VirtualScreenTop + SystemParameters.VirtualScreenHeight)
                return true;
            return false;
        }

        public void UpdatePresentationWindowPosition(double srcX, double srcY, double sourceWidth, double sourceHeight,
            double destinationWidth, double destinationHeight, bool force = false)
        {
            UpdatePresentationWindowPosition((int)srcX, (int)srcY, (int)sourceWidth, (int)sourceHeight, (int)destinationWidth, (int)destinationHeight, force);
        }

        public void UpdatePresentationWindowPosition(int srcX, int srcY, int sourceWidth, int sourceHeight,
            int destinationWidth, int destinationHeight, bool force = false)
        {
            if (IsPresentationWindowPositionUnsure() || force)
            {
                int srcCenterX = (sourceWidth / 2) + srcX;
                int srcCenterY = (sourceHeight / 2) + srcY;
                int dstX = srcCenterX - (destinationWidth / 2);
                int dstY = srcCenterY - (destinationHeight / 2);
                PresentationWindowLeft = dstX;
                PresentationWindowTop = dstY;
            }
        }
    }
}
