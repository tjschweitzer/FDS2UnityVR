using System;
using UnityEngine;

namespace Pixyz.Plugin4Unity
{

    public static class MathExtensions
    {
        public static Quaternion QuaternionFromMatrix(in Matrix4x4 m)
        {
            // Creates a quaternion from a rotation matrix. 
            // The algorithm used is from Allan and Mark Watt's "Advanced 
            // Animation and Rendering Techniques" (ACM Press 1992).

            double s = 0.0;
            double[] q = new double[4] { 0.0, 0.0, 0.0, 0.0};
            double trace = m[0, 0] + m[1, 1] + m[2, 2];

            if (trace > 0.0)
            {
                s = Math.Sqrt(trace + 1.0);
                q[3] = s * 0.5;
                s = 0.5 / s;
                q[0] = (m[2, 1] - m[1, 2]) * s;
                q[1] = (m[0, 2] - m[2, 0]) * s;
                q[2] = (m[1, 0] - m[0, 1]) * s;
            }
            else
            {
                int[] nxt = new int[3] { 1, 2, 0 };
                int i = 0, j = 0, k = 0;

                if (m[1, 1] > m[0, 0])
                    i = 1;

                if (m[2, 2] > m[i, i])
                    i = 2;

                j = nxt[i];
                k = nxt[j];
                s = Math.Sqrt((m[i, i] - (m[j, j] + m[k, k])) + 1.0);

                q[i] = s * 0.5;
                s = 0.5 / s;
                q[3] = (m[k, j] - m[j, k]) * s;
                q[k] = (m[k, i] + m[i, k]) * s;
                q[j] = (m[j, i] + m[i, j]) * s;
            }

            return new Quaternion((float)q[0], (float)q[1], (float)q[2], (float)q[3]);
        }

        public static void GetTRS(this Matrix4x4 matrix, out Vector3 translation, out Quaternion rotation, out Vector3 scale)
        {
            float det = matrix.GetDeterminant();

            // Scale
            scale.x = matrix.MultiplyVector(new Vector3(1, 0, 0)).magnitude;
            scale.y = matrix.MultiplyVector(new Vector3(0, 1, 0)).magnitude;
            scale.z = matrix.MultiplyVector(new Vector3(0, 0, 1)).magnitude;
            scale = (det < 0) ? -scale : scale;

            // Rotation
            Matrix4x4 rotationMatrix = matrix;
            rotationMatrix.m03 = rotationMatrix.m13 = rotationMatrix.m23 = 0f;
            rotationMatrix = rotationMatrix * new Matrix4x4 { m00 = 1.0f/scale.x, m11 = 1.0f/scale.y, m22 = 1.0f/scale.z, m33 = 1 };
            rotation = QuaternionFromMatrix(rotationMatrix);

            // Position
            translation = matrix.GetColumn(3);
            translation = new Vector3(translation.x, translation.y, translation.z);
        }

        public static float GetDeterminant(this Matrix4x4 matrix)
        {
            return matrix.m00 * (matrix.m11 * matrix.m22 - matrix.m12 * matrix.m21) -
                   matrix.m10 * (matrix.m01 * matrix.m22 - matrix.m02 * matrix.m21) +
                   matrix.m20 * (matrix.m01 * matrix.m12 - matrix.m02 * matrix.m11);
        }

        public static void SetFromLocalMatrix(this Transform transform, Matrix4x4 matrix)
        {
            matrix.GetTRS(out Vector3 t, out Quaternion r, out Vector3 s);

            // Assign local TRS to transform
            transform.localPosition = t;
            transform.localRotation = r;
            transform.localScale = s;
        }

        public static Matrix4x4 GetLocalMatrix(this Transform transform)
        {
            Vector3 t = transform.localPosition;
            Quaternion r = transform.localRotation;
            Vector3 s = transform.localScale;

            Matrix4x4 matrix = Matrix4x4.identity;
            matrix.SetTRS(t, r, s);

            return matrix;
        }


        public static Matrix4x4 GetWorldMatrix(this Transform transform, bool isLeftHanded, bool isZup, float scaleFactor)
        {
            Vector3 t = transform.position;
            Quaternion r = transform.rotation;
            Vector3 s = transform.lossyScale;

            Matrix4x4 matrix = Matrix4x4.identity;
            matrix.SetTRS(t, r, s);

            return matrix;
        }
    }
}