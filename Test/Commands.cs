using System;
using Vanki;

namespace Test
{
	public static class Commands
	{
		public static string Answer(DateTime time, string answer)
		{
			return MainClass.TestableMain(new[] { "--answer", answer }, time);
		}

		public static string AskForNextQuestion(DateTime time)
		{
			return MainClass.TestableMain(new[] { "--next" }, time);
		}

		public static string RegisterQuestion(string question, string answer, DateTime time)
		{
			return MainClass.TestableMain(new[] { "-q", question, "-a", answer }, time);
		}
	}
}

