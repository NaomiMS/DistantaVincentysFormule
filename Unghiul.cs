using System;
using System.Collections.Generic;
using System.Text;

namespace DistantaVincentysFormule
{
    /// <summary>
    /// Encapsulation of an Angle.  Angles are constructed and serialized in
    /// degrees for human convenience, but a conversion to radians is provided
    /// for mathematical calculations.
    /// 
    /// Angle comparisons are performed in absolute terms - no "wrapping" occurs.
    /// In other words, 360 degress != 0 degrees.
    /// </summary>
    [Serializable]
    public struct Unghiul : IComparable<Unghiul>
    {
        /// <summary>Degrees/Radians conversion constant.</summary>
        private const double PiOver180 = Math.PI / 180.0;

        /// <summary>Angle value in degrees.</summary>
        private double mGrade;

        /// <summary>Zero Angle</summary>
        static public readonly Unghiul Zero = new Unghiul(0);

        /// <summary>180 degree Angle</summary>
        static public readonly Unghiul Unghi180 = new Unghiul(180);

        /// <summary>
        /// Construct a new Angle from a degree measurement.
        /// </summary>
        /// <param name="degrees">angle measurement</param>
        public Unghiul(double grade)
        {
            mGrade = grade;
        }

        /// <summary>
        /// Construct a new Angle from degrees and minutes.
        /// </summary>
        /// <param name="degrees">degree portion of angle measurement</param>
        /// <param name="minutes">minutes portion of angle measurement (0 <= minutes < 60)</param>
        public Unghiul(int grade, double minute)
        {
            mGrade = minute / 60.0;

            mGrade = (grade < 0) ? (grade - mGrade) : (grade + mGrade);
        }

        /// <summary>
        /// Construct a new Angle from degrees, minutes, and seconds.
        /// </summary>
        /// <param name="degrees">degree portion of angle measurement</param>
        /// <param name="minutes">minutes portion of angle measurement (0 <= minutes < 60)</param>
        /// <param name="seconds">seconds portion of angle measurement (0 <= seconds < 60)</param>
        public Unghiul(int grade, int minute, double secunde)
        {
            mGrade = (secunde / 3600.0) + (minute / 60.0);

            mGrade = (grade < 0) ? (grade - mGrade) : (grade + mGrade);
        }

        /// <summary>
        /// Get/set angle measured in degrees.
        /// </summary>
        public double Grade
        {
            get { return mGrade; }
            set { mGrade = value; }
        }

        /// <summary>
        /// Get/set angle measured in radians.
        /// </summary>
        public double Radiani
        {
            get { return mGrade * PiOver180; }
            set { mGrade = value / PiOver180; }
        }

        /// <summary>
        /// Get the absolute value of the angle.
        /// </summary>
        public Unghiul Abs()
        {
            return new Unghiul(Math.Abs(mGrade));
        }

        /// <summary>
        /// Compare this angle to another angle.
        /// </summary>
        /// <param name="other">other angle to compare to.</param>
        /// <returns>result according to IComparable contract/></returns>
        public int CompareTo(Unghiul other)
        {
            return mGrade.CompareTo(other.mGrade);
        }

        /// <summary>
        /// Calculate a hash code for the angle.
        /// </summary>
        /// <returns>hash code</returns>
        public override int GetHashCode()
        {
            return (int)(mGrade * 1000033);
        }

        /// <summary>
        /// Compare this Angle to another Angle for equality.  Angle comparisons
        /// are performed in absolute terms - no "wrapping" occurs.  In other
        /// words, 360 degress != 0 degrees.
        /// </summary>
        /// <param name="obj">object to compare to</param>
        /// <returns>'true' if angles are equal</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Unghiul)) return false;

            Unghiul other = (Unghiul)obj;

            return mGrade == other.mGrade;
        }

        /// <summary>
        /// Get coordinates as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return mGrade.ToString();
        }

        #region Operators
        public static Unghiul operator +(Unghiul lhs, Unghiul rhs)
        {
            return new Unghiul(lhs.mGrade + rhs.mGrade);
        }

        public static Unghiul operator -(Unghiul lhs, Unghiul rhs)
        {
            return new Unghiul(lhs.mGrade - rhs.mGrade);
        }

        public static bool operator >(Unghiul lhs, Unghiul rhs)
        {
            return lhs.mGrade > rhs.mGrade;
        }

        public static bool operator >=(Unghiul lhs, Unghiul rhs)
        {
            return lhs.mGrade >= rhs.mGrade;
        }

        public static bool operator <(Unghiul lhs, Unghiul rhs)
        {
            return lhs.mGrade < rhs.mGrade;
        }

        public static bool operator <=(Unghiul lhs, Unghiul rhs)
        {
            return lhs.mGrade <= rhs.mGrade;
        }

        public static bool operator ==(Unghiul lhs, Unghiul rhs)
        {
            return lhs.mGrade == rhs.mGrade;
        }

        public static bool operator !=(Unghiul lhs, Unghiul rhs)
        {
            return lhs.mGrade != rhs.mGrade;
        }

        /// <summary>
        /// Imlplicity cast a double as an Angle measured in degrees.
        /// </summary>
        /// <param name="degrees">angle in degrees</param>
        /// <returns>double cast as an Angle</returns>
        public static implicit operator Unghiul(double degrees)
        {
            return new Unghiul(degrees);
        }
        #endregion
    }
}
