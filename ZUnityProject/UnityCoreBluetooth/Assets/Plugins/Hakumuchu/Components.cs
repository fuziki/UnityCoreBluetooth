using UnityEngine;
using System;

namespace Hakumuchu.DayDreamController.Components
{
    public class TouchPad
    {
        public bool click;
        public float x, y;
        public TouchPad(bool click, float x, float y)
        { this.click = click; this.x = x; this.y = y; }
    }
    public struct Buttons
    {
        public struct Volume
        {
            public bool up, down;
            public Volume(bool up, bool down)
            { this.up = up; this.down = down; }
        }
        public bool app, home;
        public Volume volume;
        public Buttons(bool app, bool home, Volume volume)
        { this.app = app; this.home = home; this.volume = volume; }
    }

    public struct Range
    {
        public int from, to;
        public Range(int from, int to)
        { this.from = from; this.to = to; }
    }

    public class ComponentValue<T>
    {
        Range range;
        public Func<int, T> func;
        public ComponentValue(Range range, Func<int, T> func)
        { 
            this.range = range; 
            this.func = func; 
        }
        public T GetValue(ref DataAnalyzer data)
        {
            return this.func(data.Slice(this.range.from, this.range.to));
        }
    }

    public class Vector3Analyzer
    {
        public ComponentValue<float> x, y, z;
        public void SetUp(Range xRange, Range yRange, Range zRange, Func<int, float> func)
        {
            x = new ComponentValue<float>(xRange, func);
            y = new ComponentValue<float>(yRange, func);
            z = new ComponentValue<float>(zRange, func);
        }
        public Vector3 GetValue(ref DataAnalyzer data)
        {
            return new Vector3(x.GetValue(ref data), y.GetValue(ref data), z.GetValue(ref data));
        }
    }
}
