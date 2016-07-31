using System;
using NUnit.Framework;
using Storage;
using Vanki;

namespace Test
{
    [TestFixture]
    public class TestWithClues
    {
        [SetUp]
        public void SetUp()
        {
            Repository.StoreString(string.Empty);
            Clock.Getter = null;
        }

        [Test]
        public void We_can_ask_for_a_clue()
        {
            Commands.RegisterQuestion("What is red?", "a color");
            var result = Commands.AskForAClue();

            Assert.AreEqual("a.c", result);
        }
        [Test]
        public void We_can_ask_for_a_clue_2()
        {
            Commands.RegisterQuestion("What is a border collie?", "a dog");
            var result = Commands.AskForAClue();

            Assert.AreEqual("a.d", result);
        }

        [Test]
        public void We_can_ask_for_two_clues()
        {
            Commands.RegisterQuestion("What is red?", "a color");
            Commands.AskForAClue();
            var result = Commands.AskForAClue();

            Assert.AreEqual("a.co", result);
        }

        [Test]
        public void Commas_are_kept_in_the_clue()
        {
            Commands.RegisterQuestion("Border collie is ____ ___?", "a kind, of dog");
            var result = Commands.AskForAClue();

            Assert.AreEqual("a.k, o.d", result);
        }

        [Test]
        public void Asking_for_a_clue_when_no_question_is_pending_returns_an_empty_string()
        {
            var result = Commands.AskForAClue();

            Assert.AreEqual(string.Empty, result);
        }
    }
}

