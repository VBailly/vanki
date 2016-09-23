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
            var response = RegisterQuestion();

            Assert.AreEqual(string.Empty, response);
        }

        [Test]
        public void A_question_is_available_straight_after_being_registered()
        {
            RegisterQuestion();

            var response = Commands.AskForNextQuestion();

            Assert.AreEqual(NextQuestionMessage, response);
        }

        static string RegisterQuestion()
        {
            return Commands.RegisterQuestion(Question, Answer);
        }

        [Test]
        public void Giving_a_correct_answer_for_the_first_time()
        {
            RegisterQuestion();

            var response = Commands.Answer(Answer);

            Assert.AreEqual(string.Empty, response);
        }

        [Test]
        public void Wrong_answers_dont_pass()
        {
            var time = DateTime.Now;
            RegisterQuestion();

            var response = Commands.Answer(WrongAnswer);

            Assert.AreEqual(WrongAnswerMessage, response);
        }
    }
}

