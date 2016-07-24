using System;
using System.IO;
using NUnit.Framework;

namespace Test
{
	[TestFixture]
	public class TestWithoutAnQuestionRegistered
	{
		[SetUp]
		public void SetUp()
		{
			File.Delete("db.xml");
		}

		[Test]
		public void There_is_no_next_question_if_we_dont_register_one()
		{
			var response = Commands.AskForNextQuestion(DateTime.Now);

			Assert.AreEqual(ConsoleOutputs.EmptyDeckMessage, response);
		}

		[Test]
		public void We_cannot_anwer_next_question_if_we_dont_register_one()
		{
			var response = Commands.Answer(DateTime.Now, "a fish");

			Assert.AreEqual(ConsoleOutputs.CannotAnswerMessage, response);
		}



	}
}

