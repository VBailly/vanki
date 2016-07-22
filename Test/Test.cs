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
		public void TestCase1 ()
		{
			var response = MainClass.TestableMain (new []{"What is red?", "a color"});
			Assert.AreEqual ("New entry registered\n", response);
		}
	}
}

