using System;
using NUnit.Framework;
using Storage;

namespace Test
{
    [TestFixture]
    public class TestWithClues
    {
        [SetUp]
        public void SetUp()
        {
            Repository.StoreString(string.Empty);
        }

        [Test]
        public void We_can_ask_for_a_clue()
        {
            var time = DateTime.UtcNow;

            Commands.RegisterQuestion(time, "What is red?", "a color");
            var result = Commands.AskForAClue(time);

            Assert.AreEqual("a.c", result);
        }
        [Test]
        public void We_can_ask_for_a_clue_2()
        {
            var time = DateTime.UtcNow;

            Commands.RegisterQuestion(time, "What is a border collie?", "a dog");
            var result = Commands.AskForAClue(time);

            Assert.AreEqual("a.d", result);
        }

        [Test]
        public void We_can_ask_for_two_clues()
        {
            var time = DateTime.UtcNow;

            Commands.RegisterQuestion(time, "What is red?", "a color");
            Commands.AskForAClue(time);
            var result = Commands.AskForAClue(time);

            Assert.AreEqual("a.co", result);
        }

        [Test]
        public void Commas_are_kept_in_the_clue()
        {
            var time = DateTime.UtcNow;

            Commands.RegisterQuestion(time, "Border collie is ____ ___?", "a kind, of dog");
            var result = Commands.AskForAClue(time);

            Assert.AreEqual("a.k, o.d", result);
        }

        [Test]
        public void Asking_for_a_clue_when_no_question_is_pending_returns_an_empty_string()
        {
            var time = DateTime.UtcNow;

            var result = Commands.AskForAClue(time);

            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void Answering_decreases_the_clue_by_one()
        {
            var time = DateTime.UtcNow;

            Commands.RegisterQuestion(time, "What is red?", "a color");
            Commands.AskForAClue(time);
            Commands.Answer(time, "a color");

            time = IncreaseTime(time, 0, 5);
            var result = Commands.AskForNextQuestion(time);

            Assert.AreEqual("What is red?", result);
        }

        [Test]
        public void The_next_question_is_shown_with_a_clue_if_any()
        {
            var time = DateTime.UtcNow;

            Commands.RegisterQuestion(time, "What is red?", "a color");
            Commands.AskForAClue(time);

            var result = Commands.AskForNextQuestion(time);

            Assert.AreEqual("What is red?\nclue: a.c", result);
        }

        [Test]
        public void Asking_for_a_clue_resets_the_score()
        {
            var time = DateTime.UtcNow;

            Commands.RegisterQuestion(time, "What is red?", "a color");
            Commands.Answer(time, "a color");

            time = IncreaseTime(time, 0, 5);

            Commands.Answer(time, "a color");

            time = IncreaseTime(time, 5, 0);

            Commands.Answer(time, "a color");

            time = IncreaseTime(time, 15, 0);

            Commands.AskForAClue(time);
            Commands.Answer(time, "a color");

            time = IncreaseTime(time, 0, 5);

            var result = Commands.AskForNextQuestion(time);

            Assert.AreEqual("What is red?", result);
        }

        [Test]
        public void Asking_for_a_clue_does_not_update_the_date()
        {
            var time = DateTime.UtcNow;

            Commands.RegisterQuestion(time, "What is red?", "a color");
            time = IncreaseTime(time, 0,5);

            Commands.RegisterQuestion(time, "What is blue?", "a color");

            time = IncreaseTime(time, 0, 5);

            Commands.AskForAClue(time);

            var result = Commands.AskForNextQuestion(time);

            Assert.AreEqual("What is red?\nclue: a.c", result);
        }

        [Test]
        public void Wrong_answers_do_not_change_the_clue_level()
        {
            var time = DateTime.UtcNow;

            Commands.RegisterQuestion(time, "What is red?", "a color");
            time = IncreaseTime(time, 0, 5);
            Commands.AskForAClue(time);
            Commands.AskForAClue(time);

            // 2 clues

            Commands.Answer(time, "a color");

            // 1 clue

            time = IncreaseTime(time, 1, 0);

            Commands.Answer(time, "an idiot");

            time = IncreaseTime(time, 0, 1);

            var result = Commands.AskForNextQuestion(time);
            Assert.AreEqual("What is red?\nclue: a.c", result);
        }

        static DateTime IncreaseTime(DateTime time, int hours, int minutes)
        {
            return time + TimeSpan.FromMinutes(minutes) + TimeSpan.FromHours(hours);
        }
    }
}

