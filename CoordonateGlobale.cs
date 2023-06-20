using System;
using System.Collections.Generic;
using System.Text;

namespace DistantaVincentysFormule
{
    /// <summary>
    /// Incapsularea coordonatelor de latitudine si longitudine pe un glob.
    /// latitudine negatica inseamna ca locatia se afla in emisfera sudica, 
    /// longitudinea negatica inseamna ca punctul se afla emisfera vestica.
    /// Oirce unghi poate fi specificat pentru long si lat, dar toate unghiurile
    /// vor fi considerate astfel:
    /// 
    ///      -90 <= latitudine <= +90
    ///     -180 < longitudine <= +180
    /// </summary>
    [Serializable]
    public struct CoordonateGlobale : IComparable<CoordonateGlobale>
    {
        /// <summary>Latitudine. Lat negativa => emisfera sudica.</summary>
        private Unghiul mLatitudine;

        /// <summary>Longitudine.  Long negativa => emisfera vestica</summary>
        private Unghiul mLongitudine;

        /// <summary>
        /// Aducerea la forma canonica a valorile curente pentru lat si long, astfel incat:
        /// 
        ///      -90 <= latitudine <= +90
        ///     -180 < longitudine <= +180
        /// </summary>
        private void Canonicalize()
        {
            double latitudine = mLatitudine.Grade;
            double longitudine = mLongitudine.Grade;

            latitudine = (latitudine + 180) % 360;
            if (latitudine < 0) latitudine += 360;
            latitudine -= 180;

            if (latitudine > 90)
            {
                latitudine = 180 - latitudine;
                latitudine += 180;
            }
            else if (latitudine < -90)
            {
                latitudine = -180 - latitudine;
                latitudine += 180;
            }

            longitudine = ((longitudine + 180) % 360);
            if (longitudine <= 0) longitudine += 360;
            longitudine -= 180;

            mLatitudine = new Unghiul(latitudine);
            mLongitudine = new Unghiul(longitudine);
        }

        /// <summary>
        /// Construim un nou CoordonateGlobale.  Unghiurile vor fi 'canonicalized'.
        /// </summary>
        /// <param name="latitude">latitude</param>
        /// <param name="longitude">longitude</param>
        public CoordonateGlobale(Unghiul latitudine, Unghiul longitudine)
        {
            mLatitudine = latitudine;
            mLongitudine = longitudine;
            Canonicalize();
        }

        /// <summary>
        /// Get/set latitude.  Valoarea latitudinii va fii 'canonicalized' (ceea ce ar putea schimba
        /// rezultatul longitudinii). Latitudine negativa = emisfera sudica.
        /// </summary>
        public Unghiul Latitudine
        {
            get { return mLatitudine; }
            set
            {
                mLatitudine = value;
                Canonicalize();
            }
        }

        /// <summary>
        /// Get/set longitude.  Valoarea longitudinii va fii 'canonicalized'.
        /// Longitudine negativa = emisfera vestica
        /// </summary>
        public Unghiul Longitudine
        {
            get { return mLongitudine; }
            set
            {
                mLongitudine = value;
                Canonicalize();
            }
        }

        /// <summary>
        /// Comparam aceste coordonate cu un alt set de coordonate.
        /// Longitudinile vestice sunt mai mici decat cele estice. Daca longitudinele sunt egale,
        /// atunci latitudinea sudica este mai micada decat latitudinea norica.
        /// </summary>
        /// <param name="other">instance to compare to</param>
        /// <returns>-1, 0, or +1 as per IComparable contract</returns>
        public int CompareTo(CoordonateGlobale other)
        {
            int retval;

            if (mLongitudine < other.mLongitudine) retval = -1;
            else if (mLongitudine > other.mLongitudine) retval = +1;
            else if (mLatitudine < other.mLatitudine) retval = -1;
            else if (mLatitudine > other.mLatitudine) retval = +1;
            else retval = 0;

            return retval;
        }

        /// <summary>
        /// hash code pentru coordonate.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ((int)(mLongitudine.GetHashCode() * (mLatitudine.GetHashCode() + 1021))) * 1000033;
        }

        /// <summary>
        /// Comparam aceste coordonate cu unal obiect pentru egalitate
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is CoordonateGlobale)) return false;

            CoordonateGlobale other = (CoordonateGlobale)obj;

            return (mLongitudine == other.mLongitudine) && (mLatitudine == other.mLatitudine);
        }

        /// <summary>
        /// Coordonate ca si string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(mLatitudine.Abs().ToString());
            builder.Append((mLatitudine >= Unghiul.Zero) ? 'N' : 'S');
            builder.Append(';');
            builder.Append(mLongitudine.Abs().ToString());
            builder.Append((mLongitudine >= Unghiul.Zero) ? 'E' : 'W');
            builder.Append(';');

            return builder.ToString();
        }
    }
}
