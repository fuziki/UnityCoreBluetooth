using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakumuchu.DayDreamController
{
    public class DayDreamControllerAnalyzer
    {
        public DataAnalyzer dataAnalyzer = new DataAnalyzer();

        public Components.TouchPad touchPad = new Components.TouchPad();
        public Components.Buttons buttons = new Components.Buttons();
        public Components.Gyro gyro = new Components.Gyro();
        public Components.Magnet magnet = new Components.Magnet();
        public Components.Accelerator accelerator = new Components.Accelerator();

        public void UpdateBytes(byte[] bytes)
        {
            dataAnalyzer.UpdateBytes(bytes);
            touchPad.Update(ref dataAnalyzer);
            buttons.Update(ref dataAnalyzer);
            gyro.Update(ref dataAnalyzer);
            magnet.Update(ref dataAnalyzer);
            accelerator.Update(ref dataAnalyzer);
        }
    }
}
