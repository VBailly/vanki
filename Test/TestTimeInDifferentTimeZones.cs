using System;
using NUnit.Framework;
using Storage;

namespace Test
{
    [TestFixture]
    public class TestTimeInDifferentTimeZones
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
        public void Traveling_to_west_does_not_disturb_progress()
        {
            var time = DateTime.Now;

            Clock.LocalTimeGetter = () => time;
            Clock.HoursDifferenceFromGlobal = () => 0;

            Commands.RegisterQuestion("What is red?", "a color");
            Commands.Answer("a color");

            time += TimeSpan.FromMinutes(5);
            Clock.LocalTimeGetter = () => time ;
            Commands.Answer("a color");

            time += TimeSpan.FromHours(5);
            Clock.HoursDifferenceFromGlobal = () => 5;

            var response = Commands.AskForNextQuestion();

            Assert.IsTrue(response.Contains(ConsoleOutputs.NoNextQuestionMessage));
        }

        [Test]
        public void Traveling_to_east_does_not_disturb_progress()
        {
            var time = DateTime.Now;

            Clock.LocalTimeGetter = () => time;
            Clock.HoursDifferenceFromGlobal = () => 0;

            Commands.RegisterQuestion("What is red?", "a color");
            Commands.Answer("a color");

            time += TimeSpan.FromMinutes(5);
            Clock.LocalTimeGetter = () => time;
            Commands.Answer("a color");

            time -= TimeSpan.FromHours(5);
            Clock.HoursDifferenceFromGlobal = () => -5;

            var response = Commands.AskForNextQuestion();

            Assert.IsTrue(response.Contains(ConsoleOutputs.NoNextQuestionMessage));
        }


    }
}

