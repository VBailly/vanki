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

        [SetUp]
        public void SetUp()
        {
            Repository.StoreString(string.Empty);
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void We_can_ask_twice_for_a_question()
        {
            DateTime time = DateTime.UtcNow;

            RegisterQuestion(time);
            AskForNextQuestion(time);

            var response = AskForNextQuestion(time);

            Assert.AreEqual(NextQuestionMessage, response);

        }

        [Test]
        public void Wrong_answers_dont_pass()
        {
            DateTime time = DateTime.UtcNow;

            RegisterQuestion(time);

            AnswerWrongly(time);

            var response = AskForNextQuestion(time);

            Assert.That(response.Contains(NextQuestionMessage));
        }

        [Test]
        public void Wrong_answers_dont_print_anything()
        {
            DateTime time = DateTime.UtcNow;

            RegisterQuestion(time);

            var response = AnswerWrongly(time);


            Assert.AreEqual(string.Empty, response);
        }

        [Test]
        public void A_wrong_answer_is_not_treated_if_no_question_is_pending()
        {
            DateTime time = DateTime.UtcNow;

            RegisterQuestion(time);
            AnswerCorrectly(time);

            AnswerWrongly(time);

            time += TimeSpan.FromHours(1);

            var response = AskForNextQuestion(time);

            Assert.AreEqual(NextQuestionMessage, response);
        }

        [Test]
        public void Register_a_new_entry()
        {
            DateTime time = DateTime.UtcNow;

            var response = RegisterQuestion(time);

            Assert.AreEqual(string.Empty, response);
        }

        [Test]
        public void A_question_is_available_straight_after_being_registered()
        {
            
            DateTime time = DateTime.UtcNow;

            RegisterQuestion(time);

            var response = AskForNextQuestion(time);
            Assert.AreEqual(NextQuestionMessage, response);
        }

        [Test]
        public void Giving_a_correct_answer_for_the_first_time()
        {
            DateTime time = DateTime.UtcNow;
            RegisterQuestion(time);
            AnswerCorrectly(time);

            AssertNoQuestionPending(time);
        }

        [Test]
        public void Answer_is_case_insensitive()
        {
            DateTime time = DateTime.UtcNow;

            RegisterQuestion(time);

            Commands.Answer(time, "A cOloR");

            AssertNoQuestionPending(time);
        }

       
        [Test]
        public void Answer_can_be_case_sensitive_and_fail()
        {
            DateTime time = DateTime.UtcNow;

            RegisterCaseSensitiveQuestion(time);

            Commands.Answer(time, "A cOloR");

            var response = AskForNextQuestion(time);

            Assert.That(response.Contains(NextQuestionMessage));
        }

        [Test]
        public void Answer_can_be_case_sensitive_and_succeed()
        {
            DateTime time = DateTime.UtcNow;

            RegisterCaseSensitiveQuestion(time);

            Commands.Answer(time, "A color");

            AssertNoQuestionPending(time);
        }

        [Test]
        public void There_is_still_no_question_1min_after_having_answered_it()
        {
            DateTime time = DateTime.UtcNow;

            RegisterQuestion(time);
            AnswerCorrectly(time);
            time += TimeSpan.FromMinutes(1);

            AssertNoQuestionPending(time);
        }

        [Test]
        public void There_is_a_question_3min_after_having_answered_it()
        {
            var time = DateTime.UtcNow;
            RegisterQuestion(time);
            AnswerCorrectly(time);
            time += TimeSpan.FromMinutes(3);

            var response = AskForNextQuestion(time);

            Assert.AreEqual(NextQuestionMessage, response);
        }

       [Test]
        public void We_can_answer_again_after_3min_from_first_answer()
        {
            DateTime time = DateTime.UtcNow;

            RegisterQuestion(time);
            AnswerCorrectly(time);
            time += TimeSpan.FromMinutes(3);

            AnswerCorrectly(time);
        }

        [Test]
        public void There_is_no_next_question_directly_after_the_second_answer()
        {
            DateTime time = DateTime.UtcNow;

            RegisterQuestion(time);
            AnswerCorrectly(time);
            time += TimeSpan.FromMinutes(3);
            AnswerCorrectly(time);

            AssertNoQuestionPending(time);
        }

       [Test]
        public void There_is_no_next_question_3_min_after_the_second_answer ()
        {
            DateTime time = DateTime.UtcNow;

            RegisterQuestion(time);
            AnswerCorrectly(time);
            time += TimeSpan.FromMinutes(3);
            AnswerCorrectly(time);
            AskForNextQuestion(time);
            time += TimeSpan.FromMinutes(3);

            AssertNoQuestionPending(time);
        }

       [Test]
        public void The_question_stays_next_if_we_answer_wrongly()
        {
            DateTime time = DateTime.UtcNow;

            RegisterQuestion(time);
            AnswerWrongly(time);

            var response = AskForNextQuestion(time);

            Assert.That(response.Contains(NextQuestionMessage));
        }

        [Test]
        public void A_wrong_answer_resets_the_lapse()
        {
            var time = DateTime.UtcNow;
            RegisterQuestion(time);
            AnswerCorrectly(time);
            time += TimeSpan.FromMinutes(3);
            AnswerWrongly(time); // reset
            AnswerCorrectly(time);
            time += TimeSpan.FromMinutes(3);

            var response = AskForNextQuestion(time);

            Assert.AreEqual(NextQuestionMessage, response);
        }

        [Test]
        public void Lapse_work_with_more_than_one_hour()
        {
            DateTime time = DateTime.UtcNow;

            RegisterQuestion(time);
            AnswerCorrectly(time);
            time += TimeSpan.FromHours(2);
            AnswerCorrectly(time);
            time += TimeSpan.FromHours(3);

            AssertNoQuestionPending(time);
        }

        [Test]
        public void The_waiting_time_is_displayed()
        {
            var time = DateTime.Parse("2016-07-24T16:49:13Z").ToUniversalTime();
            RegisterQuestion(time);
            time += TimeSpan.FromSeconds(3);
            AnswerCorrectly(time);
            time += TimeSpan.FromSeconds(3);
            var response = AskForNextQuestion(time);

            var timeUntilNextQuestion = DateTime.Parse("2016-07-24T16:51:16Z").ToUniversalTime();
            Assert.AreEqual($"There is no next question\nCome back at this time: {timeUntilNextQuestion.ToLocalTime()} (in 00:01:57)\n", response);
        }

        [Test]
        public void The_time_between_registration_and_first_answer_is_ignored()
        {
            DateTime time = DateTime.UtcNow;
            RegisterQuestion(time);

            time += TimeSpan.FromHours(6);
            AnswerCorrectly(time);

            time += TimeSpan.FromMinutes(6);
            var response = AskForNextQuestion(time);

            Assert.AreEqual(NextQuestionMessage, response);
        }

        static void AssertNoQuestionPending(DateTime time)
        {
            var response = AskForNextQuestion(time);
            Assert.That(response.Contains(ConsoleOutputs.NoNextQuestionMessage));
        }

        [Test]
        public void Nothing_to_revert()
        {
            DateTime time = DateTime.UtcNow;

            var response = Revert(time);

            Assert.AreEqual(response, ConsoleOutputs.NothingToRevert);
        }

        [Test]
        public void Revert_last()
        {
            DateTime time = DateTime.UtcNow;

            RegisterQuestion(time);

            AnswerWrongly(time);

            var response = Revert(time);

            Assert.AreEqual(response, ConsoleOutputs.RevertLast);

            response = AskForNextQuestion(time);

            Assert.AreEqual(response, NextQuestionMessage);
        }

        [Test]
        public void Revert_add_last()
        {
            DateTime time = DateTime.UtcNow;

            RegisterQuestion(time);

            AnswerWrongly(time);

            var response = Revert(time, true);

            Assert.AreEqual(response, ConsoleOutputs.RevertAddLast);

            AnswerWrongly(time);

            response = AskForNextQuestion(time);

            Assert.AreNotEqual(response, NextQuestionMessage);
        }

        static string AnswerWrongly(DateTime time)
        {
            return Commands.Answer(time, "an animal");
        }

        static string AnswerCorrectly(DateTime time)
        {
            return Commands.Answer(time, "a color");
        }

        static string Revert(DateTime time, bool add = false)
        {
            if (add)
                return Commands.RevertAddLastWrongAnswer(time);
            return Commands.RevertLastWrongAnswer(time);
        }

        static string AskForNextQuestion(DateTime time)
        {
            return Commands.AskForNextQuestion(time);
        }

        static string RegisterQuestion(DateTime time)
        {
            return Commands.RegisterQuestion(time, "What is red?", "a color");
        }

        static string RegisterCaseSensitiveQuestion(DateTime time)
        {
            return Commands.RegisterQuestionCaseSensitive(time, "What is red?", "A color");
        }

    }
}

