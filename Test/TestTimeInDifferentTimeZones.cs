using System;
using NUnit.Framework;
using Storage;

namespace Test
{
    [TestFixture]
    public class TestTimeInDifferentTimeZones
    {
        [SetUp]
        public void SetUp()
        {
            Repository.StoreString(string.Empty);
            Clock.LocalTimeGetter = null;
            Clock.HoursDifferenceFromGlobal = null;
        }


    }
}

