using System;
using System.Collections.Generic;
using System.Text;

namespace DistantaVincentysFormule
{
    /// <summary>
    /// Encapsulation of an ellipsoid, and declaration of common reference ellipsoids.
    /// </summary>
    [Serializable]
    public struct Elipsoid
    {
        /// <summary>Semi major axis (meters).</summary>
        private readonly double mSemiAxaMajora;

        /// <summary>Semi minor axis (meters).</summary>
        private readonly double mSemiAxaMinora;

        /// <summary>Flattening.</summary>
        private readonly double mAplatizarea;

        /// <summary>Inverse flattening.</summary>
        private readonly double mAplatizareaInversa;


        /// <summary>
        /// Construct a new Ellipsoid.  This is private to ensure the values are
        /// consistent (flattening = 1.0 / inverseFlattening).  Use the methods 
        /// FromAAndInverseF() and FromAAndF() to create new instances.
        /// </summary>
        /// <param name="semiMajor"></param>
        /// <param name="semiMinor"></param>
        /// <param name="flattening"></param>
        /// <param name="inverseFlattening"></param>
        private Elipsoid(double semiMajor, double semiMinor, double aplt, double inverseAplt)
        {
            mSemiAxaMajora = semiMajor;
            mSemiAxaMinora = semiMinor;
            mAplatizarea = aplt;
            mAplatizareaInversa = inverseAplt;
        }

        #region References Ellipsoids
        /// <summary>The WGS84 ellipsoid.</summary>
        static public readonly Elipsoid WGS84 = FromAAndInverseF(6378137.0, 298.257223563);

        /// <summary>The GRS80 ellipsoid.</summary>
        static public readonly Elipsoid GRS80 = FromAAndInverseF(6378137.0, 298.257222101);

        /// <summary>The GRS67 ellipsoid.</summary>
        static public readonly Elipsoid GRS67 = FromAAndInverseF(6378160.0, 298.25);

        /// <summary>The ANS ellipsoid.</summary>
        static public readonly Elipsoid ANS = FromAAndInverseF(6378160.0, 298.25);

        /// <summary>The WGS72 ellipsoid.</summary>
        static public readonly Elipsoid WGS72 = FromAAndInverseF(6378135.0, 298.26);

        /// <summary>The Clarke1858 ellipsoid.</summary>
        static public readonly Elipsoid Clarke1858 = FromAAndInverseF(6378293.645, 294.26);

        /// <summary>The Clarke1880 ellipsoid.</summary>
        static public readonly Elipsoid Clarke1880 = FromAAndInverseF(6378249.145, 293.465);

        /// <summary>A spherical "ellipsoid".</summary>
        static public readonly Elipsoid Sphere = FromAAndF(6371000, 0.0);
        #endregion

        /// <summary>
        /// Build an Ellipsoid from the semi major axis measurement and the inverse flattening.
        /// </summary>
        /// <param name="semiMajor">semi major axis (meters)</param>
        /// <param name="inverseFlattening"></param>
        /// <returns></returns>
        static public Elipsoid FromAAndInverseF(double semiMajor, double inverseAplt)
        {
            double f = 1.0 / inverseAplt;
            double b = (1.0 - f) * semiMajor;

            return new Elipsoid(semiMajor, b, f, inverseAplt);
        }

        /// <summary>
        /// Build an Ellipsoid from the semi major axis measurement and the flattening.
        /// </summary>
        /// <param name="semiMajor">semi major axis (meters)</param>
        /// <param name="flattening"></param>
        /// <returns></returns>
        static public Elipsoid FromAAndF(double semiMajor, double aplt)
        {
            double inverseA = 1.0 / aplt;
            double b = (1.0 - aplt) * semiMajor;

            return new Elipsoid(semiMajor, b, aplt, inverseA);
        }

        /// <summary>Get semi major axis (meters).</summary>
        public double SemiAxaMajora
        {
            get { return mSemiAxaMajora; }
        }

        /// <summary>Get semi minor axis (meters).</summary>
        public double SemiAxaMinora
        {
            get { return mSemiAxaMinora; }
        }

        /// <summary>Get flattening.</summary>
        public double Aplatizarea
        {
            get { return mAplatizarea; }
        }

        /// <summary>Get inverse flattening.</summary>
        public double AplatizareaInversa
        {
            get { return mAplatizareaInversa; }
        }
    }
}
