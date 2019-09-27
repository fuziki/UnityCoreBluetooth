using UnityEngine;

namespace Hakumuchu.DayDreamController.Components
{
    public class TouchPad
    {
        public struct State
        {
            public bool click;
            public Vector2 position;
            public State(bool click, Vector2 position)
            { this.click = click; this.position = position; }
        }
        private TouchPadAnalyzer analyzer = new TouchPadAnalyzer();

        public void Update(ref DataAnalyzer data)
        {
            _value = analyzer.GetValue(ref data);
        }
        private State _value = new State(false,  new Vector2(0, 0));
        public State Value
        {
            get { return _value; }
        }
    }

    public class Buttons
    {
        public struct State
        {
            public bool app, home, up, down;
            public State(bool app, bool home, bool up, bool down)
            { this.app = app; this.home = home; this.up = up; this.down = down; }
        }
        private ButtonsAnalyzer analyzer = new ButtonsAnalyzer();

        public void Update(ref DataAnalyzer data)
        {
            _value = analyzer.GetValue(ref data);
        }
        private State _value = new State(false, false, false, false);
        public State Value
        {
            get { return _value; }
        }
    }

    public class Gyro
    {
        private GyroAnalyzer analyzer = new GyroAnalyzer();
        public void Update(ref DataAnalyzer data)
        {
            _value = analyzer.GetValue(ref data);
        }
        private Vector3 _value = new Vector3(0, 0, 0);
        public Vector3 Value
        {
            get { return _value; }
        }
    }

    public class Magnet
    {
        private MagnetAnalyzer analyzer = new MagnetAnalyzer();
        public void Update(ref DataAnalyzer data)
        {
            _value = analyzer.GetValue(ref data);
        }
        private Vector3 _value = new Vector3(0, 0, 0);
        public Vector3 Value
        {
            get { return _value; }
        }
        public Quaternion ValueAsQuaternion
        {
            get {
                float angle = Mathf.Sqrt(_value.x * _value.x + _value.y * _value.y + _value.z * _value.z);
                Vector3 n = _value.normalized;
                return Quaternion.AngleAxis(angle * 180f / Mathf.PI, new Vector3(n.x * -1f, n.y * -1f, n.z)); 
            }
        }
    }

    public class Accelerator
    {
        private AcceleratorAnalyzer analyzer = new AcceleratorAnalyzer();
        public void Update(ref DataAnalyzer data)
        {
            _value = analyzer.GetValue(ref data);
        }
        private Vector3 _value = new Vector3(0, 0, 0);
        public Vector3 Value
        {
            get { return _value; }
        }
    }
}
