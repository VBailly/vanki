using NUnit.Framework;
using System;
using Vanki;
using Storage;

namespace Test
{
	[TestFixture]
	public class TestsWithASingleQuestion
	{
		const string NoNextQuestionMessage = ConsoleOutputs.NoNextQuestionMessage;
		static readonly string NextQuestionMessage = "What is red?";
		static readonly string WrongAnswerMessage = "a color";
		const string CannotAnswerMessage = ConsoleOutputs.CannotAnswerMessage;

		[SetUp]
		public void SetUp()
		{
            Repository.StoreString(string.Empty);
			Clock.LocalTimeGetter = null;
		}

		[TearDown]
		public void TearDown()
		{
		}

		[Test]
		public void We_can_ask_twice_for_a_question()
		{
			RegisterQuestion();
			AskForNextQuestion();

			var response = AskForNextQuestion();

			Assert.AreEqual(NextQuestionMessage, response);

		}

		[Test]
		public void We_cannot_answer_when_there_is_no_question()
		{
			RegisterQuestion();
			AnswerCorrectly();
			AskForNextQuestion();

			var response = AnswerCorrectly();

			Assert.AreEqual(CannotAnswerMessage, response);

		}

		[Test]
		public void Wrong_answers_dont_pass()
		{
			RegisterQuestion();

			var response = AnswerWrongly();

			Assert.AreEqual(WrongAnswerMessage, response);
		}

		[Test]
		public void A_wrong_answer_is_not_treated_if_no_question_is_pending()
		{
			RegisterQuestion();
			AnswerCorrectly();

			var response = AnswerWrongly();

			Assert.AreEqual(CannotAnswerMessage, response);
		}

		[Test]
		public void Register_a_new_entry()
		{
			var response = RegisterQuestion();

            Assert.AreEqual(string.Empty, response);
		}

		[Test]
		public void A_question_is_available_straight_after_being_registered()
		{
			RegisterQuestion();

			var response = AskForNextQuestion();

			Assert.AreEqual(NextQuestionMessage, response);
		}

		[Test]
		public void Giving_a_correct_answer_for_the_first_time()
		{
			RegisterQuestion();

			var response = AnswerCorrectly();

            Assert.AreEqual(string.Empty, response);
		}

		[Test]
		public void There_is_no_question_just_after_having_answered_it()
		{
			RegisterQuestion();
			AnswerCorrectly();

			var response = AskForNextQuestion();

			Assert.IsTrue(response.Contains(NoNextQuestionMessage));
		}

		[Test]
		public void There_is_still_no_question_1min_after_having_answered_it()
		{

			RegisterQuestion();
			AnswerCorrectly();
            Clock.LocalTimeGetter = () => DateTime.Now + TimeSpan.FromMinutes(1);

			var response = AskForNextQuestion();

			Assert.IsTrue(response.Contains(NoNextQuestionMessage));
		}

		[Test]
		public void There_is_a_question_3min_after_having_answered_it()
		{
			var time = DateTime.Now;
			RegisterQuestion();
			AnswerCorrectly();
			Clock.LocalTimeGetter = () => DateTime.Now + TimeSpan.FromMinutes(3);

			var response = AskForNextQuestion();

			Assert.AreEqual(NextQuestionMessage, response);
		}

		[Test]
		public void We_can_answer_again_after_3min_from_first_answer()
		{
			RegisterQuestion();
			AnswerCorrectly();
			Clock.LocalTimeGetter = () => DateTime.Now + TimeSpan.FromMinutes(3);

			var response = AnswerCorrectly();

            Assert.AreEqual(string.Empty, response);
		}

		[Test]
		public void There_is_no_next_question_directly_after_the_second_answer()
		{
			RegisterQuestion();
			AnswerCorrectly();
			Clock.LocalTimeGetter = () => DateTime.Now + TimeSpan.FromMinutes(3);
			AnswerCorrectly();

			var response = AskForNextQuestion();

			Assert.IsTrue(response.Contains(NoNextQuestionMessage));
		}

		[Test]
		public void There_is_no_next_question_3_min_after_the_second_answer ()
		{
			RegisterQuestion();
			AnswerCorrectly();
			Clock.LocalTimeGetter = () => DateTime.Now + TimeSpan.FromMinutes(3);
			AnswerCorrectly();
			AskForNextQuestion();
			Clock.LocalTimeGetter = () => DateTime.Now + TimeSpan.FromMinutes(6);

			var response = AskForNextQuestion();

			Assert.IsTrue(response.Contains(NoNextQuestionMessage));
		}

		[Test]
		public void The_question_stays_next_if_we_answer_wrongly()
		{
			RegisterQuestion();
			AnswerWrongly();

			var response = AskForNextQuestion();

			Assert.AreEqual(NextQuestionMessage, response);
		}

		[Test]
		public void A_wrong_answer_resets_the_lapse()
		{
			var time = DateTime.Now;
			RegisterQuestion();
			AnswerCorrectly();
			Clock.LocalTimeGetter = () => DateTime.Now + TimeSpan.FromMinutes(3);
			AnswerWrongly(); // reset
			AnswerCorrectly();
			Clock.LocalTimeGetter = () => DateTime.Now + TimeSpan.FromMinutes(6);

			var response = AskForNextQuestion();

			Assert.AreEqual(NextQuestionMessage, response);
		}

		[Test]
		public void Lapse_work_with_more_than_one_hour()
		{
			RegisterQuestion();
			AnswerCorrectly();
			Clock.LocalTimeGetter = () => DateTime.Now + TimeSpan.FromHours(2);
			AnswerCorrectly();
			Clock.LocalTimeGetter = () => DateTime.Now + TimeSpan.FromHours(5);

			var response = AskForNextQuestion();

			Assert.IsTrue(response.Contains(NoNextQuestionMessage));
		}

		[Test]
		public void The_waiting_time_is_displayed()
		{
			var time = DateTime.Parse("7/24/2016 4:49:13 PM");
            Clock.LocalTimeGetter = () => time;
			RegisterQuestion();
			Clock.LocalTimeGetter = () => time + TimeSpan.FromSeconds(3); 
			AnswerCorrectly();
			Clock.LocalTimeGetter = () => time + TimeSpan.FromSeconds(6); 
			var response = AskForNextQuestion();

			Assert.AreEqual("There is no next question\nCome back at this time: 7/24/2016 4:51:16 PM (in 00:01:57)\n", response);
		}


        [Test]
        public void The_time_between_registration_and_first_answer_is_ignored()
        {
            RegisterQuestion();
            var time = Clock.CurrentLocalTime;
            Clock.LocalTimeGetter = () => time + TimeSpan.FromHours(6);
            AnswerCorrectly();
            time = Clock.CurrentLocalTime;
            Clock.LocalTimeGetter = () => time + TimeSpan.FromMinutes(6);
            var response = AskForNextQuestion();

            Assert.AreEqual(NextQuestionMessage, response);
        }

		static string AnswerWrongly()
		{
			return Commands.Answer("an animal");
		}

		static string AnswerCorrectly()
		{
			return Commands.Answer("a color");
		}

		static string AskForNextQuestion()
		{
			return Commands.AskForNextQuestion();
		}

		static string RegisterQuestion()
		{
			return Commands.RegisterQuestion("What is red?", "a color");
		}

	}
}

