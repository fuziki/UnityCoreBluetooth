using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakumuchu.DayDreamController
{
    public class DayDreamControllerAnalyzer
    {
        public DataAnalyzer dataAnalyzer = new DataAnalyzer();
        public Components.TouchPadAnalyzer touchPadAnalyzer = new Components.TouchPadAnalyzer();
        public Components.ButtonsAnalyzer buttonsAnalyzer = new Components.ButtonsAnalyzer();
        public Components.GyroAnalyzer gyroAnalyzer = new Components.GyroAnalyzer();
        public Components.MagnetAnalyzer magnetAnalyzer = new Components.MagnetAnalyzer();
        public Components.AcceleratorAnalyzer acceleratorAnalyzer = new Components.AcceleratorAnalyzer();

        public void UpdateBytes(byte[] bytes)
        {
            dataAnalyzer.UpdateBytes(bytes);
            _touchPad = touchPadAnalyzer.GetValue(ref dataAnalyzer);
            _buttons = buttonsAnalyzer.GetValue(ref dataAnalyzer);
            _gyro = gyroAnalyzer.GetValue(ref dataAnalyzer);
            _magnet = magnetAnalyzer.GetValue(ref dataAnalyzer);
            _accelerator = acceleratorAnalyzer.GetValue(ref dataAnalyzer);
        }

        private Components.TouchPad _touchPad = new Components.TouchPad(false, 0, 0);
        public Components.TouchPad TouchPad
        { get { return _touchPad; } }

        private Components.Buttons _buttons = new Components.Buttons();
        public Components.Buttons Buttons
        { get { return _buttons; } }

        private Vector3 _gyro = new Vector3();
        public Vector3 Gyro
        { get { return _gyro; } }

        private Vector3 _magnet = new Vector3();
        public Vector3 Magnet
        { get { return _magnet; } }

        private Vector3 _accelerator = new Vector3();
        public Vector3 Accelerator
        { get { return _accelerator; } }
    }
}
