using System;
using NUnit.Framework;
using Storage;

namespace Test
{
    [TestFixture]
    public class TestAlternativeAnswers
    {
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
        public void We_can_register_two_different_answers_for_a_question_and_answer_by_the_first_one()
        {
            Commands.RegisterQuestion("What is red?", new[] { "a color", "a colour" });
            var result = Commands.Answer("a color");

            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void We_can_register_two_different_answers_for_a_question_and_answer_by_the_second_one()
        {
            Commands.RegisterQuestion("What is red?", new[] { "a color", "a colour" });
            var result = Commands.Answer("a colour");

            Assert.AreEqual(string.Empty, result);
        }
    }
}

