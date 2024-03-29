﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DistantaVincentysFormule
{
    /// <summary>
    /// This is the outcome of a three dimensional geodetic calculation.  It represents
    /// the path a between two GlobalPositions for a specified reference ellipsoid.
    /// </summary>
    [Serializable]
    public struct GeodezicMasurare
    {
        /// <summary>The average geodetic curve.</summary>
        private readonly GeodezicCurba mCurve;

        /// <summary>The elevation change, in meters, going from the starting to the ending point.</summary>
        private readonly double mElevationChange;

        /// <summary>The distance travelled, in meters, going from one point to the next.</summary>
        private readonly double mP2P;

        /// <summary>
        /// Creates a new instance of GeodeticMeasurement.
        /// </summary>
        /// <param name="averageCurve">the geodetic curve as measured at the average elevation between two points</param>
        /// <param name="elevationChange">the change in elevation, in meters, going from the starting point to the ending point</param>
        public GeodezicMasurare(GeodezicCurba averageCurve, double elevationChange)
        {
            double ellDist = averageCurve.DistantaElipsoidala;

            mCurve = averageCurve;
            mElevationChange = elevationChange;
            mP2P = Math.Sqrt(ellDist * ellDist + mElevationChange * mElevationChange);
        }

        /// <summary>
        /// Get the average geodetic curve.  This is the geodetic curve as measured
        /// at the average elevation between two points.
        /// </summary>
        public GeodezicCurba AverageCurve
        {
            get { return mCurve; }
        }

        /// <summary>
        /// Get the ellipsoidal distance (in meters).  This is the length of the average geodetic
        /// curve.  For actual point-to-point distance, use PointToPointDistance property.
        /// </summary>
        public double EllipsoidalDistance
        {
            get { return mCurve.DistantaElipsoidala; }
        }

        /// <summary>
        /// Get the azimuth.  This is angle from north from start to end.
        /// </summary>
        public Unghiul Azimuth
        {
            get { return mCurve.Azimuth; }
        }

        /// <summary>
        /// Get the reverse azimuth.  This is angle from north from end to start.
        /// </summary>
        public Unghiul ReverseAzimuth
        {
            get { return mCurve.ReverseAzimuth; }
        }

        /// <summary>
        /// Get the elevation change, in meters, going from the starting to the ending point.
        /// </summary>
        public double ElevationChange
        {
            get { return mElevationChange; }
        }

        /// <summary>
        /// Get the distance travelled, in meters, going from one point to the next.
        /// </summary>
        public double PointToPointDistance
        {
            get { return mP2P; }
        }

        /// <summary>
        /// Get the GeodeticMeasurement as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(mCurve.ToString());
            builder.Append(";elev12=");
            builder.Append(mElevationChange);
            builder.Append(";p2p=");
            builder.Append(mP2P);

            return builder.ToString();
        }
    }
}
