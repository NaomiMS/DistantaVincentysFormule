using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace DistantaVincentysFormule
{
    public struct GeodezicCurba
    {
        /// <summary>Ellipsoidal distance (in meters).</summary>
        private readonly double mDistantaElipsoidala;

        /// <summary>Azimuth (degrees from north).</summary>
        private readonly Unghiul mAzimuth;

        /// <summary>Reverse azimuth (degrees from north).</summary>
        private readonly Unghiul mReverseAzimuth;

        /// <summary>
        /// Create a new GeodeticCurve.
        /// </summary>
        /// <param name="ellipsoidalDistance"></param>
        /// <param name="azimuth"></param>
        /// <param name="reverseAzimuth"></param>
        public GeodezicCurba(double distantaElipsoidala, Unghiul azimuth, Unghiul reverseAzimuth)
        {
            mDistantaElipsoidala = distantaElipsoidala;
            mAzimuth = azimuth;
            mReverseAzimuth = reverseAzimuth;
        }

        /// <summary>Ellipsoidal distance (in meters).</summary>
        public double DistantaElipsoidala
        {
            get { return mDistantaElipsoidala; }
        }

        /// <summary>
        /// Get the azimuth.  This is angle from north from start to end.
        /// </summary>
        public Unghiul Azimuth
        {
            get { return mAzimuth; }
        }

        /// <summary>
        /// Get the reverse azimuth.  This is angle from north from end to start.
        /// </summary>
        public Unghiul ReverseAzimuth
        {
            get { return mReverseAzimuth; }
        }

        /// <summary>
        /// Get curve as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("s=");
            builder.Append(mDistantaElipsoidala);
            builder.Append(";a12=");
            builder.Append(mAzimuth);
            builder.Append(";a21=");
            builder.Append(mReverseAzimuth);
            builder.Append(";");

            return builder.ToString();
        }
    }
}
