using System;
using NUnit.Framework;
using Orchestration;

namespace Test
{
    [TestFixture]
    public class TestWithoutAnQuestionRegistered
    {
        [SetUp]
        public void SetUp()
        {
            ServiceOrchestration.InstallServicesForTests();
        }

        [TearDown]
        public void TearDown()
        {
            ServiceOrchestration.UninstallServices();
        }


        [Test]
        public void There_is_no_next_question_if_we_dont_register_one()
        {
            var time = DateTime.UtcNow;

            var response = Commands.AskForNextQuestion(time);

            Assert.AreEqual(ConsoleOutputs.EmptyDeckMessage, response);
        }
    }
}

