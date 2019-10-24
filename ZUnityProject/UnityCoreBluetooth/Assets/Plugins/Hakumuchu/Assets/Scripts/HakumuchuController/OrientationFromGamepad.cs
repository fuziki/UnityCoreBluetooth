using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakumuchu
{
    public class OrientationFromGamepad: MonoBehaviour
    {
        public class JoyStick
        {
            public struct ValueType
            {
                public float h, v;
            }
            public string key_h, key_v;
            public JoyStick(string key_h, string key_v)
            {
                this.key_h = key_h; this.key_v = key_v;
            }
            public ValueType Value => new ValueType() { h = Input.GetAxis(key_h), v = Input.GetAxis(key_v) };
        }

        public JoyStick RStick = new JoyStick("R_Stick_H", "R_Stick_V"), LStick = new JoyStick("L_Stick_H", "L_Stick_V");

        public Quaternion RStickAsOrientation => StickValueToQuaternion(RStick.Value);

        private Quaternion StickValueToQuaternion(JoyStick.ValueType value)
        {
//            return (value.h == 0f && value.v == 0) ? Quaternion.Euler(50f, 323f, 0f) : Quaternion.Euler(value.v * 100f, value.h * -60f, 0f);
            return Quaternion.Euler(value.v * 100f, value.h * -60f, 0f);
        }

        void Update()
        {
/*            float lsh = Input.GetAxis("L_Stick_H");
            float lsv = Input.GetAxis("L_Stick_V");
            if ((lsh != 0) || (lsv != 0))
            {
                Debug.Log("L stick:" + lsh + "," + lsv);
            }
            //R Stick
            float rsh = Input.GetAxis("R_Stick_H");
            float rsv = Input.GetAxis("R_Stick_V");
            if ((rsh != 0) || (rsv != 0))
            {
                Debug.Log("R stick:" + rsh + "," + rsv);
            }*/
        }
    }
}
