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
        public void Failing_increase_the_clue_once()
        {
            var time = DateTime.UtcNow;

            Commands.RegisterQuestion(time, "What is red?", "a color");
            Commands.Answer(time, "wrong");

            var result = Commands.AskForNextQuestion(time);
            Assert.That(result.Contains("a.c"));
        }

        [Test]
        public void Failing_twice_increase_the_clue_twice()
        {
            var time = DateTime.UtcNow;

            Commands.RegisterQuestion(time, "What is red?", "a color");
            Commands.Answer(time, "wrong");
            Commands.Answer(time, "wrong");

            var result = Commands.AskForNextQuestion(time);
            Assert.That(result.Contains("a.co"));
        }

        [Test]
        public void Commas_are_kept_in_the_clue()
        {
            var time = DateTime.UtcNow;

            Commands.RegisterQuestion(time, "Border collie is ____ ___?", "a kind, of dog");
            Commands.Answer(time, "wrong");

            var result = Commands.AskForNextQuestion(time);
            Assert.That(result.Contains("a.k, o.d"));
        }

        [Test]
        public void Answering_correctly_decreases_the_clue_by_one()
        {
            var time = DateTime.UtcNow;

            Commands.RegisterQuestion(time, "What is red?", "a color");
            Commands.Answer(time, "wrong");
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
            Commands.Answer(time, "wrong");

            var result = Commands.AskForNextQuestion(time);

            Assert.AreEqual("What is red?\nclue: a.c", result);
        }

        [Test]
        public void Failing_a_question_does_not_update_the_date()
        {
            var time = DateTime.UtcNow;

            Commands.RegisterQuestion(time, "What is red?", "a color");
            time = IncreaseTime(time, 0,5);

            Commands.RegisterQuestion(time, "What is blue?", "a color");

            time = IncreaseTime(time, 0, 5);

            Commands.Answer(time, "wrong");

            var result = Commands.AskForNextQuestion(time);

            Assert.AreEqual("What is red?\nclue: a.c", result);
        }

        static DateTime IncreaseTime(DateTime time, int hours, int minutes)
        {
            return time + TimeSpan.FromMinutes(minutes) + TimeSpan.FromHours(hours);
        }
    }
}

