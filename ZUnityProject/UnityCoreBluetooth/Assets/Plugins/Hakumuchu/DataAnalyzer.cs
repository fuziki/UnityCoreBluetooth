using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakumuchu.DayDreamController
{
    public class DataAnalyzer
    {
        private class ByteSlicer
        {
            public byte[] bytes = new byte[] { 0x00 };

            private int IntPow(int x, int y)
            {
                return (int)Mathf.Pow(x, y);
            }

            public int SliceUnsignedInt(int from, int to)
            {
                if (bytes.Length < 20) return 0;
                int fromBytesIndex = from / 8;
                int fromBytesAt = from % 8;
                int toBytesIndex = to / 8;
                int toBytesAt = to % 8;

                int ret = bytes[fromBytesIndex] & (byte)(IntPow(2, 8 - fromBytesAt) - 1);
                for (int i = fromBytesIndex + 1; i < toBytesIndex + 1; i++)
                {
                    ret = ret << 8;
                    ret |= bytes[i];
                }
                return ret >> (8 - toBytesAt);
            }

            public int SliceSignedInt(int from, int to)
            {
                int v = this.SliceUnsignedInt(from, to);
                int d = 64 - (to - from);
                v = (v << d) >> d;
                return v;
            }
        }

        private ByteSlicer byteSlicer = new ByteSlicer();

        public void UpdateBytes(byte[] bytes)
        {
            byteSlicer.bytes = bytes;
        }

        public int Slice(int from, int to)
        {
            return byteSlicer.SliceSignedInt(from, to);
        }
    }
}
