﻿using FixMath.NET;
using Fix64 = TrueSync.FP;
using System;

namespace BEPUutilities
{
    /// <summary>
    /// Contains helper math methods.
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Approximate value of Pi.
        /// </summary>
        public static readonly TrueSync.FP Pi = TrueSync.FP.Pi;

    /// <summary>
    /// Approximate value of Pi multiplied by two.
    /// </summary>
    public static readonly TrueSync.FP TwoPi = TrueSync.FP.PiTimes2;

    /// <summary>
    /// Approximate value of Pi divided by two.
    /// </summary>
    public static readonly TrueSync.FP PiOver2 = TrueSync.FP.PiOver2;

    /// <summary>
    /// Approximate value of Pi divided by four.
    /// </summary>
    public static readonly TrueSync.FP PiOver4 = TrueSync.FP.Pi / new TrueSync.FP(4);

    /// <summary>
    /// Calculate remainder of of TrueSync.FP division using same algorithm
    /// as Math.IEEERemainder
    /// </summary>
    /// <param name="dividend">Dividend</param>
    /// <param name="divisor">Divisor</param>
    /// <returns>Remainder</returns>
    public static TrueSync.FP IEEERemainder(TrueSync.FP dividend, TrueSync.FP divisor)
    {
		return dividend - (divisor * TrueSync.FP.Round(dividend / divisor));
    }

        /// <summary>
        /// Reduces the angle into a range from -Pi to Pi.
        /// </summary>
        /// <param name="angle">Angle to wrap.</param>
        /// <returns>Wrapped angle.</returns>
        public static TrueSync.FP WrapAngle(TrueSync.FP angle)
        {
            angle = IEEERemainder(angle, TwoPi);
            if (angle < -Pi)
            {
                angle += TwoPi;
                return angle;
            }
            if (angle >= Pi)
            {
                angle -= TwoPi;
            }
            return angle;

        }

        /// <summary>
        /// Clamps a value between a minimum and maximum value.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <param name="min">Minimum value.  If the value is less than this, the minimum is returned instead.</param>
        /// <param name="max">Maximum value.  If the value is more than this, the maximum is returned instead.</param>
        /// <returns>Clamped value.</returns>
        public static TrueSync.FP Clamp(TrueSync.FP value, TrueSync.FP min, TrueSync.FP max)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            return value;
        }


        /// <summary>
        /// Returns the higher value of the two parameters.
        /// </summary>
        /// <param name="a">First value.</param>
        /// <param name="b">Second value.</param>
        /// <returns>Higher value of the two parameters.</returns>
        public static TrueSync.FP Max(TrueSync.FP a, TrueSync.FP b)
        {
            return a > b ? a : b;
        }

        /// <summary>
        /// Returns the lower value of the two parameters.
        /// </summary>
        /// <param name="a">First value.</param>
        /// <param name="b">Second value.</param>
        /// <returns>Lower value of the two parameters.</returns>
        public static TrueSync.FP Min(TrueSync.FP a, TrueSync.FP b)
        {
            return a < b ? a : b;
        }

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="degrees">Degrees to convert.</param>
        /// <returns>Radians equivalent to the input degrees.</returns>
        public static TrueSync.FP ToRadians(TrueSync.FP degrees)
        {
            return degrees * (Pi / F64.C180);
        }

        /// <summary>
        /// Converts radians to degrees.
        /// </summary>
        /// <param name="radians">Radians to convert.</param>
        /// <returns>Degrees equivalent to the input radians.</returns>
        public static TrueSync.FP ToDegrees(TrueSync.FP radians)
        {
            return radians * (F64.C180 / Pi);
        }
    }
}
