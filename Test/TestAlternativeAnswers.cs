using System;
using NUnit.Framework;
using Orchestration;
using Storage;

namespace Test
{
    [TestFixture]
    public class TestAlternativeAnswers
    {
        [SetUp]
        public void SetUp()
        {
            ServiceOrchestration.InstallServices();
            Repository.StoreString(string.Empty);
        }

        [TearDown]
        public void TearDown()
        {
            ServiceOrchestration.UninstallServices();
        }

        [Test]
        public void We_can_register_two_different_answers_for_a_question_and_answer_by_the_first_one()
        {
            Commands.RegisterQuestion(DateTime.UtcNow, "What is red?", new[] { "a color", "a colour" });
            var result = Commands.Answer(DateTime.UtcNow, "a color");

            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void We_can_register_two_different_answers_for_a_question_and_answer_by_the_second_one()
        {
            Commands.RegisterQuestion(DateTime.UtcNow, "What is red?", new[] { "a color", "a colour" });
            var result = Commands.Answer(DateTime.UtcNow, "a colour");

            Assert.AreEqual(string.Empty, result);
        }
    }
}

