using System;
using System.Collections.Generic;
using System.Text;

namespace DistantaVincentysFormule
{
    /// <summary>
    /// Encapsulates a three dimensional location on a globe (GlobalCoordinates combined with
    /// an elevation in meters above a reference ellipsoid).
    /// </summary>
    [Serializable]
    public struct PozitiiGlobale : IComparable<PozitiiGlobale>
    {
        /// <summary>Global coordinates.</summary>
        private CoordonateGlobale mCoordinates;

        /// <summary>Elevation, in meters, above the surface of the ellipsoid.</summary>
        private double mElevation;

        /// <summary>
        /// Creates a new instance of GlobalPosition.
        /// </summary>
        /// <param name="coords">coordinates on the reference ellipsoid.</param>
        /// <param name="elevation">elevation, in meters, above the reference ellipsoid./param>
        public PozitiiGlobale(CoordonateGlobale coords, double elevation)
        {
            mCoordinates = coords;
            mElevation = elevation;
        }

        /// <summary>
        /// Creates a new instance of GlobalPosition for a position on the surface of
        /// the reference ellipsoid.
        /// </summary>
        /// <param name="coords"></param>
        public PozitiiGlobale(CoordonateGlobale coords)
          : this(coords, 0.0)
        {
        }

        /// <summary>Get/set global coordinates.</summary>
        public CoordonateGlobale Coordinates
        {
            get { return mCoordinates; }
            set { mCoordinates = value; }
        }

        /// <summary>Get/set latitude.</summary>
        public Unghiul Latitude
        {
            get { return mCoordinates.Latitudine; }
            set { mCoordinates.Latitudine = value; }
        }

        /// <summary>Get/set longitude.</summary>
        public Unghiul Longitude
        {
            get { return mCoordinates.Longitudine; }
            set { mCoordinates.Longitudine = value; }
        }

        /// <summary>
        /// Get/set elevation, in meters, above the surface of the reference ellipsoid.
        /// </summary>
        public double Elevation
        {
            get { return mElevation; }
            set { mElevation = value; }
        }

        /// <summary>
        /// Compare these coordinates to another.  Western longitudes are less than
        /// eastern logitudes.  If longitudes are equal, then southern latitudes are
        /// less than northern latitudes.  If coordinates are equal, lower elevations
        /// are less than higher elevations
        /// </summary>
        /// <param name="other">instance to compare to</param>
        /// <returns>-1, 0, or +1 as per IComparable contract</returns>
        public int CompareTo(PozitiiGlobale other)
        {
            int retval = mCoordinates.CompareTo(other.mCoordinates);

            if (retval == 0)
            {
                if (mElevation < other.mElevation) retval = -1;
                else if (mElevation > other.mElevation) retval = +1;
            }

            return retval;
        }

        /// <summary>
        /// Calculate a hash code.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hash = mCoordinates.GetHashCode();

            if (mElevation != 0) hash *= (int)mElevation;

            return hash;
        }

        /// <summary>
        /// Compare this position to another object for equality.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is PozitiiGlobale)) return false;

            PozitiiGlobale other = (PozitiiGlobale)obj;

            return (mElevation == other.mElevation) && (mCoordinates.Equals(other.mCoordinates));
        }

        /// <summary>
        /// Get position as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(mCoordinates.ToString());
            builder.Append(";elevation=");
            builder.Append(mElevation.ToString());
            builder.Append("m");

            return builder.ToString();
        }
    }
}
