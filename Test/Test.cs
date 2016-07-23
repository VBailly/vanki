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
			File.Delete ("db.xml");
		}

		[TearDown]
		public void TearDown ()
		{
		}

		[Test ()]
		public void TestCase1 ()
		{

			var time = DateTime.Now;
			
			var response = MainClass.TestableMain (new []{"--add", "What is red?", "a color"}, DateTime.Now);
			Assert.AreEqual ("New entry registered\n", response);

			response = MainClass.TestableMain (new []{"--next"}, time);
			Assert.AreEqual ("The next question is:\n\"What is red?\"\n", response);

			response = MainClass.TestableMain (new []{"--answer", "a color"}, time);
			Assert.AreEqual ("That is a correct answer!\n", response);

			response = MainClass.TestableMain (new []{"--next"}, time);
			Assert.AreEqual ("There is no next question\n", response);

			time += TimeSpan.FromMinutes (1); // +1

			response = MainClass.TestableMain (new []{"--next"}, time);
			Assert.AreEqual ("There is no next question\n", response);

			time += TimeSpan.FromMinutes (2); // +3

			response = MainClass.TestableMain (new []{"--next"}, time);
			Assert.AreEqual ("The next question is:\n\"What is red?\"\n", response);

			response = MainClass.TestableMain (new []{"--answer", "a color"}, time);
			Assert.AreEqual ("That is a correct answer!\n", response);

			response = MainClass.TestableMain (new []{"--next"}, time);
			Assert.AreEqual ("There is no next question\n", response);

		}
	}
}

