using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Orchestration;

namespace Test
{
    class MyTestRandom : RandomAPI.Random
    {
        int index_;

        public MyTestRandom(int i)
        {
            index_ = i;
        }

        public override T PickRandomly<T>(IEnumerable<T> elements)
        {
            return elements.ToList()[index_];
        }
    }

    [TestFixture]
    public class TestMultipleQuestions
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
        public void We_can_register_multiple_version_of_a_question()
        {
            var time = DateTime.UtcNow;
            Commands.RegisterQuestion(time, new[] { "What is red?", "What is blue?" }, new[] { "a color" });

            var response = Commands.AskForNextQuestion(time);

            Assert.Contains(response, new[] { "What is red?", "What is blue?" }); 
        }

        [Test]
        public void The_question_displayed_is_randomly_chosen_between_the_one_possible()
        {
            var time = DateTime.UtcNow;
            Commands.RegisterQuestion(time, new[] { "What is red?", "What is blue?" }, new[] { "a color" });

            RandomAPI.Random.Instance = new MyTestRandom(0);

            var response = Commands.AskForNextQuestion(time);

            Assert.AreEqual(response, "What is red?");

            RandomAPI.Random.Instance = new MyTestRandom(1);

            response = Commands.AskForNextQuestion(time);

            Assert.AreEqual(response, "What is blue?");
        }
    }
}
