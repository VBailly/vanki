using NUnit.Framework;
using System;
using System.IO;
using Vanki;

namespace Test
{
	[TestFixture]
	public class TestWithTwoQuestion
	{
		static readonly string Question1 = "What is the color of a red fish?";
		static readonly string Question2 = "What is the color of a blue fish?";
		static readonly string NextQuestion1Message = string.Format(ConsoleOutputs.NextQuestionMessage, Question1);
		static readonly string NextQuestion2Message = string.Format(ConsoleOutputs.NextQuestionMessage, Question2);


		[SetUp]
		public void SetUp()
		{
			File.Delete("db.xml");
            Clock.Getter = null;
		}

		[Test]
		public void Oldest_command_of_newly_created_questions_is_the_next_one()
		{
			var time = DateTime.Now;
			Commands.RegisterQuestion(Question1, "red");
			Clock.Getter = () => DateTime.Now + TimeSpan.FromSeconds(2);
			Commands.RegisterQuestion(Question2, "blue");
			Clock.Getter = () => DateTime.Now + TimeSpan.FromSeconds(4);
			var response = Commands.AskForNextQuestion();

			Assert.AreEqual(NextQuestion1Message, response);
		}

		[Test]
		public void We_can_answer_the_first_question()
		{
			var time = DateTime.Now;
			Commands.RegisterQuestion(Question1, "red");
			Clock.Getter = () => DateTime.Now + TimeSpan.FromSeconds(2);
			Commands.RegisterQuestion(Question2, "blue");
			Clock.Getter = () => DateTime.Now + TimeSpan.FromSeconds(4);
			var response = Commands.Answer("red");

			Assert.AreEqual(ConsoleOutputs.CorrectAnswerMessage, response);
		}

		[Test]
		public void The_second_question_is_presented_after_we_answer_the_first_one()
		{
			var time = DateTime.Now;
			Commands.RegisterQuestion(Question1, "red");
			Clock.Getter = () => DateTime.Now + TimeSpan.FromSeconds(2);
			Commands.RegisterQuestion(Question2, "blue");
			Clock.Getter = () => DateTime.Now + TimeSpan.FromSeconds(4);
			Commands.Answer("red");
			Clock.Getter = () => DateTime.Now + TimeSpan.FromSeconds(6);

			var response = Commands.AskForNextQuestion();

			Assert.AreEqual(NextQuestion2Message, response);
		}

		[Test]
		public void The_second_question_can_be_properly_answered()
		{
			var time = DateTime.Now;
			Commands.RegisterQuestion(Question1, "red");
			Clock.Getter = () => DateTime.Now + TimeSpan.FromSeconds(2);
			Commands.RegisterQuestion(Question2, "blue");
			Clock.Getter = () => DateTime.Now + TimeSpan.FromSeconds(4);
			Commands.Answer("red");
			Clock.Getter = () => DateTime.Now + TimeSpan.FromSeconds(6);
			var response = Commands.Answer("blue");

			Assert.AreEqual(ConsoleOutputs.CorrectAnswerMessage, response);
		}

		[Test]
		public void No_questions_available_after_the_two_answers()
		{
			var time = DateTime.Now;
			Commands.RegisterQuestion(Question1, "red");
			Clock.Getter = () => DateTime.Now + TimeSpan.FromSeconds(2);
			Commands.RegisterQuestion(Question2, "blue");
			Clock.Getter = () => DateTime.Now + TimeSpan.FromSeconds(4);
			Commands.Answer("red");
			Clock.Getter = () => DateTime.Now + TimeSpan.FromSeconds(6);
			Commands.Answer("blue");

			var response = Commands.AskForNextQuestion();

			Assert.IsTrue(response.Contains(ConsoleOutputs.NoNextQuestionMessage));
		}

		[Test]
		public void Question_available_again_after_the_two_answers_and_3_min()
		{
			var time = DateTime.Now;
			Commands.RegisterQuestion(Question1, "red");
			Clock.Getter = () => DateTime.Now + TimeSpan.FromSeconds(2);
			Commands.RegisterQuestion(Question2, "blue");
			Clock.Getter = () => DateTime.Now + TimeSpan.FromSeconds(4);
			Commands.Answer("red");
			Clock.Getter = () => DateTime.Now + TimeSpan.FromSeconds(6);
			Commands.Answer("blue");
			Clock.Getter = () => DateTime.Now + TimeSpan.FromMinutes(9);

			var response = Commands.AskForNextQuestion();

			Assert.AreEqual(NextQuestion1Message, response);
		}
	}
}

