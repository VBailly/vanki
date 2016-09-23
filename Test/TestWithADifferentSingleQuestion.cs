using System;
using NUnit.Framework;
using Storage;

namespace Test
{

    [TestFixture]
    public class TestWithADifferentSingleQuestion
    {
        static readonly string Question = "What color is the sky?";
        static readonly string Answer = "blue";
        static readonly string WrongAnswer = "a fish";
        static readonly string NextQuestionMessage = Question;
        static readonly string WrongAnswerMessage = Answer;

        [SetUp]
        public void SetUp()
        {
            Repository.StoreString(string.Empty);
        }

        [Test]
        public void Register_a_new_entry()
        {
            var time = DateTime.UtcNow;

            var response = RegisterQuestion(time);

            Assert.AreEqual(string.Empty, response);
        }

        [Test]
        public void A_question_is_available_straight_after_being_registered()
        {
            var time = DateTime.UtcNow;

            RegisterQuestion(time);

            var response = Commands.AskForNextQuestion(time);

            Assert.AreEqual(NextQuestionMessage, response);
        }

        static string RegisterQuestion(DateTime time)
        {
            return Commands.RegisterQuestion(time, Question, Answer);
        }

        [Test]
        public void Giving_a_correct_answer_for_the_first_time()
        {
            var time = DateTime.UtcNow;

            RegisterQuestion(time);

            var response = Commands.Answer(time, Answer);

            Assert.AreEqual(string.Empty, response);
        }

        [Test]
        public void Wrong_answers_dont_pass()
        {
            var time = DateTime.UtcNow;

            RegisterQuestion(time);

            var response = Commands.Answer(time, WrongAnswer);

            Assert.AreEqual(WrongAnswerMessage, response);
        }
    }
}

