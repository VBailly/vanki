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
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void Cards_with_the_no_alternative_answer_still_work()
        {
            var time = DateTime.Parse("8/2/2016 2:25:13 PM").ToUniversalTime();
            time += TimeSpan.FromMinutes(3);

            string content = @"<Deck version=""1"">
                <Card version=""3"">
                <time>8/2/2016 2:25:13 PM</time>
                <lapse>2</lapse>
                <question>What is red?</question>
                <answer>a color</answer>
                <clue>0</clue>
                </Card>
                </Deck>";
            Repository.StoreString(content);
            Commands.AskForNextQuestion(time);
            var response = Commands.Answer(time, "a color");

            Assert.IsEmpty(response);
        }

        [Test]
        public void Upgrades_to_version_more_than_4_do_update_the_answer_field()
        {
            var time = DateTime.Parse("8/2/2016 2:25:13 PM").ToUniversalTime();
            time += TimeSpan.FromMinutes(3);

            string content = @"<Deck version=""1"">
                <Card version=""3"">
                <time>8/2/2016 2:25:13 PM</time>
                <lapse>2</lapse>
                <question>What is red?</question>
                <answer>a color</answer>
                <clue>0</clue>
                </Card>
                </Deck>";
            Repository.StoreString(content);
            Commands.AskForNextQuestion(time);
            Commands.Answer(time, "a color");

            time += TimeSpan.FromMinutes(13);

            var response = Commands.Answer(time, "a color");

            Assert.IsEmpty(response);
        }
    }
}

