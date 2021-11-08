using Enterprise.Framework.Topics;
using FsCheck;
using FsCheck.Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Enterprise.UnitTest.Topics
{
    public class MonoidTests
    {
        [Property(QuietOnSuccess = true)]
        public void AddIsAssociative(Angle x, Angle y, Angle z)
        {
            Assert.Equal(x.Add(y).Add(z), x.Add(y.Add(z)));
        }

        [Property(QuietOnSuccess = true)]
        public void AddHasIdentity(Angle x)
        {
            Assert.Equal(x, Angle.Identity.Add(x));
            Assert.Equal(x, x.Add(Angle.Identity));
        }

        [Property(QuietOnSuccess = true)]
        public void ConcatIsAssociative(int[] xs, int[] ys, int[] zs)
        {
            Assert.Equal(xs.Concat(ys).Concat(zs), xs.Concat(ys.Concat(zs)));
        }

        [Property(QuietOnSuccess = true)]
        public void ConcatHasIdentity(int[] xs)
        {
            Assert.Equal(Enumerable.Empty<int>().Concat(xs), xs.Concat(Enumerable.Empty<int>()));
            Assert.Equal(xs, xs.Concat(Enumerable.Empty<int>()));
        }

        [Property(QuietOnSuccess = true)]
        public void PlusIsAssociative(string x, string y, string z)
        {
            Assert.Equal((x + y) + z, x + (y + z));
        }
        [Property(QuietOnSuccess = true)]
        public void PlusHasIdentity(NonNull<string> x)
        {
            Assert.Equal("" + x.Get, x.Get + "");
            Assert.Equal(x.Get, x.Get + "");
        }

    }
}
