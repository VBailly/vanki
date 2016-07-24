using NUnit.Framework;
using System;
using System.IO;
using Vanki;

namespace Test
{
	[TestFixture]
	public class TestsWithASingleQuestion
	{
		private const string NoNextQuestionMessage = "There is no next question\n";
		private const string NextQuestionMessage = "The next question is:\n\"What is red?\"\n";
		private const string CorrectAnswerMessage = "That is a correct answer!\n";
		private const string WrongAnswerMessage = "WRONG! The correct answer is \"a color\".\n";
		private const string CannotAnswerMessage = "You cannot answer because there is no question pending\n";
		private const string NewEntryMessage = "New entry registered\n";

		[SetUp]
		public void SetUp()
		{
			File.Delete("db.xml");
		}

		[TearDown]
		public void TearDown()
		{
		}

		[Test]
		public void We_can_ask_twice_for_a_question()
		{
			var time = DateTime.Now;
			RegisterQuestion();
			AskForNextQuestion(time);

			var response = AskForNextQuestion(time);

			Assert.AreEqual(NextQuestionMessage, response);

		}

		[Test]
		public void We_cannot_answer_when_there_is_no_question()
		{
			var time = DateTime.Now;
			RegisterQuestion();
			AnswerCorrectly(time);
			AskForNextQuestion(time);

			var response = AnswerCorrectly(time);

			Assert.AreEqual(CannotAnswerMessage, response);

		}

		[Test]
		public void Wrong_answers_dont_pass()
		{
			var time = DateTime.Now;
			RegisterQuestion();

			var response = AnswerWrongly(time);

			Assert.AreEqual(WrongAnswerMessage, response);
		}

		[Test]
		public void An_wrong_answer_is_not_treated_if_no_question_is_pending()
		{
			var time = DateTime.Now;
			RegisterQuestion();
			AnswerCorrectly(time);

			var response = AnswerWrongly(time);

			Assert.AreEqual(CannotAnswerMessage, response);
		}

		[Test]
		public void Register_a_new_entry()
		{
			var response = RegisterQuestion();

			Assert.AreEqual(NewEntryMessage, response);
		}

		[Test]
		public void A_question_is_available_straight_after_being_registered()
		{
			RegisterQuestion();

			var response = AskForNextQuestion(DateTime.Now);

			Assert.AreEqual(NextQuestionMessage, response);
		}

		[Test]
		public void Giving_a_correct_answer_for_the_first_time()
		{
			RegisterQuestion();

			var response = AnswerCorrectly(DateTime.Now);

			Assert.AreEqual(CorrectAnswerMessage, response);
		}

		[Test]
		public void There_is_no_question_just_after_having_answered_it()
		{
			RegisterQuestion();
			AnswerCorrectly(DateTime.Now);

			var response = AskForNextQuestion(DateTime.Now);

			Assert.AreEqual(NoNextQuestionMessage, response);
		}

		[Test]
		public void There_is_still_no_question_1min_after_having_answered_it()
		{
			var time = DateTime.Now;
			RegisterQuestion();
			AnswerCorrectly(time);
			time += TimeSpan.FromMinutes(1);

			var response = AskForNextQuestion(time);

			Assert.AreEqual(NoNextQuestionMessage, response);
		}

		[Test]
		public void There_is_a_question_3min_after_having_answered_it()
		{
			var time = DateTime.Now;
			RegisterQuestion();
			AnswerCorrectly(time);
			time += TimeSpan.FromMinutes(3);

			var response = AskForNextQuestion(time);

			Assert.AreEqual(NextQuestionMessage, response);
		}

		[Test]
		public void We_can_answer_again_after_3min_from_first_answer()
		{
			var time = DateTime.Now;
			RegisterQuestion();
			AnswerCorrectly(time);
			time += TimeSpan.FromMinutes(3);

			var response = AnswerCorrectly(time);

			Assert.AreEqual(CorrectAnswerMessage, response);
		}

		[Test]
		public void There_is_no_next_question_directly_after_the_second_answer()
		{
			var time = DateTime.Now;
			RegisterQuestion();
			AnswerCorrectly(time);
			time += TimeSpan.FromMinutes(3);
			AnswerCorrectly(time);

			var response = AskForNextQuestion(time);

			Assert.AreEqual(NoNextQuestionMessage, response);
		}

		[Test]
		public void There_is_no_next_question_5_min_after_the_second_answer ()
		{
			var time = DateTime.Now;
			RegisterQuestion();
			AnswerCorrectly(time);
			time += TimeSpan.FromMinutes(3); // +3
			AnswerCorrectly(time);
			AskForNextQuestion(time);
			time += TimeSpan.FromMinutes(5); // +8

			var response = AskForNextQuestion(time);

			Assert.AreEqual(NoNextQuestionMessage, response);
		}

		[Test]
		public void There_is_no_next_question_if_we_dont_register_one()
		{
			var response = AskForNextQuestion(DateTime.Now);

			Assert.AreEqual(NoNextQuestionMessage, response);
		}

		[Test]
		public void The_question_stays_next_if_we_answer_wrongly()
		{
			var time = DateTime.Now;
			RegisterQuestion();
			AnswerWrongly(time);

			var response = AskForNextQuestion(time);

			Assert.AreEqual(NextQuestionMessage, response);
		}

		[Test]
		public void A_wrong_answer_resets_the_lapse()
		{
			var time = DateTime.Now;
			RegisterQuestion();
			AnswerCorrectly(time);
			AskForNextQuestion(time);
			time += TimeSpan.FromMinutes(3); // +3
			AnswerWrongly(time); // reset
			AnswerCorrectly(time);
			time += TimeSpan.FromMinutes(3); // +3 after reset

			var response = AskForNextQuestion(time);

			Assert.AreEqual(NextQuestionMessage, response);
		}

		static string AnswerWrongly(DateTime time)
		{
			return MainClass.TestableMain(new[] { "--answer", "an animal" }, time);
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

