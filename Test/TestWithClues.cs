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

        [Test]
        public void Answering_decreases_the_clue_by_one()
        {
            Commands.RegisterQuestion("What is red?", "a color");
            Commands.AskForAClue();
            Commands.Answer("a color");

            Clock.Getter = () => DateTime.Now + TimeSpan.FromMinutes(5);
            var result = Commands.AskForNextQuestion();

            Assert.AreEqual("What is red?", result);
        }

        [Test]
        public void The_next_question_is_shown_with_a_clue_if_any()
        {
            Commands.RegisterQuestion("What is red?", "a color");
            Commands.AskForAClue();

            var result = Commands.AskForNextQuestion();

            Assert.AreEqual("What is red?\nclue: a.c", result);
        }

        [Test]
        public void Asking_for_a_clue_resets_the_score()
        {
            Commands.RegisterQuestion("What is red?", "a color");
            Commands.Answer("a color");

            var time = Clock.CurrentTime;
            Clock.Getter = () => time + TimeSpan.FromMinutes(5);

            Commands.Answer("a color");

            time = Clock.CurrentTime;
            Clock.Getter = () => time + TimeSpan.FromHours(5);

            Commands.Answer("a color");

            time = Clock.CurrentTime;
            Clock.Getter = () => time + TimeSpan.FromHours(15);

            Commands.AskForAClue();
            Commands.Answer("a color");

            time = Clock.CurrentTime;
            Clock.Getter = () => time + TimeSpan.FromMinutes(5);

            var result = Commands.AskForNextQuestion();

            Assert.AreEqual("What is red?", result);
        }
    }
}

