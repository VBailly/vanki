using NUnit.Framework;
using System;
using System.IO;
using Vanki;

namespace Test
{
	[TestFixture()]
	public class Test
	{
		private const string NoNextQuestionMessage = "There is no next question\n";

		[SetUp]
		public void SetUp()
		{
			File.Delete("db.xml");
		}

		[TearDown]
		public void TearDown()
		{
		}

		[Test()]
		public void We_can_ask_twice_for_a_question()
		{
			var time = DateTime.Now;
			RegisterQuestion();
			AskForNextQuestion(time);

			var response = AskForNextQuestion(time);

			Assert.AreEqual("The next question is:\n\"What is red?\"\n", response);

		}

		[Test()]
		public void We_cannot_answer_when_there_is_no_question()
		{
			var time = DateTime.Now;
			RegisterQuestion();
			AnswerCorrectly(time);
			AskForNextQuestion(time);

			var response = AnswerCorrectly(time);

			Assert.AreEqual("You cannot answer because there is no question pending\n", response);

		}

		[Test()]
		public void A_wrong_answer_resets_the_lapse()
		{
			var time = DateTime.Now;

			RegisterQuestion();
			AnswerCorrectly(time);
			AskForNextQuestion(time);
			time += TimeSpan.FromMinutes(3); // +3

			var response = AskForNextQuestion(time);
			Assert.AreEqual("The next question is:\n\"What is red?\"\n", response);

			response = MainClass.TestableMain(new[] { "--answer", "an animal" }, time);
			Assert.AreEqual("WRONG! The correct answer is \"a color\".\n", response);

			response = AskForNextQuestion(time);
			Assert.AreEqual("The next question is:\n\"What is red?\"\n", response);

			response = MainClass.TestableMain(new[] { "--answer", "a color" }, time);
			Assert.AreEqual("That is a correct answer!\n", response);

			time += TimeSpan.FromMinutes(3); // +8

			response = AskForNextQuestion(time);
			Assert.AreEqual("The next question is:\n\"What is red?\"\n", response);
		}

		[Test()]
		public void Wrong_answers_dont_pass()
		{
			var time = DateTime.Now;
			RegisterQuestion();

			var response = MainClass.TestableMain(new[] { "--answer", "an animal" }, time);

			Assert.AreEqual("WRONG! The correct answer is \"a color\".\n", response);
		}

		[Test()]
		public void An_wrong_answer_is_not_treated_if_no_question_is_pending()
		{
			var time = DateTime.Now;
			RegisterQuestion();
			AnswerCorrectly(time);

			var response = MainClass.TestableMain(new[] { "--answer", "an animal" }, time);

			Assert.AreEqual("You cannot answer because there is no question pending\n", response);
		}

		[Test()]
		public void Register_a_new_entry()
		{
			var response = RegisterQuestion();

			Assert.AreEqual("New entry registered\n", response);
		}

		[Test()]
		public void A_question_is_available_straight_after_being_registered()
		{
			RegisterQuestion();

			var response = AskForNextQuestion(DateTime.Now);

			Assert.AreEqual("The next question is:\n\"What is red?\"\n", response);
		}

		[Test()]
		public void Giving_a_correct_answer_for_the_first_time()
		{
			RegisterQuestion();

			var response = AnswerCorrectly(DateTime.Now);

			Assert.AreEqual("That is a correct answer!\n", response);
		}

		[Test()]
		public void There_is_no_question_just_after_having_answered_it()
		{
			RegisterQuestion();
			AnswerCorrectly(DateTime.Now);

			var response = AskForNextQuestion(DateTime.Now);

			Assert.AreEqual(NoNextQuestionMessage, response);
		}

		[Test()]
		public void There_is_still_no_question_1min_after_having_answered_it()
		{
			var time = DateTime.Now;
			RegisterQuestion();
			AnswerCorrectly(time);
			time += TimeSpan.FromMinutes(1);

			var response = AskForNextQuestion(time);

			Assert.AreEqual(NoNextQuestionMessage, response);

		}

		[Test ()]
		public void TestCase1 ()
		{

			var time = DateTime.Now;

			RegisterQuestion();
			AnswerCorrectly(time);

			time += TimeSpan.FromMinutes(3); // +3

			var response = AskForNextQuestion(time);
			Assert.AreEqual("The next question is:\n\"What is red?\"\n", response);

			response = MainClass.TestableMain(new[] { "--answer", "a color" }, time);
			Assert.AreEqual("That is a correct answer!\n", response);

			response = AskForNextQuestion(time);
			Assert.AreEqual(NoNextQuestionMessage, response);

			time += TimeSpan.FromMinutes(5); // +8
			response = AskForNextQuestion(time);
			Assert.AreEqual(NoNextQuestionMessage, response);

		}

		static string AnswerCorrectly(DateTime time)
		{
			return MainClass.TestableMain(new[] { "--answer", "a color" }, time);
		}

		static string AskForNextQuestion(DateTime time)
		{
			return MainClass.TestableMain(new[] { "--next" }, time);
		}

		static string RegisterQuestion()
		{
			return MainClass.TestableMain(new[] { "-q", "What is red?", "-a", "a color" }, DateTime.Now);
		}
	}
}

