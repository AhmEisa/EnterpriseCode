using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Framework.Topics
{
    // monoid is an associative binary operation with a neutral element (also known as identity).

    //Angular addition monoid
    public struct Angle
    {
        private readonly decimal degrees;
        private Angle(decimal degrees)
        {
            this.degrees = degrees % 360m;
            if (this.degrees < 0)
                this.degrees += 360m;
        }

        public static Angle FromDegrees(decimal degrees)
        {
            return new Angle(degrees);
        }
        public static Angle FromRadians(double radians)
        {
            return new Angle((decimal)((180D / Math.PI) * radians));
        }
        public Angle Add(Angle other)
        {
            return new Angle(this.degrees + other.degrees);
        }
        
        public readonly static Angle Identity = new Angle(0);

        public override bool Equals(object obj)
        {
            if (obj is Angle)
                return ((Angle)obj).degrees == this.degrees;

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.degrees.GetHashCode();
        }

        public static bool operator ==(Angle x, Angle y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Angle x, Angle y)
        {
            return !x.Equals(y);
        }


    }
}
