using System;
using NUnit.Framework;
using Storage;

namespace Test
{
    [TestFixture]
    public class TestVersionsUpgrades
    {
        [SetUp]
        public void SetUp()
        {
            Repository.StoreString(string.Empty);
            Clock.LocalTimeGetter = null;
            Clock.HoursDifferenceFromGlobal = null;
        }

        [TearDown]
        public void TearDown()
        {
            Clock.LocalTimeGetter = null;
            Clock.HoursDifferenceFromGlobal = null;
        }

        [Test]
        public void Version1_cards_time_is_local()
        {
            var time = DateTime.Parse("8/2/2016 2:25:13 PM");
            time += TimeSpan.FromMinutes(1);

            Clock.LocalTimeGetter = () => time;
            Clock.HoursDifferenceFromGlobal = () => -5;

            string content = @"<Deck version=""1\"">
                <Card version=""1"">
                <time>8/2/2016 2:25:13 PM</time>
                <lapse>2</lapse>
                <question>What is red?</question>
                <answer>a color</answer>
                </Card>
                </Deck>";
            Repository.StoreString(content);

            var response = Commands.AskForNextQuestion();

            Assert.IsTrue(response.Contains(ConsoleOutputs.NoNextQuestionMessage));
        }

        [Test]
        public void Cards_without_a_clue_have_a_clue_of_zero()
        {
            var time = DateTime.Parse("8/2/2016 2:25:13 PM");
            time += TimeSpan.FromMinutes(3);

            Clock.LocalTimeGetter = () => time;
            Clock.HoursDifferenceFromGlobal = () => 0;

            string content = @"<Deck version=""1\"">
                <Card version=""2"">
                <time>8/2/2016 2:25:13 PM</time>
                <lapse>2</lapse>
                <question>What is red?</question>
                <answer>a color</answer>
                </Card>
                </Deck>";
            Repository.StoreString(content);

            var response = Commands.AskForNextQuestion();

            Assert.AreEqual("What is red?", response);
        }

        [Test]
        public void Cards_with_the_no_alternative_answer_still_work()
        {
            var time = DateTime.Parse("8/2/2016 2:25:13 PM");
            time += TimeSpan.FromMinutes(3);

            Clock.LocalTimeGetter = () => time;
            Clock.HoursDifferenceFromGlobal = () => 0;

            string content = @"<Deck version=""1\"">
                <Card version=""3"">
                <time>8/2/2016 2:25:13 PM</time>
                <lapse>2</lapse>
                <question>What is red?</question>
                <answer>a color</answer>
                <clue>0</clue>
                </Card>
                </Deck>";
            Repository.StoreString(content);
            Commands.AskForNextQuestion();
            var response = Commands.Answer("a color");

            Assert.IsEmpty(response);
        }

        [Test]
        public void Upgrades_to_version_more_than_4_do_update_the_answer_field()
        {
            var time = DateTime.Parse("8/2/2016 2:25:13 PM");
            time += TimeSpan.FromMinutes(3);

            Clock.LocalTimeGetter = () => time;
            Clock.HoursDifferenceFromGlobal = () => 0;

            string content = @"<Deck version=""1\"">
                <Card version=""3"">
                <time>8/2/2016 2:25:13 PM</time>
                <lapse>2</lapse>
                <question>What is red?</question>
                <answer>a color</answer>
                <clue>0</clue>
                </Card>
                </Deck>";
            Repository.StoreString(content);
            Commands.AskForNextQuestion();
            Commands.Answer("a color");

            time += TimeSpan.FromMinutes(13);

            Clock.LocalTimeGetter = () => time;
            var response = Commands.Answer("a color");

            Assert.IsEmpty(response);
        }
    }
}

