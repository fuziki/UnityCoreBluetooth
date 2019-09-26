using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Hakumuchu.DayDreamController.Components
{
    public class TouchPadAnalyzer
    {
        public ComponentValue<bool> click = new ComponentValue<bool>(new Range(151, 152), (int data) => {
            return data != 0;
        });
        public ComponentValue<float> x = new ComponentValue<float>(new Range(131, 139), (int data) => {
            data &= 0xFF;
            data = data == 0 ? 0 : data - 127;
            return (float)data / 127.0f;
        });
        public ComponentValue<float> y = new ComponentValue<float>(new Range(139, 147), (int data) => {
            data &= 0xFF;
            data = data == 0 ? 0 : 127 - data;
            return (float)data / 127.0f;
        });
        public TouchPad GetValue(ref DataAnalyzer data)
        {
            return new TouchPad(click.GetValue(ref data), x.GetValue(ref data), y.GetValue(ref data));
        }
    }

    public class ButtonsAnalyzer
    {
        public class VolumeAnalyzer
        {
            public ComponentValue<bool> up = new ComponentValue<bool>(new Range(147, 148), (int data) => {
                return data != 0;
            });
            public ComponentValue<bool> down = new ComponentValue<bool>(new Range(148, 149), (int data) => {
                return data != 0;
            });
        }
        public ComponentValue<bool> app = new ComponentValue<bool>(new Range(149, 150), (int data) => {
            return data != 0;
        });
        public ComponentValue<bool> home = new ComponentValue<bool>(new Range(150, 151), (int data) => {
            return data != 0;
        });
        public VolumeAnalyzer volume;
        public Buttons GetValue(ref DataAnalyzer data)
        {
            return new Buttons(app.GetValue(ref data),
                home.GetValue(ref data),
                new Buttons.Volume(volume.up.GetValue(ref data), volume.down.GetValue(ref data)));
        }
    }

    public class GyroAnalyzer: Vector3Analyzer
    {
        public GyroAnalyzer()
        {
            this.SetUp(new Range(92, 105), new Range(105, 118), new Range(118, 131), (int data) => {
                return (float)data / 4095.0f * Mathf.PI * 2048.0f / 180.0f;
            });
        }
    }

    public class MagnetAnalyzer: Vector3Analyzer
    {
        public MagnetAnalyzer()
        {
            this.SetUp(new Range(14, 27), new Range(27, 40), new Range(40, 53), (int data) => {
                return (float)data / 4095.0f * 2.0f * Mathf.PI;
             });
        }
    }

    public class AcceleratorAnalyzer: Vector3Analyzer
    {
        public AcceleratorAnalyzer()
        {
            this.SetUp(new Range(53, 66), new Range(66, 79), new Range(79, 92), (int data) => {
                return (float)data / 4095.0f * 8.0f * 9.8f;
            });
        }
    }
}
