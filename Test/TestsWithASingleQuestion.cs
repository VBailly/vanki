using NUnit.Framework;
using System;
using System.IO;

namespace Test
{
	[TestFixture]
	public class TestsWithASingleQuestion
	{
		const string NoNextQuestionMessage = ConsoleOutputs.NoNextQuestionMessage;
		static readonly string NextQuestionMessage = string.Format(ConsoleOutputs.NextQuestionMessage, "What is red?");
		const string CorrectAnswerMessage = ConsoleOutputs.CorrectAnswerMessage;
		static readonly string WrongAnswerMessage = string.Format(ConsoleOutputs.WrongAnswerMessage, "a color");
		const string CannotAnswerMessage = ConsoleOutputs.CannotAnswerMessage;
		const string NewEntryMessage = ConsoleOutputs.NewEntryMessage;

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
			RegisterQuestion(time);
			AskForNextQuestion(time);

			var response = AskForNextQuestion(time);

			Assert.AreEqual(NextQuestionMessage, response);

		}

		[Test]
		public void We_cannot_answer_when_there_is_no_question()
		{
			var time = DateTime.Now;
			RegisterQuestion(time);
			AnswerCorrectly(time);
			AskForNextQuestion(time);

			var response = AnswerCorrectly(time);

			Assert.AreEqual(CannotAnswerMessage, response);

		}

		[Test]
		public void Wrong_answers_dont_pass()
		{
			var time = DateTime.Now;
			RegisterQuestion(time);

			var response = AnswerWrongly(time);

			Assert.AreEqual(WrongAnswerMessage, response);
		}

		[Test]
		public void A_wrong_answer_is_not_treated_if_no_question_is_pending()
		{
			var time = DateTime.Now;
			RegisterQuestion(time);
			AnswerCorrectly(time);

			var response = AnswerWrongly(time);

			Assert.AreEqual(CannotAnswerMessage, response);
		}

		[Test]
		public void Register_a_new_entry()
		{
			var response = RegisterQuestion(DateTime.Now);

			Assert.AreEqual(NewEntryMessage, response);
		}

		[Test]
		public void A_question_is_available_straight_after_being_registered()
		{
			RegisterQuestion(DateTime.Now);

			var response = AskForNextQuestion(DateTime.Now);

			Assert.AreEqual(NextQuestionMessage, response);
		}

		[Test]
		public void Giving_a_correct_answer_for_the_first_time()
		{
			RegisterQuestion(DateTime.Now);

			var response = AnswerCorrectly(DateTime.Now);

			Assert.AreEqual(CorrectAnswerMessage, response);
		}

		[Test]
		public void There_is_no_question_just_after_having_answered_it()
		{
			RegisterQuestion(DateTime.Now);
			AnswerCorrectly(DateTime.Now);

			var response = AskForNextQuestion(DateTime.Now);

			Assert.IsTrue(response.Contains(NoNextQuestionMessage));
		}

		[Test]
		public void There_is_still_no_question_1min_after_having_answered_it()
		{
			var time = DateTime.Now;
			RegisterQuestion(time);
			AnswerCorrectly(time);
			time += TimeSpan.FromMinutes(1);

			var response = AskForNextQuestion(time);

			Assert.IsTrue(response.Contains(NoNextQuestionMessage));
		}

		[Test]
		public void There_is_a_question_3min_after_having_answered_it()
		{
			var time = DateTime.Now;
			RegisterQuestion(time);
			AnswerCorrectly(time);
			time += TimeSpan.FromMinutes(3);

			var response = AskForNextQuestion(time);

			Assert.AreEqual(NextQuestionMessage, response);
		}

		[Test]
		public void We_can_answer_again_after_3min_from_first_answer()
		{
			var time = DateTime.Now;
			RegisterQuestion(time);
			AnswerCorrectly(time);
			time += TimeSpan.FromMinutes(3);

			var response = AnswerCorrectly(time);

			Assert.AreEqual(CorrectAnswerMessage, response);
		}

		[Test]
		public void There_is_no_next_question_directly_after_the_second_answer()
		{
			var time = DateTime.Now;
			RegisterQuestion(time);
			AnswerCorrectly(time);
			time += TimeSpan.FromMinutes(3);
			AnswerCorrectly(time);

			var response = AskForNextQuestion(time);

			Assert.IsTrue(response.Contains(NoNextQuestionMessage));
		}

		[Test]
		public void There_is_no_next_question_3_min_after_the_second_answer ()
		{
			var time = DateTime.Now;
			RegisterQuestion(time);
			AnswerCorrectly(time);
			time += TimeSpan.FromMinutes(3); // +3
			AnswerCorrectly(time);
			AskForNextQuestion(time);
			time += TimeSpan.FromMinutes(3); // +6

			var response = AskForNextQuestion(time);

			Assert.IsTrue(response.Contains(NoNextQuestionMessage));
		}

		[Test]
		public void The_question_stays_next_if_we_answer_wrongly()
		{
			var time = DateTime.Now;
			RegisterQuestion(time);
			AnswerWrongly(time);

			var response = AskForNextQuestion(time);

			Assert.AreEqual(NextQuestionMessage, response);
		}

		[Test]
		public void A_wrong_answer_resets_the_lapse()
		{
			var time = DateTime.Now;
			RegisterQuestion(time);
			AnswerCorrectly(time);
			time += TimeSpan.FromMinutes(3); // +3
			AnswerWrongly(time); // reset
			AnswerCorrectly(time);
			time += TimeSpan.FromMinutes(3); // +3 after reset

			var response = AskForNextQuestion(time);

			Assert.AreEqual(NextQuestionMessage, response);
		}

		[Test]
		public void Lapse_work_with_more_than_one_hour()
		{
			var time = DateTime.Now;
			RegisterQuestion(time);
			AnswerCorrectly(time);
			time += TimeSpan.FromHours(2); // +2h
			AnswerCorrectly(time);
			time += TimeSpan.FromHours(3); // +3 after reset

			var response = AskForNextQuestion(time);

			Assert.IsTrue(response.Contains(NoNextQuestionMessage));
		}

		[Test]
		public void The_waiting_time_is_displayed()
		{
			var time = DateTime.Parse("7/24/2016 4:49:13 PM");

			RegisterQuestion(time);
			time += TimeSpan.FromSeconds(3); // +3
			AnswerCorrectly(time);
			time += TimeSpan.FromSeconds(3); // +3
			var response = AskForNextQuestion(time);

			Assert.AreEqual("There is no next question\nCome back at this time: 7/24/2016 4:51:16 PM (in 00:01:57)\n", response);
		}

		static string AnswerWrongly(DateTime time)
		{
			return Commands.Answer(time, "an animal");
		}

		static string AnswerCorrectly(DateTime time)
		{
			return Commands.Answer(time, "a color");
		}

		static string AskForNextQuestion(DateTime time)
		{
			return Commands.AskForNextQuestion(time);
		}

		static string RegisterQuestion(DateTime time)
		{
			return Commands.RegisterQuestion("What is red?", "a color", time);
		}

	}
}

