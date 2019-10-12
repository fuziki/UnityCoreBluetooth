
namespace Hakumuchu.DayDreamController
{
    public class DayDreamControllerAnalyzer
    {
        public struct State
        {
            public Components.TouchPad.State TouchPad;
            public Components.Buttons.State Buttons;
            public UnityEngine.Vector3 Gyro;
            public UnityEngine.Vector3 Magnet;
            public UnityEngine.Quaternion MagnetAsQuaternion;
            public UnityEngine.Vector3 Accelerator;
            public override string ToString()
            {
                return "TouchPad: " + TouchPad + ", Buttons: " + Buttons + ", Gyro: " + Gyro + ", Magnet: " + Magnet + ", MagnetAsQuaternion: " + ", Accelerator: " + Accelerator;
            }
        }

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

        ~DayDreamControllerAnalyzer()
        {
            touchPad.OnUpdateValue(null);
            buttons.OnUpdateValue(null);
            gyro.OnUpdateValue(null);
            magnet.OnUpdateValue(null);
            accelerator.OnUpdateValue(null);
        }

        public State Value
        {
            get
            {
                return new State() { TouchPad = touchPad.Value, Buttons = buttons.Value, Gyro = gyro.Value, Magnet = magnet.Value, MagnetAsQuaternion = magnet.ValueAsQuaternion, Accelerator = accelerator.Value };
            }
        }
    }
}
