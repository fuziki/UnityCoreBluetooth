using UnityEngine;

namespace Hakumuchu.DayDreamController.Components
{

    public abstract class ValueSystem<T> where T: new()
    {
        public abstract void Update(ref DataAnalyzer data);
        private T _value = new T();
        public T Value { 
            get { return _value; }
        }
        public void UpdateValue(T newValue)
        {
            if (_func != null) _func(newValue);
            _value = newValue;
        }
        public virtual void OnUpdateValue(System.Action<T> func)
        {
            _func = func;
        }
        private System.Action<T> _func;
    }


    public class TouchPad: ValueSystem<TouchPad.State>
    {
        public struct State
        {
            public bool click;
            public Vector2 position;
            public State(bool click, Vector2 position)
            { this.click = click; this.position = position; }
        }
        private TouchPadAnalyzer analyzer = new TouchPadAnalyzer();

        public override void Update(ref DataAnalyzer data)
        {
            base.UpdateValue(analyzer.GetValue(ref data));
        }
    }

    public class Buttons: ValueSystem<Buttons.State>
    {
        public struct State
        {
            public bool app, home, up, down;
            public State(bool app, bool home, bool up, bool down)
            { this.app = app; this.home = home; this.up = up; this.down = down; }
        }
        private ButtonsAnalyzer analyzer = new ButtonsAnalyzer();

        public override void Update(ref DataAnalyzer data)
        {
            base.UpdateValue(analyzer.GetValue(ref data));
        }
    }

    public class Gyro: ValueSystem<Vector3>
    {
        private GyroAnalyzer analyzer = new GyroAnalyzer();
        public override void Update(ref DataAnalyzer data)
        {
            base.UpdateValue(analyzer.GetValue(ref data));
        }
    }

    public class Magnet : ValueSystem<Vector3>
    {
        private MagnetAnalyzer analyzer = new MagnetAnalyzer();
        public override void Update(ref DataAnalyzer data)
        {
            base.UpdateValue(analyzer.GetValue(ref data));
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
        private AcceleratorAnalyzer analyzer = new AcceleratorAnalyzer();
        public override void Update(ref DataAnalyzer data)
        {
            base.UpdateValue(analyzer.GetValue(ref data));
        }
    }
}
