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
		public void We_can_ask_twice_for_a_question()
		{

			var time = DateTime.Now;

			var response = MainClass.TestableMain (new []{"-q", "What is red?", "-a", "a color"}, DateTime.Now);
			Assert.AreEqual ("New entry registered\n", response);

			response = MainClass.TestableMain (new []{"--next"}, time);
			Assert.AreEqual ("The next question is:\n\"What is red?\"\n", response);

			response = MainClass.TestableMain (new []{"--next"}, time);
			Assert.AreEqual ("The next question is:\n\"What is red?\"\n", response);

		}

		[Test ()]
		public void We_cannot_answer_when_there_is_no_question ()
		{

			var time = DateTime.Now;

			var response = MainClass.TestableMain (new []{"-q", "What is red?", "-a", "a color"}, DateTime.Now);
			Assert.AreEqual ("New entry registered\n", response);

			response = MainClass.TestableMain (new []{"--next"}, time);
			Assert.AreEqual ("The next question is:\n\"What is red?\"\n", response);


			response = MainClass.TestableMain (new []{"--answer", "a color"}, time);
			Assert.AreEqual ("That is a correct answer!\n", response);

			response = MainClass.TestableMain (new []{"--next"}, time);
			Assert.AreEqual ("There is no next question\n", response);

			response = MainClass.TestableMain (new []{"--answer", "a color"}, time);
			Assert.AreEqual ("You cannot answer because there is no question pending\n", response);

		}

		[Test ()]
		public void We_need_to_answer_correct_to_pass ()
		{

			var time = DateTime.Now;

			var response = MainClass.TestableMain (new []{"-q", "What is red?", "-a", "a color"}, DateTime.Now);
			Assert.AreEqual ("New entry registered\n", response);

			response = MainClass.TestableMain (new []{"--next"}, time);
			Assert.AreEqual ("The next question is:\n\"What is red?\"\n", response);


			response = MainClass.TestableMain (new []{"--answer", "an animal"}, time);
			Assert.AreEqual ("WRONG! The correct answer is \"a color\".\n", response);
		}

		[Test ()]
		public void TestCase1 ()
		{

			var time = DateTime.Now;
			
			var response = MainClass.TestableMain (new []{"-q", "What is red?", "-a", "a color" }, DateTime.Now);
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

			time += TimeSpan.FromMinutes (5); // +8
			response = MainClass.TestableMain (new []{"--next"}, time);
			Assert.AreEqual ("There is no next question\n", response);

		}
	}
}

