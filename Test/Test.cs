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
			var response = MainClass.TestableMain (new []{"--add", "What is red?", "a color"});
			Assert.AreEqual ("New entry registered\n", response);

			response = MainClass.TestableMain (new []{"--next"});
			Assert.AreEqual ("The next question is:\n\"What is red?\"\n", response);

			response = MainClass.TestableMain (new []{"--answer", "a color"});
			Assert.AreEqual ("That is a correct answer!\n", response);

		}
	}
}

