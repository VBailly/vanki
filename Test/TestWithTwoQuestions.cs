using NUnit.Framework;
using System;
using Vanki;
using Storage;

namespace Test
{
    [TestFixture]
    public class TestWithTwoQuestions
    {
        static readonly string Question1 = "What is the color of a red fish?";
        static readonly string Question2 = "What is the color of a blue fish?";
        static readonly string NextQuestion1Message = Question1;
        static readonly string NextQuestion2Message = Question2;


        [SetUp]
        public void SetUp()
        {
            Repository.StoreString(string.Empty);
        }

        [Test]
        public void Oldest_question_of_newly_created_questions_is_the_next_one()
        {
            var time = DateTime.UtcNow;
            Commands.RegisterQuestion(time, Question1, "red");
            time += TimeSpan.FromSeconds(2);
            Commands.RegisterQuestion(time, Question2, "blue");
            time += TimeSpan.FromSeconds(4);
            var response = Commands.AskForNextQuestion(time);

            Assert.AreEqual(NextQuestion1Message, response);
        }

        [Test]
        public void We_can_answer_the_first_question()
        {
            var time = DateTime.UtcNow;
            Commands.RegisterQuestion(time, Question1, "red");
            time += TimeSpan.FromSeconds(2);
            Commands.RegisterQuestion(time, Question2, "blue");
            time += TimeSpan.FromSeconds(4);
            var response = Commands.Answer(time, "red");

            Assert.AreEqual(string.Empty, response);
        }

        [Test]
        public void The_second_question_is_presented_after_we_answer_the_first_one()
        {
            var time = DateTime.UtcNow;
            Commands.RegisterQuestion(time, Question1, "red");
            time += TimeSpan.FromSeconds(2);
            Commands.RegisterQuestion(time, Question2, "blue");
            time += TimeSpan.FromSeconds(4);
            Commands.Answer(time, "red");
            time += TimeSpan.FromSeconds(6);

            var response = Commands.AskForNextQuestion(time);

            Assert.AreEqual(NextQuestion2Message, response);
        }

        [Test]
        public void The_second_question_can_be_properly_answered()
        {
            var time = DateTime.UtcNow;
            Commands.RegisterQuestion(time, Question1, "red");
            time += TimeSpan.FromSeconds(2);
            Commands.RegisterQuestion(time, Question2, "blue");
            time += TimeSpan.FromSeconds(4);
            Commands.Answer(time, "red");
            time += TimeSpan.FromSeconds(6);

            var response = Commands.Answer(time, "blue");
            Assert.AreEqual(string.Empty, response);
        }

        [Test]
        public void No_questions_available_after_the_two_answers()
        {
            var time = DateTime.UtcNow;
            Commands.RegisterQuestion(time, Question1, "red");
            time += TimeSpan.FromSeconds(2);
            Commands.RegisterQuestion(time, Question2, "blue");
            time += TimeSpan.FromSeconds(4);
            Commands.Answer(time, "red");
            time += TimeSpan.FromSeconds(6);
            Commands.Answer(time, "blue");

            var response = Commands.AskForNextQuestion(time);
            Assert.IsTrue(response.Contains(ConsoleOutputs.NoNextQuestionMessage));
        }

        [Test]
        public void Question_available_again_after_the_two_answers_and_3_min()
        {
            var time = DateTime.UtcNow;
            Commands.RegisterQuestion(time, Question1, "red");
            time += TimeSpan.FromSeconds(2);
            Commands.RegisterQuestion(time, Question2, "blue");
            time += TimeSpan.FromSeconds(4);
            Commands.Answer(time, "red");
            time += TimeSpan.FromSeconds(6);
            Commands.Answer(time, "blue");
            time += TimeSpan.FromMinutes(9);

            var response = Commands.AskForNextQuestion(time);

            Assert.AreEqual(NextQuestion1Message, response);
        }

        [Test]
        public void We_do_not_pass_to_the_next_question_after_a_wrong_answer()
        {
            var time = DateTime.UtcNow;
            Commands.RegisterQuestion(time, Question1, "red");
            time += TimeSpan.FromSeconds(2);
            Commands.RegisterQuestion(time, Question2, "blue");
            time += TimeSpan.FromSeconds(4);
            Commands.Answer(time, "wrong");
            time += TimeSpan.FromSeconds(6);

            var response = Commands.AskForNextQuestion(time);

            Assert.That(response.Contains(NextQuestion1Message));
        }
    }
}

