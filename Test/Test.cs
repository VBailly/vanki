using NUnit.Framework;
using System;
using System.IO;
using Vanki;

namespace Test
{
	[TestFixture ()]
	public class Test
	{


		[SetUp]
		public void SetUp ()
		{
		}

		[TearDown]
		public void TearDown ()
		{
		}


		[Test ()]
		public void TestCase ()
		{
			Assert.AreEqual ("Hello World!\n", MainClass.TestableMain (null));
		}
	}
}

