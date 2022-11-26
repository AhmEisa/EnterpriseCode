using Enterprise.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Enterprise.UnitTest.Algorithms
{
    public class QueuesTests
    {
        [Fact]
        public void DownToZero_Test()
        {
            var result = Queues.downToZero(3);

            Assert.Equal(3, result);
        }

        [Fact]
        public void DownToZero_Test2()
        {
            var result = Queues.downToZero(4);

            Assert.Equal(3, result);
        }

        [Fact]
        public void DownToZero_Test3()
        {
            var result = Queues.downToZero(966514);

            Assert.Equal(8, result);
        }

        [Fact]
        public void DownToZero_Test5()
        {
            var result = Queues.downToZero(812849);

            Assert.Equal(10, result);
        }

        [Fact]
        public void DownToZero_Test6()
        {
            var result = Queues.downToZero(15);
            Assert.Equal(5, result);
        }

        [Fact]
        public void DownToZero_Test4()
        {
            var testData = DownToZeroLargeStringTest.Split("\n").Select(r => Convert.ToInt32(r.Replace("\r", string.Empty))).ToList();
            var testResultData = DownToZeroLargeStringTestResult.Split("\n").Select(r => Convert.ToInt32(r.Replace("\r", string.Empty))).ToList();
            for (int i = 0; i < testData.Count; i++)
            {
                var result = Queues.downToZero(testData[i]);
                Assert.Equal(testResultData[i], result);
            }
        }

        private string DownToZeroLargeStringTest = @"966514
812849
808707
360422
691410
691343
551065
432560
192658
554548
27978
951717
663795
315528
522506
300432
412509
109052
614346
589115
301840
7273
193764
702818
639354
584658
208828
255463
506460
471454
554516
739987
303876
813024
118681
708473
616288
962466
55094
599778
385504
428443
646717
572077
463452
750219
725457
672957
750371
542716
87017
743756
293742
301031
939025
503398
334595
209039
191818
158563
617470
118260
176581
966721
48924
235330
200174
992221
411098
559560
117381
814728
795418
309832
943111
775314
875208
168234
933574
444474
995856
687362
543687
761831
952514
970724
611269
237583
88891
708888
387629
407891
393991
577592";
        private string DownToZeroLargeStringTestResult = @"8
10
8
11
9
11
9
9
10
10
9
10
7
9
9
9
8
9
8
8
7
9
8
10
8
7
8
8
7
8
9
9
7
9
8
9
10
9
8
7
9
8
9
8
9
10
8
9
8
8
8
10
10
11
10
9
9
10
9
9
9
7
8
8
7
8
10
8
11
9
9
9
9
10
8
9
10
9
9
9
9
9
9
9
9
10
8
9
10
8
8
9
8
8";
    }
}
