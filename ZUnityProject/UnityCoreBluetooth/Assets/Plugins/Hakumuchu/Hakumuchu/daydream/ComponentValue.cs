
namespace Hakumuchu.DayDreamController.Components
{
    public struct Range
    {
        public int from, to;
        public Range(int from, int to)
        { this.from = from; this.to = to; }
    }

    public class ComponentValue<T>
    {
        Range range;
        public System.Func<int, T> func;
        public ComponentValue(Range range, System.Func<int, T> func)
        {
            this.range = range;
            this.func = func;
        }
        public T GetValue(ref DataAnalyzer data)
        {
            return this.func(data.Slice(this.range.from, this.range.to));
        }
    }

    public abstract class ValueSystem<T> where T : new()
    {
        protected abstract IValueAnalyzer<T> CreateAnalyzer();
        private T _value = new T();
        public void Update(ref DataAnalyzer data)
        {
            if (_analyzer == null) _analyzer = CreateAnalyzer();
            T newValue = _analyzer.GetValue(ref data);
            if (_func != null) _func(newValue);
            _value = newValue;
        }
        public T Value
        {
            get { return _value; }
        }
        private System.Action<T> _func;
        public virtual void OnUpdateValue(System.Action<T> func)
        {
            _func = func;
        }
        private IValueAnalyzer<T> _analyzer = null;
    }

    public class Vector3Analyzer: IValueAnalyzer<UnityEngine.Vector3>
    {
        public ComponentValue<float> x, y, z;
        public void SetUp(Range xRange, Range yRange, Range zRange, System.Func<int, float> func)
        {
            x = new ComponentValue<float>(xRange, func);
            y = new ComponentValue<float>(yRange, func);
            z = new ComponentValue<float>(zRange, func);
        }
        public UnityEngine.Vector3 GetValue(ref DataAnalyzer data)
        {
            return new UnityEngine.Vector3(x.GetValue(ref data), y.GetValue(ref data), z.GetValue(ref data));
        }
    }
}
