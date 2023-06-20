using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace DistantaVincentysFormule
{
    public class CalculatorGeodezica { 
    private const double TwoPi = 2.0 * Math.PI;

    /// <summary>
    /// Calculate the geodetic curve between two points on a specified reference ellipsoid.
    /// </summary>
    /// <param name="ellipsoid">reference ellipsoid to use</param>
    /// <param name="start">starting coordinates</param>
    /// <param name="end">ending coordinates </param>
    /// <returns></returns>
    public GeodezicCurba CalculateGeodeticCurve(Elipsoid ellipsoid, CoordonateGlobale start, CoordonateGlobale end)
    {
        //
        // All equation numbers refer back to Vincenty's publication:
        // See http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf
        //

        // get constants
        double a = ellipsoid.SemiAxaMajora;
        double b = ellipsoid.SemiAxaMinora;
        double f = ellipsoid.Aplatizarea;

        // get parameters as radians
        double phi1 = start.Latitudine.Radiani;
        double lambda1 = start.Longitudine.Radiani;
        double phi2 = end.Latitudine.Radiani;
        double lambda2 = end.Longitudine.Radiani;

        // calculations
        double a2 = a * a;
        double b2 = b * b;
        double a2b2b2 = (a2 - b2) / b2;

        double omega = lambda2 - lambda1;

        double tanphi1 = Math.Tan(phi1);
        double tanU1 = (1.0 - f) * tanphi1;
        double U1 = Math.Atan(tanU1);
        double sinU1 = Math.Sin(U1);
        double cosU1 = Math.Cos(U1);

        double tanphi2 = Math.Tan(phi2);
        double tanU2 = (1.0 - f) * tanphi2;
        double U2 = Math.Atan(tanU2);
        double sinU2 = Math.Sin(U2);
        double cosU2 = Math.Cos(U2);

        double sinU1sinU2 = sinU1 * sinU2;
        double cosU1sinU2 = cosU1 * sinU2;
        double sinU1cosU2 = sinU1 * cosU2;
        double cosU1cosU2 = cosU1 * cosU2;

        // eq. 13
        double lambda = omega;

        // intermediates we'll need to compute 's'
        double A = 0.0;
        double B = 0.0;
        double sigma = 0.0;
        double deltasigma = 0.0;
        double lambda0;
        bool converged = false;

        for (int i = 0; i < 10; i++)
        {
            lambda0 = lambda;

            double sinlambda = Math.Sin(lambda);
            double coslambda = Math.Cos(lambda);

            // eq. 14
            double sin2sigma = (cosU2 * sinlambda * cosU2 * sinlambda) + (cosU1sinU2 - sinU1cosU2 * coslambda) * (cosU1sinU2 - sinU1cosU2 * coslambda);
            double sinsigma = Math.Sqrt(sin2sigma);

            // eq. 15
            double cossigma = sinU1sinU2 + (cosU1cosU2 * coslambda);

            // eq. 16
            sigma = Math.Atan2(sinsigma, cossigma);

            // eq. 17    Careful!  sin2sigma might be almost 0!
            double sinalpha = (sin2sigma == 0) ? 0.0 : cosU1cosU2 * sinlambda / sinsigma;
            double alpha = Math.Asin(sinalpha);
            double cosalpha = Math.Cos(alpha);
            double cos2alpha = cosalpha * cosalpha;

            // eq. 18    Careful!  cos2alpha might be almost 0!
            double cos2sigmam = cos2alpha == 0.0 ? 0.0 : cossigma - 2 * sinU1sinU2 / cos2alpha;
            double u2 = cos2alpha * a2b2b2;

            double cos2sigmam2 = cos2sigmam * cos2sigmam;

            // eq. 3
            A = 1.0 + u2 / 16384 * (4096 + u2 * (-768 + u2 * (320 - 175 * u2)));

            // eq. 4
            B = u2 / 1024 * (256 + u2 * (-128 + u2 * (74 - 47 * u2)));

            // eq. 6
            deltasigma = B * sinsigma * (cos2sigmam + B / 4 * (cossigma * (-1 + 2 * cos2sigmam2) - B / 6 * cos2sigmam * (-3 + 4 * sin2sigma) * (-3 + 4 * cos2sigmam2)));

            // eq. 10
            double C = f / 16 * cos2alpha * (4 + f * (4 - 3 * cos2alpha));

            // eq. 11 (modified)
            lambda = omega + (1 - C) * f * sinalpha * (sigma + C * sinsigma * (cos2sigmam + C * cossigma * (-1 + 2 * cos2sigmam2)));

            // see how much improvement we got
            double change = Math.Abs((lambda - lambda0) / lambda);

            if ((i > 1) && (change < 0.0000000000001))
            {
                converged = true;
                break;
            }
        }

        // eq. 19
        double s = b * A * (sigma - deltasigma);
        Unghiul alpha1;
        Unghiul alpha2;

        // didn't converge?  must be N/S
        if (!converged)
        {
            if (phi1 > phi2)
            {
                alpha1 = Unghiul.Unghi180;
                alpha2 = Unghiul.Zero;
            }
            else if (phi1 < phi2)
            {
                alpha1 = Unghiul.Zero;
                alpha2 = Unghiul.Unghi180;
            }
            else
            {
                alpha1 = new Unghiul(Double.NaN);
                alpha2 = new Unghiul(Double.NaN);
            }
        }

        // else, it converged, so do the math
        else
        {
            double radians;
            alpha1 = new Unghiul();
            alpha2 = new Unghiul();

            // eq. 20
            radians = Math.Atan2(cosU2 * Math.Sin(lambda), (cosU1sinU2 - sinU1cosU2 * Math.Cos(lambda)));
            if (radians < 0.0) radians += TwoPi;
            alpha1.Radiani = radians;

            // eq. 21
            radians = Math.Atan2(cosU1 * Math.Sin(lambda), (-sinU1cosU2 + cosU1sinU2 * Math.Cos(lambda))) + Math.PI;
            if (radians < 0.0) radians += TwoPi;
            alpha2.Radiani = radians;
        }

        return new GeodezicCurba(s, alpha1, alpha2);
    }

    /// <summary>
    /// Calculate the three dimensional geodetic measurement between two positions
    /// measured in reference to a specified ellipsoid.
    /// 
    /// This calculation is performed by first computing a new ellipsoid by expanding or contracting
    /// the reference ellipsoid such that the new ellipsoid passes through the average elevation
    /// of the two positions.  A geodetic curve across the new ellisoid is calculated.  The
    /// point-to-point distance is calculated as the hypotenuse of a right triangle where the length
    /// of one side is the ellipsoidal distance and the other is the difference in elevation.
    /// </summary>
    /// <param name="refEllipsoid">reference ellipsoid to use</param>
    /// <param name="start">starting position</param>
    /// <param name="end">ending position</param>
    /// <returns></returns>
    public GeodezicMasurare CalculateGeodeticMeasurement(Elipsoid refEllipsoid, PozitiiGlobale start, PozitiiGlobale end)
    {
        // get the coordinates
        CoordonateGlobale startCoords = start.Coordinates;
        CoordonateGlobale endCoords = end.Coordinates;

        // calculate elevation differences
        double elev1 = start.Elevation;
        double elev2 = end.Elevation;
        double elev12 = (elev1 + elev2) / 2.0;

        // calculate latitude differences
        double phi1 = startCoords.Latitudine.Radiani;
        double phi2 = endCoords.Latitudine.Radiani;
        double phi12 = (phi1 + phi2) / 2.0;

        // calculate a new ellipsoid to accommodate average elevation
        double refA = refEllipsoid.SemiAxaMajora;
        double f = refEllipsoid.Aplatizarea;
        double a = refA + elev12 * (1.0 + f * Math.Sin(phi12));
        Elipsoid ellipsoid = Elipsoid.FromAAndF(a, f);

        // calculate the curve at the average elevation
        GeodezicCurba averageCurve = CalculateGeodeticCurve(ellipsoid, startCoords, endCoords);

        // return the measurement
        return new GeodezicMasurare(averageCurve, elev2 - elev1);
    }
}
}
