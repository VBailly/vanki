using System;
using System.IO;
using NUnit.Framework;

namespace Test
{

	[TestFixture]
	public class TestWithADifferentSingleQuestion
	{
		static readonly string Question = "What color is the sky?";
		static readonly string NextQuestionMessage = string.Format(ConsoleOutputs.NextQuestionMessage, Question);

		[SetUp]
		public void SetUp()
		{
			File.Delete("db.xml");
		}

		[Test]
		public void Register_a_new_entry()
		{
			var response = RegisterQuestion();

			Assert.AreEqual(ConsoleOutputs.NewEntryMessage, response);
		}

		[Test]
		public void A_question_is_available_straight_after_being_registered()
		{
			RegisterQuestion();

			var response = Commands.AskForNextQuestion(DateTime.Now);

			Assert.AreEqual(NextQuestionMessage, response);
		}

		static string RegisterQuestion()
		{
			return Commands.RegisterQuestion(Question, "blue");
		}
	}
}

