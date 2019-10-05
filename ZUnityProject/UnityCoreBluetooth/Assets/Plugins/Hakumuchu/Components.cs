using UnityEngine;

namespace Hakumuchu.DayDreamController.Components
{
    public class TouchPad: ValueSystem<TouchPad.State>
    {
        public struct State
        {
            public bool click;
            public Vector2 position;
            public State(bool click, Vector2 position)
            { this.click = click; this.position = position; }
            public override string ToString()
            { return "click: " + click + ", position: " + position; }
        }

        protected override IValueAnalyzer<TouchPad.State> CreateAnalyzer()
        {
            return new TouchPadAnalyzer();
        }
    }

    public class Buttons: ValueSystem<Buttons.State>
    {
        public struct State
        {
            public bool app, home, up, down;
            public State(bool app, bool home, bool up, bool down)
            { this.app = app; this.home = home; this.up = up; this.down = down; }
            public override string ToString()
            { return "app: " + app + ", home: " + home + ", up: " + up + ", down" + down; }
        }

        protected override IValueAnalyzer<Buttons.State> CreateAnalyzer()
        {
            return new ButtonsAnalyzer();
        }
    }

    public class Gyro: ValueSystem<Vector3>
    {
        protected override IValueAnalyzer<Vector3> CreateAnalyzer()
        {
            return new GyroAnalyzer();
        }
    }

    public class Magnet : ValueSystem<Vector3>
    {
        protected override IValueAnalyzer<Vector3> CreateAnalyzer()
        {
            return new MagnetAnalyzer();
        }
        public Quaternion ValueAsQuaternion
        {
            get {
                float angle = Mathf.Sqrt(Value.x * Value.x + Value.y * Value.y + Value.z * Value.z);
                Vector3 n = Value.normalized;
                return Quaternion.AngleAxis(angle * 180f / Mathf.PI, new Vector3(n.x * -1f, n.y * -1f, n.z)); 
            }
        }
    }

    public class Accelerator : ValueSystem<Vector3>
    {
        protected override IValueAnalyzer<Vector3> CreateAnalyzer()
        {
            return new AcceleratorAnalyzer();
        }
    }
}
