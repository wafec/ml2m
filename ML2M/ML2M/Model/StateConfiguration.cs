using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
            return Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "Data"), "States.json");
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
            return PresentationWindowLeft == 0 && PresentationWindowTop == 0;
        }

        public void UpdatePresentationWindowPosition(double srcX, double srcY, double sourceWidth, double sourceHeight,
            double destinationWidth, double destinationHeight)
        {
            UpdatePresentationWindowPosition((int)srcX, (int)srcY, (int)sourceWidth, (int)sourceHeight, (int)destinationWidth, (int)destinationHeight);
        }

        public void UpdatePresentationWindowPosition(int srcX, int srcY, int sourceWidth, int sourceHeight,
            int destinationWidth, int destinationHeight)
        {
            if (IsPresentationWindowPositionUnsure())
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
