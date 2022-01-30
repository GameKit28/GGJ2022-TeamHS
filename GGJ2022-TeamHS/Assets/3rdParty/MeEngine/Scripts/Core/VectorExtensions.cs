using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MeEngine
{
    public static class VectorExtensions
    {
        /// <summary>
        /// Clamps the x and y values of a Vector2 so neither fall outside the range of 0-1.
        /// </summary>
        public static Vector2 Clamp01(this Vector2 sourceVector)
        {
            return new Vector2(Mathf.Clamp01(sourceVector.x), Mathf.Clamp01(sourceVector.y));
        }

        /// <summary>
        /// Clamps the x, y, and z values of a Vector3 so none fall outside the range of 0-1.
        /// </summary>
        public static Vector3 Clamp01(this Vector3 sourceVector)
        {
            return new Vector3(Mathf.Clamp01(sourceVector.x), Mathf.Clamp01(sourceVector.y), Mathf.Clamp01(sourceVector.z));
        }


        /// <summary>
        /// Returns a vector containing the absolute values of x and y
        /// </summary>
        public static Vector2 Abs(this Vector2 sourceVector)
        {
            return new Vector2(Mathf.Abs(sourceVector.x), Mathf.Abs(sourceVector.y));
        }

        /// <summary>
        /// Returns a vector containing the absolute values of x, y, and z
        /// </summary>
        public static Vector3 Abs(this Vector3 sourceVector)
        {
            return new Vector3(Mathf.Abs(sourceVector.x), Mathf.Abs(sourceVector.y), Mathf.Abs(sourceVector.z));
        }

        public static Vector2 Max(this Vector2 sourceVector, float minValue)
        {
            return new Vector2(Mathf.Max(sourceVector.x, minValue), Mathf.Max(sourceVector.y, minValue));
        }
    }
}