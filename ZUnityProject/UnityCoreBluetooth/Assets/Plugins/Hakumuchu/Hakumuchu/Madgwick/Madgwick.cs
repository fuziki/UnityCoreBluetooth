using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakumuchu
{
    //this code is based on https://raw.githubusercontent.com/psiphi75/ahrs/master/Madgwick.js
    public class Madgwick
    {

        float q0 = 1.0f, q1 = 0.0f, q2 = 0.0f, q3 = 0.0f;
        bool initalised = false;

        float beta = 0.4f;

        public Madgwick(float beta, float period)
        {

        }

        public Quaternion Update(UnityEngine.Vector3 mag, UnityEngine.Vector3 gyr, UnityEngine.Vector3 acc)
        {
            //            return this.Update(gyr.x, gyr.y, -1f * gyr.z, acc.x, acc.y, -1f * acc.z, -1f * mag.x, -1f * mag.y, -1f * mag.z, 1f / 60f);
            //return this.Update(gyr.x, gyr.y, gyr.z, acc.x, acc.y, acc.z, mag.x, mag.y, mag.z, 1f / 60f);

            for (int i = 0; i < 3; i++)
            {
                while (mag[i] >  Mathf.PI / 2.0f) mag[i] -= Mathf.PI * 2.0f;
                while (mag[i] < -Mathf.PI / 2.0f) mag[i] += Mathf.PI * 2.0f;
                while (mag[i] >  Mathf.PI / 2.0f) mag[i] -= Mathf.PI * 2.0f;
            }

            return this.Update(
                gyr.x, gyr.y, gyr.z,
                acc.x, acc.y, acc.z,
                mag.x, mag.y, mag.z,
                1f / 60f);
            //return this.Update(
                //gyr.x, -1f * gyr.z, gyr.y,
                //acc.x, -1f * acc.z, acc.y,
                //mag.x, -1f * mag.z, mag.y,
                //1f / 60f);
            //            return this.Update(gyr.x, -1f * gyr.y, gyr.z, acc.x, -1f* acc.y, acc.z, -1f * mag.x, mag.y, -1f * mag.z, 1f / 60f);
            //            return this.Update(-1f * gyr.z, gyr.x, gyr.y, -1f * acc.z, acc.x, acc.y, -1f * mag.z, mag.x, mag.y, 1f / 60f);
        }

        void doBruteForceInitialisation(float ax, float ay, float az, float mx, float my, float mz)
        {
            initalised = true;
            float betaOrig = beta;
            beta = 0.4f;
            for (int i = 0; i < 10; i += 1)
            {
                Update(0.0f, 0.0f, 0.0f, ax, ay, az, mx, my, mz, 1.0f);
            }
            beta = betaOrig;
        }

        public Quaternion Update(float gx, float gy, float gz, float ax, float ay, float az, float mx, float my, float mz, float deltaTimeSec)
        {
            if (!initalised)
            {
                doBruteForceInitialisation(ax, ay, az, mx, my, mz);
            }


            float recipSampleFreq = deltaTimeSec;

            float recipNorm;
            float s0, s1, s2, s3;
            float qDot1, qDot2, qDot3, qDot4;
            float hx, hy;
            float v2q0mx, v2q0my, v2q0mz, v2q1mx, v2bx, v2bz, v4bx, v4bz, v2q0, v2q1, v2q2, v2q3, v2q0q2, v2q2q3;
            float q0q0, q0q1, q0q2, q0q3, q1q1, q1q2, q1q3, q2q2, q2q3, q3q3;

            // Use IMU algorithm if magnetometer measurement invalid (avoids NaN in magnetometer normalisation)
            //if (mx === undefined || my === undefined || mz === undefined || (mx === 0 && my === 0 && mz === 0))
            //{
            //    madgwickAHRSUpdateIMU(gx, gy, gz, ax, ay, az);
            //    return;
            //}

            // Rate of change of quaternion from gyroscope
            qDot1 = 0.5f * (-q1 * gx - q2 * gy - q3 * gz);
            qDot2 = 0.5f * (q0 * gx + q2 * gz - q3 * gy);
            qDot3 = 0.5f * (q0 * gy - q1 * gz + q3 * gx);
            qDot4 = 0.5f * (q0 * gz + q1 * gy - q2 * gx);

            // Compute feedback only if accelerometer measurement valid (avoids NaN in accelerometer normalisation)
            //if (!(Mathf.Abs(ax) < 0.01f && Mathf.Abs(ay) < 0.01f && Mathf.Abs(az) < 0.01f))
            //{
            // Normalise accelerometer measurement
            recipNorm = 1.0f / Mathf.Sqrt(ax * ax + ay * ay + az * az);// * *-0.5f;
            ax *= recipNorm;
            ay *= recipNorm;
            az *= recipNorm;

            // Normalise magnetometer measurement
            recipNorm = 1.0f / Mathf.Sqrt(mx * mx + my * my + mz * mz);// * *-0.5f;
            mx *= recipNorm;
            my *= recipNorm;
            mz *= recipNorm;

            // Auxiliary variables to avoid repeated arithmetic
            v2q0mx = 2.0f * q0 * mx;
            v2q0my = 2.0f * q0 * my;
            v2q0mz = 2.0f * q0 * mz;
            v2q1mx = 2.0f * q1 * mx;
            v2q0 = 2.0f * q0;
            v2q1 = 2.0f * q1;
            v2q2 = 2.0f * q2;
            v2q3 = 2.0f * q3;
            v2q0q2 = 2.0f * q0 * q2;
            v2q2q3 = 2.0f * q2 * q3;
            q0q0 = q0 * q0;
            q0q1 = q0 * q1;
            q0q2 = q0 * q2;
            q0q3 = q0 * q3;
            q1q1 = q1 * q1;
            q1q2 = q1 * q2;
            q1q3 = q1 * q3;
            q2q2 = q2 * q2;
            q2q3 = q2 * q3;
            q3q3 = q3 * q3;

            // Reference direction of Earth's magnetic field
            hx = mx * q0q0 - v2q0my * q3 + v2q0mz * q2 + mx * q1q1 + v2q1 * my * q2 + v2q1 * mz * q3 - mx * q2q2 - mx * q3q3;
            hy = v2q0mx * q3 + my * q0q0 - v2q0mz * q1 + v2q1mx * q2 - my * q1q1 + my * q2q2 + v2q2 * mz * q3 - my * q3q3;
            v2bx = 1.0f / Mathf.Sqrt(hx * hx + hy * hy);
            v2bz = -v2q0mx * q2 + v2q0my * q1 + mz * q0q0 + v2q1mx * q3 - mz * q1q1 + v2q2 * my * q3 - mz * q2q2 + mz * q3q3;
            v4bx = 2.0f * v2bx;
            v4bz = 2.0f * v2bz;

            // Gradient decent algorithm corrective step
            s0 =
              -v2q2 * (2.0f * q1q3 - v2q0q2 - ax) +
              v2q1 * (2.0f * q0q1 + v2q2q3 - ay) -
              v2bz * q2 * (v2bx * (0.5f - q2q2 - q3q3) + v2bz * (q1q3 - q0q2) - mx) +
              (-v2bx * q3 + v2bz * q1) * (v2bx * (q1q2 - q0q3) + v2bz * (q0q1 + q2q3) - my) +
              v2bx * q2 * (v2bx * (q0q2 + q1q3) + v2bz * (0.5f - q1q1 - q2q2) - mz);
            s1 =
              v2q3 * (2.0f * q1q3 - v2q0q2 - ax) +
              v2q0 * (2.0f * q0q1 + v2q2q3 - ay) -
              4.0f * q1 * (1f - 2.0f * q1q1 - 2.0f * q2q2 - az) +
              v2bz * q3 * (v2bx * (0.5f - q2q2 - q3q3) + v2bz * (q1q3 - q0q2) - mx) +
              (v2bx * q2 + v2bz * q0) * (v2bx * (q1q2 - q0q3) + v2bz * (q0q1 + q2q3) - my) +
              (v2bx * q3 - v4bz * q1) * (v2bx * (q0q2 + q1q3) + v2bz * (0.5f - q1q1 - q2q2) - mz);
            s2 =
              -v2q0 * (2.0f * q1q3 - v2q0q2 - ax) +
              v2q3 * (2.0f * q0q1 + v2q2q3 - ay) -
              4.0f * q2 * (1f - 2.0f * q1q1 - 2.0f * q2q2 - az) +
              (-v4bx * q2 - v2bz * q0) * (v2bx * (0.5f - q2q2 - q3q3) + v2bz * (q1q3 - q0q2) - mx) +
              (v2bx * q1 + v2bz * q3) * (v2bx * (q1q2 - q0q3) + v2bz * (q0q1 + q2q3) - my) +
              (v2bx * q0 - v4bz * q2) * (v2bx * (q0q2 + q1q3) + v2bz * (0.5f - q1q1 - q2q2) - mz);
            s3 =
              v2q1 * (2.0f * q1q3 - v2q0q2 - ax) +
              v2q2 * (2.0f * q0q1 + v2q2q3 - ay) +
              (-v4bx * q3 + v2bz * q1) * (v2bx * (0.5f - q2q2 - q3q3) + v2bz * (q1q3 - q0q2) - mx) +
              (-v2bx * q0 + v2bz * q2) * (v2bx * (q1q2 - q0q3) + v2bz * (q0q1 + q2q3) - my) +
              v2bx * q1 * (v2bx * (q0q2 + q1q3) + v2bz * (0.5f - q1q1 - q2q2) - mz);
            recipNorm = 1.0f / Mathf.Sqrt(s0 * s0 + s1 * s1 + s2 * s2 + s3 * s3);// * *-0.5f; // normalise step magnitude
            s0 *= recipNorm;
            s1 *= recipNorm;
            s2 *= recipNorm;
            s3 *= recipNorm;

            // Apply feedback step
            qDot1 -= beta * s0;
            qDot2 -= beta * s1;
            qDot3 -= beta * s2;
            qDot4 -= beta * s3;
//            }

            // Integrate rate of change of quaternion to yield quaternion
            q0 += qDot1 * recipSampleFreq;
            q1 += qDot2 * recipSampleFreq;
            q2 += qDot3 * recipSampleFreq;
            q3 += qDot4 * recipSampleFreq;

            // Normalise quaternion
            recipNorm = 1.0f / Mathf.Sqrt(q0 * q0 + q1 * q1 + q2 * q2 + q3 * q3);// * *-0.5;
            q0 *= recipNorm;
            q1 *= recipNorm;
            q2 *= recipNorm;
            q3 *= recipNorm;

//            return new Quaternion(q1, q2, q3, q0);
            return new Quaternion(q1, -1f * q3, -1f * q2, q0);
            //            return new Quaternion(-1f * q2, q3, q1, q0);
            //0:w 1:z 2:x 3:-y 
        }

    }







    //https://github.com/japaric/madgwick/blob/master/src/lib.rs
    /*
        public class FailedMadgwick
        {
            private Quaternion q = new Quaternion(0, 0, 0, 1);
            private bool qIsInit = false;
            private float beta = 0.0f;
            private float period = 0.0f;
            public FailedMadgwick(float beta, float period)
            {
                this.beta = beta;
                this.period = period;
                this.beta = 0.6f;
                this.period = 0.0f1f;
                sw.Start();
                lastTimeSec = sw.ElapsedMilliseconds * 0.0f01f;
            }

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            private float lastTimeSec = 0f;

            public UnityEngine.Quaternion Update(UnityEngine.Vector3 mag, UnityEngine.Vector3 gyr, UnityEngine.Vector3 acc)
            {
                //if(!qIsInit)
                //{
                //    float angle = Mathf.Sqrt(mag.x * mag.x + mag.y * mag.y + mag.z * mag.z);
                //    Vector3 n = mag.normalized;
                //    this.q = Quaternion.AngleAxis(angle * 180f / Mathf.PI, new Vector3(n.x, n.y, n.z));
                //    qIsInit = true;
                //}

                //for (int i = 0; i < 3; i++)
                //{
                //    if (mag[i] >  2f * Mathf.PI) mag[i] -= 2f * Mathf.PI;
                //    if (mag[i] < -2f * Mathf.PI) mag[i] += 2f * Mathf.PI;
                //}

                acc.Normalize();
                mag.Normalize();

                float q1 = this.q.w;
                float q2 = this.q.x;
                float q3 = this.q.y;
                float q4 = this.q.z;

                Quaternion h = this.q * new Quaternion(mag.x, mag.y, mag.z, 0f) * this.q.Conjugate();
                float bx = Mathf.Sqrt(h.x * h.x + h.y * h.y);
                float bz = h.z;

    //            Debug.Log("h: " + h + ", bx: " + bx + ", bz: " + bz);

                Vector4 f_g = new Vector4(
                    2.0ff * (q2 * q4 - q1 * q3) - acc.x,
                    2.0ff * (q1 * q2 + q3 * q4) - acc.y,
                    2.0ff * (0.5f - q2 * q2 - q3 * q3) - acc.z,
                    0.0ff
                    );

                Matrix4x4 j_g_t = new Matrix4x4(
                    new Vector4(-2.0ff * q3, 2.0ff * q4, -2.0ff * q1, 2.0ff * q2),  //column0
                    new Vector4(2.0ff * q2, 2.0ff * q1, 2.0ff * q4, 2.0ff * q3),  //column1
                    new Vector4(0.0ff, -4.0ff * q2, -4.0ff * q3, 0.0ff),  //column2
                    new Vector4(0.0ff, 0.0ff, 0.0ff, 0.0ff)  //column3
                    );

    //            Debug.Log("f_g: " + f_g + ", j_g_t: " + j_g_t + ", j_g_t * f_g: " + j_g_t * f_g);

                Vector4 f_b = new Vector4(
                    2.0ff * bx * (0.5f - q3 * q3 - q4 * q4) + 2.0ff * bz * (q2 * q4 - q1 * q3) - mag.x,
                    2.0ff * bx * (q2 * q3 - q1 * q4) + 2.0ff * bz * (q1 * q2 + q3 * q4) - mag.y,
                    2.0ff * bx * (q1 * q3 + q2 * q4) + 2.0ff * bz * (0.5f - q2 * q2 - q3 * q3) - mag.z,
                    0.0ff
                    );

                Matrix4x4 j_b_t = new Matrix4x4(
                    new Vector4(
                        -2.0ff * bz * q3, 
                        2.0ff * bz * q4, 
                        -4.0ff * bx * q3 - 2.0ff * bz * q1, 
                        -4.0ff * bx * q4 + 2.0ff * bz * q2),  //column0
                    new Vector4(
                        -2.0ff * bx * q4 + 2.0ff * bz * q2, 
                        2.0ff * bx * q3 + 2.0ff * bz * q1, 
                        2.0ff * bx * q2 + 2.0ff * bz * q4,
                        -2.0ff * bx * q1 + 2.0ff * bz * q3),  //column1
                    new Vector4(
                        2.0ff * bx * q3, 
                        2.0ff * bx * q4 - 4.0ff * bz * q2, 
                        2.0ff * bx * q1 - 4.0ff * bz * q3,
                        2.0ff * bx * q2),               //column2
                    new Vector4(0.0ff, 0.0ff, 0.0ff, 0.0ff)  //column3
                    );

                Vector4 nabla_f = j_g_t * f_g + j_b_t * f_b;
                nabla_f.Normalize();

                // rate of change of quaternion from gyroscope (Eq 11)
                Vector4 dqdt = 0.5f * (this.q * new Quaternion(gyr.x, gyr.y, gyr.z, 0f)).ToVector4();
                //            Debug.Log("this.q: " + this.q + ", new Quaternion(gyr.x, gyr.y, gyr.z, 0f): " + new Quaternion(gyr.x, gyr.y, gyr.z, 0f) + ", ans: " + (this.q * new Quaternion(gyr.x, gyr.y, gyr.z, 0f)).ToVector4());
                Debug.Log("dqdt: " + dqdt);

                dqdt -= this.beta * nabla_f;

                float now = sw.ElapsedMilliseconds * 0.0f01f;
                float interval = now - lastTimeSec;
                lastTimeSec = now;
                Debug.Log("now: " + now + "interval: " + interval);

                Vector4 uq = this.q.ToVector4() + dqdt * 1f / 60f;
                uq.Normalize();

    //            Debug.Log(dqdt + ", " + period + ", " + nabla_f + ", j_g_t * f_g: " + j_g_t * f_g + ", " + j_b_t + ", " + f_b);

                this.q = uq.ToQuaternion();

                return this.q;
            }
        }*/
}

public static class ExtensionQuaternion
{
    public static UnityEngine.Vector4 ToVector4(this UnityEngine.Quaternion q)
    {
        return new UnityEngine.Vector4(q.w, q.x, q.y, q.z);
    }

    public static UnityEngine.Quaternion ToQuaternion(this Vector4 vector4)
    {
        return new Quaternion(vector4[1], vector4[2], vector4[3], vector4[0]);
    }

    public static UnityEngine.Quaternion Conjugate(this UnityEngine.Quaternion q)
    {
        return new Quaternion(-1.0f * q.x, -1.0f * q.y, -1.0f * q.z, q.w);
    }
}