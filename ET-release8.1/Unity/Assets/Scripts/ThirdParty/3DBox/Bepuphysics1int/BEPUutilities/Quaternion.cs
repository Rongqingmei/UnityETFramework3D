﻿using FixMath.NET;
using Fix64 = TrueSync.FP;
using System;

namespace BEPUutilities
{
    /// <summary>
    /// Provides XNA-like quaternion support.
    /// </summary>
    public struct Quaternion : IEquatable<Quaternion>
    {
        /// <summary>
        /// X component of the quaternion.
        /// </summary>
        public TrueSync.FP X;

        /// <summary>
        /// Y component of the quaternion.
        /// </summary>
        public TrueSync.FP Y;

        /// <summary>
        /// Z component of the quaternion.
        /// </summary>
        public TrueSync.FP Z;

        /// <summary>
        /// W component of the quaternion.
        /// </summary>
        public TrueSync.FP W;

        /// <summary>
        /// Constructs a new Quaternion.
        /// </summary>
        /// <param name="x">X component of the quaternion.</param>
        /// <param name="y">Y component of the quaternion.</param>
        /// <param name="z">Z component of the quaternion.</param>
        /// <param name="w">W component of the quaternion.</param>
        public Quaternion(TrueSync.FP x, TrueSync.FP y, TrueSync.FP z, TrueSync.FP w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        /// <summary>
        /// Adds two quaternions together.
        /// </summary>
        /// <param name="a">First quaternion to add.</param>
        /// <param name="b">Second quaternion to add.</param>
        /// <param name="result">Sum of the addition.</param>
        public static void Add(ref Quaternion a, ref Quaternion b, out Quaternion result)
        {
            result.X = a.X + b.X;
            result.Y = a.Y + b.Y;
            result.Z = a.Z + b.Z;
            result.W = a.W + b.W;
        }

        /// <summary>
        /// Multiplies two quaternions.
        /// </summary>
        /// <param name="a">First quaternion to multiply.</param>
        /// <param name="b">Second quaternion to multiply.</param>
        /// <param name="result">Product of the multiplication.</param>
        public static void Multiply(ref Quaternion a, ref Quaternion b, out Quaternion result)
        {
            TrueSync.FP x = a.X;
            TrueSync.FP y = a.Y;
            TrueSync.FP z = a.Z;
            TrueSync.FP w = a.W;
            TrueSync.FP bX = b.X;
            TrueSync.FP bY = b.Y;
            TrueSync.FP bZ = b.Z;
            TrueSync.FP bW = b.W;
            result.X = x * bW + bX * w + y * bZ - z * bY;
            result.Y = y * bW + bY * w + z * bX - x * bZ;
            result.Z = z * bW + bZ * w + x * bY - y * bX;
            result.W = w * bW - x * bX - y * bY - z * bZ;
        }

        /// <summary>
        /// Scales a quaternion.
        /// </summary>
        /// <param name="q">Quaternion to multiply.</param>
        /// <param name="scale">Amount to multiply each component of the quaternion by.</param>
        /// <param name="result">Scaled quaternion.</param>
        public static void Multiply(ref Quaternion q, TrueSync.FP scale, out Quaternion result)
        {
            result.X = q.X * scale;
            result.Y = q.Y * scale;
            result.Z = q.Z * scale;
            result.W = q.W * scale;
        }

        /// <summary>
        /// Multiplies two quaternions together in opposite order.
        /// </summary>
        /// <param name="a">First quaternion to multiply.</param>
        /// <param name="b">Second quaternion to multiply.</param>
        /// <param name="result">Product of the multiplication.</param>
        public static void Concatenate(ref Quaternion a, ref Quaternion b, out Quaternion result)
        {
            TrueSync.FP aX = a.X;
            TrueSync.FP aY = a.Y;
            TrueSync.FP aZ = a.Z;
            TrueSync.FP aW = a.W;
            TrueSync.FP bX = b.X;
            TrueSync.FP bY = b.Y;
            TrueSync.FP bZ = b.Z;
            TrueSync.FP bW = b.W;

            result.X = aW * bX + aX * bW + aZ * bY - aY * bZ;
            result.Y = aW * bY + aY * bW + aX * bZ - aZ * bX;
            result.Z = aW * bZ + aZ * bW + aY * bX - aX * bY;
            result.W = aW * bW - aX * bX - aY * bY - aZ * bZ;


        }

        /// <summary>
        /// Multiplies two quaternions together in opposite order.
        /// </summary>
        /// <param name="a">First quaternion to multiply.</param>
        /// <param name="b">Second quaternion to multiply.</param>
        /// <returns>Product of the multiplication.</returns>
        public static Quaternion Concatenate(Quaternion a, Quaternion b)
        {
            Quaternion result;
            Concatenate(ref a, ref b, out result);
            return result;
        }

        /// <summary>
        /// Quaternion representing the identity transform.
        /// </summary>
        public static Quaternion Identity
        {
            get
            {
                return new Quaternion(F64.C0, F64.C0, F64.C0, F64.C1);
            }
        }




        /// <summary>
        /// Constructs a quaternion from a rotation matrix.
        /// </summary>
        /// <param name="r">Rotation matrix to create the quaternion from.</param>
        /// <param name="q">Quaternion based on the rotation matrix.</param>
        public static void CreateFromRotationMatrix(ref Matrix3x3 r, out Quaternion q)
        {
            TrueSync.FP trace = r.M11 + r.M22 + r.M33;
#if !WINDOWS
            q = new Quaternion();
#endif
            if (trace >= F64.C0)
            {
                var S = TrueSync.FP.Sqrt(trace + F64.C1) * F64.C2; // S=4*qw 
                var inverseS = F64.C1 / S;
                q.W = F64.C0p25 * S;
                q.X = (r.M23 - r.M32) * inverseS;
                q.Y = (r.M31 - r.M13) * inverseS;
                q.Z = (r.M12 - r.M21) * inverseS;
            }
            else if ((r.M11 > r.M22) & (r.M11 > r.M33))
            {
                var S = TrueSync.FP.Sqrt(F64.C1 + r.M11 - r.M22 - r.M33) * F64.C2; // S=4*qx 
                var inverseS = F64.C1 / S;
                q.W = (r.M23 - r.M32) * inverseS;
                q.X = F64.C0p25 * S;
                q.Y = (r.M21 + r.M12) * inverseS;
                q.Z = (r.M31 + r.M13) * inverseS;
            }
            else if (r.M22 > r.M33)
            {
                var S = TrueSync.FP.Sqrt(F64.C1 + r.M22 - r.M11 - r.M33) * F64.C2; // S=4*qy
                var inverseS = F64.C1 / S;
                q.W = (r.M31 - r.M13) * inverseS;
                q.X = (r.M21 + r.M12) * inverseS;
                q.Y = F64.C0p25 * S;
                q.Z = (r.M32 + r.M23) * inverseS;
            }
            else
            {
                var S = TrueSync.FP.Sqrt(F64.C1 + r.M33 - r.M11 - r.M22) * F64.C2; // S=4*qz
                var inverseS = F64.C1 / S;
                q.W = (r.M12 - r.M21) * inverseS;
                q.X = (r.M31 + r.M13) * inverseS;
                q.Y = (r.M32 + r.M23) * inverseS;
                q.Z = F64.C0p25 * S;
            }
        }

        /// <summary>
        /// Creates a quaternion from a rotation matrix.
        /// </summary>
        /// <param name="r">Rotation matrix used to create a new quaternion.</param>
        /// <returns>Quaternion representing the same rotation as the matrix.</returns>
        public static Quaternion CreateFromRotationMatrix(Matrix3x3 r)
        {
            Quaternion toReturn;
            CreateFromRotationMatrix(ref r, out toReturn);
            return toReturn;
        }

        /// <summary>
        /// Constructs a quaternion from a rotation matrix.
        /// </summary>
        /// <param name="r">Rotation matrix to create the quaternion from.</param>
        /// <param name="q">Quaternion based on the rotation matrix.</param>
        public static void CreateFromRotationMatrix(ref Matrix r, out Quaternion q)
        {
            Matrix3x3 downsizedMatrix;
            Matrix3x3.CreateFromMatrix(ref r, out downsizedMatrix);
            CreateFromRotationMatrix(ref downsizedMatrix, out q);
        }

        /// <summary>
        /// Creates a quaternion from a rotation matrix.
        /// </summary>
        /// <param name="r">Rotation matrix used to create a new quaternion.</param>
        /// <returns>Quaternion representing the same rotation as the matrix.</returns>
        public static Quaternion CreateFromRotationMatrix(Matrix r)
        {
            Quaternion toReturn;
            CreateFromRotationMatrix(ref r, out toReturn);
            return toReturn;
        }


        /// <summary>
        /// Ensures the quaternion has unit length.
        /// </summary>
        /// <param name="quaternion">Quaternion to normalize.</param>
        /// <returns>Normalized quaternion.</returns>
        public static Quaternion Normalize(Quaternion quaternion)
        {
            Quaternion toReturn;
            Normalize(ref quaternion, out toReturn);
            return toReturn;
        }

        /// <summary>
        /// Ensures the quaternion has unit length.
        /// </summary>
        /// <param name="quaternion">Quaternion to normalize.</param>
        /// <param name="toReturn">Normalized quaternion.</param>
        public static void Normalize(ref Quaternion quaternion, out Quaternion toReturn)
        {
            TrueSync.FP inverse = F64.C1 / TrueSync.FP.Sqrt(quaternion.X * quaternion.X + quaternion.Y * quaternion.Y + quaternion.Z * quaternion.Z + quaternion.W * quaternion.W);
            toReturn.X = quaternion.X * inverse;
            toReturn.Y = quaternion.Y * inverse;
            toReturn.Z = quaternion.Z * inverse;
            toReturn.W = quaternion.W * inverse;
        }

        /// <summary>
        /// Scales the quaternion such that it has unit length.
        /// </summary>
        public void Normalize()
        {
            TrueSync.FP inverse = F64.C1 / TrueSync.FP.Sqrt(X * X + Y * Y + Z * Z + W * W);
            X *= inverse;
            Y *= inverse;
            Z *= inverse;
            W *= inverse;
        }

        /// <summary>
        /// Computes the squared length of the quaternion.
        /// </summary>
        /// <returns>Squared length of the quaternion.</returns>
        public TrueSync.FP LengthSquared()
        {
            return X * X + Y * Y + Z * Z + W * W;
        }

        /// <summary>
        /// Computes the length of the quaternion.
        /// </summary>
        /// <returns>Length of the quaternion.</returns>
        public TrueSync.FP Length()
        {
            return TrueSync.FP.Sqrt(X * X + Y * Y + Z * Z + W * W);
        }


        /// <summary>
        /// Blends two quaternions together to get an intermediate state.
        /// </summary>
        /// <param name="start">Starting point of the interpolation.</param>
        /// <param name="end">Ending point of the interpolation.</param>
        /// <param name="interpolationAmount">Amount of the end point to use.</param>
        /// <param name="result">Interpolated intermediate quaternion.</param>
        public static void Slerp(ref Quaternion start, ref Quaternion end, TrueSync.FP interpolationAmount, out Quaternion result)
        {
			TrueSync.FP cosHalfTheta = start.W * end.W + start.X * end.X + start.Y * end.Y + start.Z * end.Z;
            if (cosHalfTheta < F64.C0)
            {
                //Negating a quaternion results in the same orientation, 
                //but we need cosHalfTheta to be positive to get the shortest path.
                end.X = -end.X;
                end.Y = -end.Y;
                end.Z = -end.Z;
                end.W = -end.W;
                cosHalfTheta = -cosHalfTheta;
            }
            // If the orientations are similar enough, then just pick one of the inputs.
            if (cosHalfTheta > F64.C1m1em12)
            {
                result.W = start.W;
                result.X = start.X;
                result.Y = start.Y;
                result.Z = start.Z;
                return;
            }
            // Calculate temporary values.
            TrueSync.FP halfTheta = TrueSync.FP.Acos(cosHalfTheta);
			TrueSync.FP sinHalfTheta = TrueSync.FP.Sqrt(F64.C1 - cosHalfTheta * cosHalfTheta);

			TrueSync.FP aFraction = TrueSync.FP.Sin((F64.C1 - interpolationAmount) * halfTheta) / sinHalfTheta;
			TrueSync.FP bFraction = TrueSync.FP.Sin(interpolationAmount * halfTheta) / sinHalfTheta;

            //Blend the two quaternions to get the result!
            result.X = (TrueSync.FP)(start.X * aFraction + end.X * bFraction);
            result.Y = (TrueSync.FP)(start.Y * aFraction + end.Y * bFraction);
            result.Z = (TrueSync.FP)(start.Z * aFraction + end.Z * bFraction);
            result.W = (TrueSync.FP)(start.W * aFraction + end.W * bFraction);




        }

        /// <summary>
        /// Blends two quaternions together to get an intermediate state.
        /// </summary>
        /// <param name="start">Starting point of the interpolation.</param>
        /// <param name="end">Ending point of the interpolation.</param>
        /// <param name="interpolationAmount">Amount of the end point to use.</param>
        /// <returns>Interpolated intermediate quaternion.</returns>
        public static Quaternion Slerp(Quaternion start, Quaternion end, TrueSync.FP interpolationAmount)
        {
            Quaternion toReturn;
            Slerp(ref start, ref end, interpolationAmount, out toReturn);
            return toReturn;
        }


        /// <summary>
        /// Computes the conjugate of the quaternion.
        /// </summary>
        /// <param name="quaternion">Quaternion to conjugate.</param>
        /// <param name="result">Conjugated quaternion.</param>
        public static void Conjugate(ref Quaternion quaternion, out Quaternion result)
        {
            result.X = -quaternion.X;
            result.Y = -quaternion.Y;
            result.Z = -quaternion.Z;
            result.W = quaternion.W;
        }

        /// <summary>
        /// Computes the conjugate of the quaternion.
        /// </summary>
        /// <param name="quaternion">Quaternion to conjugate.</param>
        /// <returns>Conjugated quaternion.</returns>
        public static Quaternion Conjugate(Quaternion quaternion)
        {
            Quaternion toReturn;
            Conjugate(ref quaternion, out toReturn);
            return toReturn;
        }



        /// <summary>
        /// Computes the inverse of the quaternion.
        /// </summary>
        /// <param name="quaternion">Quaternion to invert.</param>
        /// <param name="result">Result of the inversion.</param>
        public static void Inverse(ref Quaternion quaternion, out Quaternion result)
        {
            TrueSync.FP inverseSquaredNorm = quaternion.X * quaternion.X + quaternion.Y * quaternion.Y + quaternion.Z * quaternion.Z + quaternion.W * quaternion.W;
            result.X = -quaternion.X * inverseSquaredNorm;
            result.Y = -quaternion.Y * inverseSquaredNorm;
            result.Z = -quaternion.Z * inverseSquaredNorm;
            result.W = quaternion.W * inverseSquaredNorm;
        }

        /// <summary>
        /// Computes the inverse of the quaternion.
        /// </summary>
        /// <param name="quaternion">Quaternion to invert.</param>
        /// <returns>Result of the inversion.</returns>
        public static Quaternion Inverse(Quaternion quaternion)
        {
            Quaternion result;
            Inverse(ref quaternion, out result);
            return result;

        }

        /// <summary>
        /// Tests components for equality.
        /// </summary>
        /// <param name="a">First quaternion to test for equivalence.</param>
        /// <param name="b">Second quaternion to test for equivalence.</param>
        /// <returns>Whether or not the quaternions' components were equal.</returns>
        public static bool operator ==(Quaternion a, Quaternion b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.W == b.W;
        }

        /// <summary>
        /// Tests components for inequality.
        /// </summary>
        /// <param name="a">First quaternion to test for equivalence.</param>
        /// <param name="b">Second quaternion to test for equivalence.</param>
        /// <returns>Whether the quaternions' components were not equal.</returns>
        public static bool operator !=(Quaternion a, Quaternion b)
        {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z || a.W != b.W;
        }

        /// <summary>
        /// Negates the components of a quaternion.
        /// </summary>
        /// <param name="a">Quaternion to negate.</param>
        /// <param name="b">Negated result.</param>
        public static void Negate(ref Quaternion a, out Quaternion b)
        {
            b.X = -a.X;
            b.Y = -a.Y;
            b.Z = -a.Z;
            b.W = -a.W;
        }      
        
        /// <summary>
        /// Negates the components of a quaternion.
        /// </summary>
        /// <param name="q">Quaternion to negate.</param>
        /// <returns>Negated result.</returns>
        public static Quaternion Negate(Quaternion q)
        {
            Negate(ref q, out var result);
            return result;
        }

        /// <summary>
        /// Negates the components of a quaternion.
        /// </summary>
        /// <param name="q">Quaternion to negate.</param>
        /// <returns>Negated result.</returns>
        public static Quaternion operator -(Quaternion q)
        {
            Negate(ref q, out var result);
            return result;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Quaternion other)
        {
            return X == other.X && Y == other.Y && Z == other.Z && W == other.W;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (obj is Quaternion)
            {
                return Equals((Quaternion)obj);
            }
            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode() + W.GetHashCode();
        }

        /// <summary>
        /// Transforms the vector using a quaternion.
        /// </summary>
        /// <param name="v">Vector to transform.</param>
        /// <param name="rotation">Rotation to apply to the vector.</param>
        /// <param name="result">Transformed vector.</param>
        public static void Transform(ref Vector3 v, ref Quaternion rotation, out Vector3 result)
        {
            //This operation is an optimized-down version of v' = q * v * q^-1.
            //The expanded form would be to treat v as an 'axis only' quaternion
            //and perform standard quaternion multiplication.  Assuming q is normalized,
            //q^-1 can be replaced by a conjugation.
            TrueSync.FP x2 = rotation.X + rotation.X;
            TrueSync.FP y2 = rotation.Y + rotation.Y;
            TrueSync.FP z2 = rotation.Z + rotation.Z;
            TrueSync.FP xx2 = rotation.X * x2;
            TrueSync.FP xy2 = rotation.X * y2;
            TrueSync.FP xz2 = rotation.X * z2;
            TrueSync.FP yy2 = rotation.Y * y2;
            TrueSync.FP yz2 = rotation.Y * z2;
            TrueSync.FP zz2 = rotation.Z * z2;
            TrueSync.FP wx2 = rotation.W * x2;
            TrueSync.FP wy2 = rotation.W * y2;
            TrueSync.FP wz2 = rotation.W * z2;
            //Defer the component setting since they're used in computation.
            TrueSync.FP transformedX = v.X * (F64.C1 - yy2 - zz2) + v.Y * (xy2 - wz2) + v.Z * (xz2 + wy2);
            TrueSync.FP transformedY = v.X * (xy2 + wz2) + v.Y * (F64.C1 - xx2 - zz2) + v.Z * (yz2 - wx2);
            TrueSync.FP transformedZ = v.X * (xz2 - wy2) + v.Y * (yz2 + wx2) + v.Z * (F64.C1 - xx2 - yy2);
            result.X = transformedX;
            result.Y = transformedY;
            result.Z = transformedZ;

        }

        /// <summary>
        /// Transforms the vector using a quaternion.
        /// </summary>
        /// <param name="v">Vector to transform.</param>
        /// <param name="rotation">Rotation to apply to the vector.</param>
        /// <returns>Transformed vector.</returns>
        public static Vector3 Transform(Vector3 v, Quaternion rotation)
        {
            Vector3 toReturn;
            Transform(ref v, ref rotation, out toReturn);
            return toReturn;
        }

        /// <summary>
        /// Transforms a vector using a quaternion. Specialized for x,0,0 vectors.
        /// </summary>
        /// <param name="x">X component of the vector to transform.</param>
        /// <param name="rotation">Rotation to apply to the vector.</param>
        /// <param name="result">Transformed vector.</param>
        public static void TransformX(TrueSync.FP x, ref Quaternion rotation, out Vector3 result)
        {
            //This operation is an optimized-down version of v' = q * v * q^-1.
            //The expanded form would be to treat v as an 'axis only' quaternion
            //and perform standard quaternion multiplication.  Assuming q is normalized,
            //q^-1 can be replaced by a conjugation.
            TrueSync.FP y2 = rotation.Y + rotation.Y;
            TrueSync.FP z2 = rotation.Z + rotation.Z;
            TrueSync.FP xy2 = rotation.X * y2;
            TrueSync.FP xz2 = rotation.X * z2;
            TrueSync.FP yy2 = rotation.Y * y2;
            TrueSync.FP zz2 = rotation.Z * z2;
            TrueSync.FP wy2 = rotation.W * y2;
            TrueSync.FP wz2 = rotation.W * z2;
            //Defer the component setting since they're used in computation.
            TrueSync.FP transformedX = x * (F64.C1 - yy2 - zz2);
            TrueSync.FP transformedY = x * (xy2 + wz2);
            TrueSync.FP transformedZ = x * (xz2 - wy2);
            result.X = transformedX;
            result.Y = transformedY;
            result.Z = transformedZ;

        }

        /// <summary>
        /// Transforms a vector using a quaternion. Specialized for 0,y,0 vectors.
        /// </summary>
        /// <param name="y">Y component of the vector to transform.</param>
        /// <param name="rotation">Rotation to apply to the vector.</param>
        /// <param name="result">Transformed vector.</param>
        public static void TransformY(TrueSync.FP y, ref Quaternion rotation, out Vector3 result)
        {
            //This operation is an optimized-down version of v' = q * v * q^-1.
            //The expanded form would be to treat v as an 'axis only' quaternion
            //and perform standard quaternion multiplication.  Assuming q is normalized,
            //q^-1 can be replaced by a conjugation.
            TrueSync.FP x2 = rotation.X + rotation.X;
            TrueSync.FP y2 = rotation.Y + rotation.Y;
            TrueSync.FP z2 = rotation.Z + rotation.Z;
            TrueSync.FP xx2 = rotation.X * x2;
            TrueSync.FP xy2 = rotation.X * y2;
            TrueSync.FP yz2 = rotation.Y * z2;
            TrueSync.FP zz2 = rotation.Z * z2;
            TrueSync.FP wx2 = rotation.W * x2;
            TrueSync.FP wz2 = rotation.W * z2;
            //Defer the component setting since they're used in computation.
            TrueSync.FP transformedX = y * (xy2 - wz2);
            TrueSync.FP transformedY = y * (F64.C1 - xx2 - zz2);
            TrueSync.FP transformedZ = y * (yz2 + wx2);
            result.X = transformedX;
            result.Y = transformedY;
            result.Z = transformedZ;

        }

        /// <summary>
        /// Transforms a vector using a quaternion. Specialized for 0,0,z vectors.
        /// </summary>
        /// <param name="z">Z component of the vector to transform.</param>
        /// <param name="rotation">Rotation to apply to the vector.</param>
        /// <param name="result">Transformed vector.</param>
        public static void TransformZ(TrueSync.FP z, ref Quaternion rotation, out Vector3 result)
        {
            //This operation is an optimized-down version of v' = q * v * q^-1.
            //The expanded form would be to treat v as an 'axis only' quaternion
            //and perform standard quaternion multiplication.  Assuming q is normalized,
            //q^-1 can be replaced by a conjugation.
            TrueSync.FP x2 = rotation.X + rotation.X;
            TrueSync.FP y2 = rotation.Y + rotation.Y;
            TrueSync.FP z2 = rotation.Z + rotation.Z;
            TrueSync.FP xx2 = rotation.X * x2;
            TrueSync.FP xz2 = rotation.X * z2;
            TrueSync.FP yy2 = rotation.Y * y2;
            TrueSync.FP yz2 = rotation.Y * z2;
            TrueSync.FP wx2 = rotation.W * x2;
            TrueSync.FP wy2 = rotation.W * y2;
            //Defer the component setting since they're used in computation.
            TrueSync.FP transformedX = z * (xz2 + wy2);
            TrueSync.FP transformedY = z * (yz2 - wx2);
            TrueSync.FP transformedZ = z * (F64.C1 - xx2 - yy2);
            result.X = transformedX;
            result.Y = transformedY;
            result.Z = transformedZ;

        }


        /// <summary>
        /// Multiplies two quaternions.
        /// </summary>
        /// <param name="a">First quaternion to multiply.</param>
        /// <param name="b">Second quaternion to multiply.</param>
        /// <returns>Product of the multiplication.</returns>
        public static Quaternion operator *(Quaternion a, Quaternion b)
        {
            Quaternion toReturn;
            Multiply(ref a, ref b, out toReturn);
            return toReturn;
        }

        /// <summary>
        /// Creates a quaternion from an axis and angle.
        /// </summary>
        /// <param name="axis">Axis of rotation.</param>
        /// <param name="angle">Angle to rotate around the axis.</param>
        /// <returns>Quaternion representing the axis and angle rotation.</returns>
        public static Quaternion CreateFromAxisAngle(Vector3 axis, TrueSync.FP angle)
        {
			TrueSync.FP halfAngle = angle * F64.C0p5;
			TrueSync.FP s = TrueSync.FP.Sin(halfAngle);
            Quaternion q;
            q.X = axis.X * s;
            q.Y = axis.Y * s;
            q.Z = axis.Z * s;
            q.W = TrueSync.FP.Cos(halfAngle);
            return q;
        }

        /// <summary>
        /// Creates a quaternion from an axis and angle.
        /// </summary>
        /// <param name="axis">Axis of rotation.</param>
        /// <param name="angle">Angle to rotate around the axis.</param>
        /// <param name="q">Quaternion representing the axis and angle rotation.</param>
        public static void CreateFromAxisAngle(ref Vector3 axis, TrueSync.FP angle, out Quaternion q)
        {
			TrueSync.FP halfAngle = angle * F64.C0p5;
			TrueSync.FP s = TrueSync.FP.Sin(halfAngle);
            q.X = axis.X * s;
            q.Y = axis.Y * s;
            q.Z = axis.Z * s;
            q.W = TrueSync.FP.Cos(halfAngle);
        }

        /// <summary>
        /// Constructs a quaternion from yaw, pitch, and roll.
        /// </summary>
        /// <param name="yaw">Yaw of the rotation.</param>
        /// <param name="pitch">Pitch of the rotation.</param>
        /// <param name="roll">Roll of the rotation.</param>
        /// <returns>Quaternion representing the yaw, pitch, and roll.</returns>
        public static Quaternion CreateFromYawPitchRoll(TrueSync.FP yaw, TrueSync.FP pitch, TrueSync.FP roll)
        {
            Quaternion toReturn;
            CreateFromYawPitchRoll(yaw, pitch, roll, out toReturn);
            return toReturn;
        }

        /// <summary>
        /// Constructs a quaternion from yaw, pitch, and roll.
        /// </summary>
        /// <param name="yaw">Yaw of the rotation.</param>
        /// <param name="pitch">Pitch of the rotation.</param>
        /// <param name="roll">Roll of the rotation.</param>
        /// <param name="q">Quaternion representing the yaw, pitch, and roll.</param>
        public static void CreateFromYawPitchRoll(TrueSync.FP yaw, TrueSync.FP pitch, TrueSync.FP roll, out Quaternion q)
        {
			TrueSync.FP halfRoll = roll * F64.C0p5;
			TrueSync.FP halfPitch = pitch * F64.C0p5;
			TrueSync.FP halfYaw = yaw * F64.C0p5;

			TrueSync.FP sinRoll = TrueSync.FP.Sin(halfRoll);
			TrueSync.FP sinPitch = TrueSync.FP.Sin(halfPitch);
			TrueSync.FP sinYaw = TrueSync.FP.Sin(halfYaw);

			TrueSync.FP cosRoll = TrueSync.FP.Cos(halfRoll);
			TrueSync.FP cosPitch = TrueSync.FP.Cos(halfPitch);
			TrueSync.FP cosYaw = TrueSync.FP.Cos(halfYaw);

			TrueSync.FP cosYawCosPitch = cosYaw * cosPitch;
			TrueSync.FP cosYawSinPitch = cosYaw * sinPitch;
			TrueSync.FP sinYawCosPitch = sinYaw * cosPitch;
			TrueSync.FP sinYawSinPitch = sinYaw * sinPitch;

            q.X = cosYawSinPitch * cosRoll + sinYawCosPitch * sinRoll;
            q.Y = sinYawCosPitch * cosRoll - cosYawSinPitch * sinRoll;
            q.Z = cosYawCosPitch * sinRoll - sinYawSinPitch * cosRoll;
            q.W = cosYawCosPitch * cosRoll + sinYawSinPitch * sinRoll;

        }

        /// <summary>
        /// Computes the angle change represented by a normalized quaternion.
        /// </summary>
        /// <param name="q">Quaternion to be converted.</param>
        /// <returns>Angle around the axis represented by the quaternion.</returns>
        public static TrueSync.FP GetAngleFromQuaternion(ref Quaternion q)
        {
            TrueSync.FP qw = TrueSync.FP.Abs(q.W);
            if (qw > F64.C1)
                return F64.C0;
            return F64.C2 * TrueSync.FP.Acos(qw);
        }

        /// <summary>
        /// Computes the axis angle representation of a normalized quaternion.
        /// </summary>
        /// <param name="q">Quaternion to be converted.</param>
        /// <param name="axis">Axis represented by the quaternion.</param>
        /// <param name="angle">Angle around the axis represented by the quaternion.</param>
        public static void GetAxisAngleFromQuaternion(ref Quaternion q, out Vector3 axis, out TrueSync.FP angle)
        {
#if !WINDOWS
            axis = new Vector3();
#endif
            TrueSync.FP qw = q.W;
            if (qw > F64.C0)
            {
                axis.X = q.X;
                axis.Y = q.Y;
                axis.Z = q.Z;
            }
            else
            {
                axis.X = -q.X;
                axis.Y = -q.Y;
                axis.Z = -q.Z;
                qw = -qw;
            }

            TrueSync.FP lengthSquared = axis.LengthSquared();
            if (lengthSquared > F64.C1em14)
            {
                Vector3.Divide(ref axis, TrueSync.FP.Sqrt(lengthSquared), out axis);
                angle = F64.C2 * TrueSync.FP.Acos(MathHelper.Clamp(qw, -1, F64.C1));
            }
            else
            {
                axis = Toolbox.UpVector;
                angle = F64.C0;
            }
        }

        /// <summary>
        /// Computes the quaternion rotation between two normalized vectors.
        /// </summary>
        /// <param name="v1">First unit-length vector.</param>
        /// <param name="v2">Second unit-length vector.</param>
        /// <param name="q">Quaternion representing the rotation from v1 to v2.</param>
        public static void GetQuaternionBetweenNormalizedVectors(ref Vector3 v1, ref Vector3 v2, out Quaternion q)
        {
            TrueSync.FP dot;
            Vector3.Dot(ref v1, ref v2, out dot);
            //For non-normal vectors, the multiplying the axes length squared would be necessary:
            //TrueSync.FP w = dot + (TrueSync.FP)Math.Sqrt(v1.LengthSquared() * v2.LengthSquared());
            if (dot < F64.Cm0p9999) //parallel, opposing direction
            {
                //If this occurs, the rotation required is ~180 degrees.
                //The problem is that we could choose any perpendicular axis for the rotation. It's not uniquely defined.
                //The solution is to pick an arbitrary perpendicular axis.
                //Project onto the plane which has the lowest component magnitude.
                //On that 2d plane, perform a 90 degree rotation.
                TrueSync.FP absX = TrueSync.FP.Abs(v1.X);
                TrueSync.FP absY = TrueSync.FP.Abs(v1.Y);
                TrueSync.FP absZ = TrueSync.FP.Abs(v1.Z);
                if (absX < absY && absX < absZ)
                    q = new Quaternion(F64.C0, -v1.Z, v1.Y, F64.C0);
                else if (absY < absZ)
                    q = new Quaternion(-v1.Z, F64.C0, v1.X, F64.C0);
                else
                    q = new Quaternion(-v1.Y, v1.X, F64.C0, F64.C0);
            }
            else
            {
                Vector3 axis;
                Vector3.Cross(ref v1, ref v2, out axis);
                q = new Quaternion(axis.X, axis.Y, axis.Z, dot + F64.C1);
            }
            q.Normalize();
        }

        //The following two functions are highly similar, but it's a bit of a brain teaser to phrase one in terms of the other.
        //Providing both simplifies things.

        /// <summary>
        /// Computes the rotation from the start orientation to the end orientation such that end = Quaternion.Concatenate(start, relative).
        /// </summary>
        /// <param name="start">Starting orientation.</param>
        /// <param name="end">Ending orientation.</param>
        /// <param name="relative">Relative rotation from the start to the end orientation.</param>
        public static void GetRelativeRotation(ref Quaternion start, ref Quaternion end, out Quaternion relative)
        {
            Quaternion startInverse;
            Conjugate(ref start, out startInverse);
            Concatenate(ref startInverse, ref end, out relative);
        }

        
        /// <summary>
        /// Transforms the rotation into the local space of the target basis such that rotation = Quaternion.Concatenate(localRotation, targetBasis)
        /// </summary>
        /// <param name="rotation">Rotation in the original frame of reference.</param>
        /// <param name="targetBasis">Basis in the original frame of reference to transform the rotation into.</param>
        /// <param name="localRotation">Rotation in the local space of the target basis.</param>
        public static void GetLocalRotation(ref Quaternion rotation, ref Quaternion targetBasis, out Quaternion localRotation)
        {
            Quaternion basisInverse;
            Conjugate(ref targetBasis, out basisInverse);
            Concatenate(ref rotation, ref basisInverse, out localRotation);
        }

        /// <summary>
        /// Gets a string representation of the quaternion.
        /// </summary>
        /// <returns>String representing the quaternion.</returns>
        public override string ToString()
        {
            return "{ X: " + X + ", Y: " + Y + ", Z: " + Z + ", W: " + W + "}";
        }
    }
}
