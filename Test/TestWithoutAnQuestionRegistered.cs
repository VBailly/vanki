
using NUnit.Framework;
using Storage;

namespace Test
{
    [TestFixture]
    public class TestWithoutAnQuestionRegistered
    {
        [SetUp]
        public void SetUp()
        {
            Repository.StoreString(string.Empty);
        }

        [Test]
        public void There_is_no_next_question_if_we_dont_register_one()
        {
            var response = Commands.AskForNextQuestion();

            Assert.AreEqual(ConsoleOutputs.EmptyDeckMessage, response);
        }

        [Test]
        public void We_cannot_anwer_next_question_if_we_dont_register_one()
        {
            var response = Commands.Answer("a fish");

            Assert.AreEqual(ConsoleOutputs.CannotAnswerMessage, response);
        }



    }
}

