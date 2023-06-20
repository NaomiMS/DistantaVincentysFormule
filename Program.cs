
using System;
using System.Text;

namespace DistantaVincentysFormule
{
    public class Example
    {
        /// <summary>
        /// Calcularea directiei 2 dimensionala; +distanta elipsoidala de la 
        ///    Academia Platonica, Atena, D.C (coordonatele geografice in radiani) --> 37.9925N, 23.70805E
        ///         la
        ///    Universitatea din Berlin --> 52.51805N, 13.3933E
        ///         folosind ca referinta sistemul WGS84
        /// </summary>
        static void CalculDoiDimensional()
        {
            // instantiate the calculator
            CalculatorGeodezica geoCalc = new CalculatorGeodezica();

            // select a reference elllipsoid
            Elipsoid reference = Elipsoid.WGS84;

            // setam coordonatele Academiei Platonice
            /*
            CoordonateGlobale academiaPlatonica;
            academiaPlatonica = new CoordonateGlobale(
                new Unghiul(37.9925), new Unghiul(23.70805)
            );

            // Universitatea din Berlin
            CoordonateGlobale univBerlin;
            univBerlin = new CoordonateGlobale(
                new Unghiul(52.51805), new Unghiul(13.3933)
            );*/

            const string Deg = "°";

            double ParseToString(string str)
            {
                str = str.ToUpper().Replace(Deg, " ").Replace("'", " ").Replace("\"", " ");
                str = str.Replace("S", " S").Replace("N", " N");
                str = str.Replace("E", " E").Replace("W", " W");
                char[] separators = { ' ' };
                string[] fields = str.Split(separators,
                    StringSplitOptions.RemoveEmptyEntries);

                double result =             // Degrees.
                    double.Parse(fields[0]);
                if (fields.Length > 2)      // Minutes.
                    result += double.Parse(fields[1]) / 60;
                if (fields.Length > 3)      // Seconds.
                    result += double.Parse(fields[2]) / 3600;
                if (str.Contains('S') || str.Contains('W')) result *= -1;

                return result;
            }

            CoordonateGlobale primulPunct;
             /*double latitudine_1;
             double longitudine_1;*/

            string latitudine_1;
            string longitudine_1;
            Console.Write("Introduceti latitudinea primului punct:");
            //latitudine_1 = double.Parse(Console.ReadLine());
            latitudine_1 = Console.ReadLine();
            Console.Write("Introduceti longitudinea primului punct:");
            //longitudine_1 = double.Parse(Console.ReadLine());
            longitudine_1 = Console.ReadLine();
            //primulPunct = new CoordonateGlobale(new Unghiul(latitudine_1), new Unghiul(longitudine_1));
            double lat1 = ParseToString(latitudine_1);
            Console.WriteLine();
            Console.WriteLine("Latitudinea in grade:" + ParseToString(latitudine_1));
            double lon1 = ParseToString(longitudine_1);
            Console.WriteLine("Longitudinea in grade:" + ParseToString(longitudine_1));
            Console.WriteLine();
            primulPunct = new CoordonateGlobale(new Unghiul(lat1), new Unghiul(lon1));
            Console.WriteLine();

            CoordonateGlobale secundPunct;
            /*double latitudine_2;
            double longitudine_2;*/
            string latitudine_2;
            string longitudine_2;
            Console.Write("Introduceti latitudinea celui de-al doilea punct:");
             //latitudine_2 = double.Parse(Console.ReadLine());
            latitudine_2 = Console.ReadLine();
            Console.Write("Introduceti longitudinea celui de-al doilea punct:");
             //longitudine_2 = double.Parse(Console.ReadLine());
            longitudine_2 = Console.ReadLine();
            //secundPunct = new CoordonateGlobale(new Unghiul(latitudine_2), new Unghiul(longitudine_2));
            Console.WriteLine();
            double lat2 = ParseToString(latitudine_2);
            Console.WriteLine("Latitudinea in grade:" + ParseToString(latitudine_2));
            double lon2 = ParseToString(longitudine_2);
            Console.WriteLine("Longitudinea in grade:" + ParseToString(longitudine_2));
            Console.WriteLine();
            secundPunct = new CoordonateGlobale(new Unghiul(lat2), new Unghiul(lon2));




            // calculam curba geodezica
            GeodezicCurba geoCurve = geoCalc.CalculateGeodeticCurve(reference, primulPunct, secundPunct);
            double ellipseKilometers = geoCurve.DistantaElipsoidala / 1000.0;
            //double ellipseMiles = ellipseKilometers * 0.621371192;

            Console.WriteLine("Directia 2-dimensionala de la Academia Platonica spre Universitatea " +
                "din Berlin folosind sistemul geodezic WGS84");
            Console.WriteLine("Distanta elipsoidala: {0:0.00} km", ellipseKilometers);
            Console.WriteLine("Azimut: {0:0.00} grade", geoCurve.Azimuth.Grade);
            Console.WriteLine("Invers Azimut (directia opusa): {0:0.00} grade", geoCurve.ReverseAzimuth.Grade);
        }

        /// <summary>
        /// Calculate the three-dimensional path from
        ///    Pike's Peak in Colorado --> 38.840511N, 105.0445896W, 4301 meters
        ///        to
        ///    Alcatraz Island --> 37.826389N, 122.4225W, sea level
        ///        using
        ///    WGS84 reference ellipsoid
        /// </summary>
        static void ThreeDimensionalCalculation()
        {
            // instantiate the calculator
            CalculatorGeodezica geoCalc = new CalculatorGeodezica();

            // select a reference elllipsoid
            Elipsoid reference = Elipsoid.WGS84;

            // set Pike's Peak position
            PozitiiGlobale pikesPeak;
            pikesPeak = new PozitiiGlobale(
              new CoordonateGlobale(new Unghiul(37.9925), new Unghiul(23.70805)),
              4301.0
            );

            // set Alcatrazt Island coordinates
            PozitiiGlobale alcatrazIsland;
            alcatrazIsland = new PozitiiGlobale(
              new CoordonateGlobale(new Unghiul(52.51805), new Unghiul(13.3933)),
              0.0
            );

            // calculate the geodetic measurement
            GeodezicMasurare geoMeasurement;
            double p2pKilometers;
            double p2pMiles;
            double elevChangeMeters;
            double elevChangeFeet;

            geoMeasurement = geoCalc.CalculateGeodeticMeasurement(reference, pikesPeak, alcatrazIsland);
            p2pKilometers = geoMeasurement.PointToPointDistance / 1000.0;
            p2pMiles = p2pKilometers * 0.621371192;
            elevChangeMeters = geoMeasurement.ElevationChange;
            elevChangeFeet = elevChangeMeters * 3.2808399;

            Console.WriteLine("3-D 'path' de la Academia Platonica spre Universitatea din Berlin folosind WGS84");
            Console.WriteLine("   Distanta punct la punct: {0:0.00} kilometers ({1:0.00} miles)", p2pKilometers, p2pMiles);
            Console.WriteLine("   Elevation change: {0:0.0} mettrii ({1:0.0} feet)", elevChangeMeters /*elevChangeFeet*/);
            Console.WriteLine("   Azimut: {0:0.00} grade", geoMeasurement.Azimuth.Grade);
            Console.WriteLine("   Invers Azimut (directia opusa): {0:0.00} grade", geoMeasurement.ReverseAzimuth.Grade);
        }

        static void Main()
        {
            CalculDoiDimensional();

            Console.WriteLine();

            //ThreeDimensionalCalculation();

            //Console.ReadLine();
        }
    }
}
