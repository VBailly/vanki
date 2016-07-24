using NUnit.Framework;
using System;
using System.IO;

namespace Test
{
	[TestFixture]
	public class TestWithTwoQuestion
	{
		static readonly string Question1 = "What is the color of a red fish?";
		static readonly string NextQuestion1Message = string.Format(ConsoleOutputs.NextQuestionMessage, Question1);

		[SetUp]
		public void SetUp()
		{
			File.Delete("db.xml");
		}

		[Test]
		public void Oldest_command_of_newly_created_questions_is_the_next_one()
		{
			var time = DateTime.Now;
			Commands.RegisterQuestion(Question1, "red", time);
			time += TimeSpan.FromSeconds(2);
			Commands.RegisterQuestion("What is the color of a blue fish?", "blue", time);
			time += TimeSpan.FromSeconds(2);
			var response = Commands.AskForNextQuestion(time);

			Assert.AreEqual(NextQuestion1Message, response);
		}
	}
}

