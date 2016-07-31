using System;
using Vanki;

namespace Test
{
	public static class Commands
	{
		public static string Answer(string answer)
		{
            return MainClass.TestableMain(new[] { "--answer", answer });
		}

		public static string AskForNextQuestion()
		{
			return MainClass.TestableMain(new[] { "--next" });
		}

		public static string RegisterQuestion(string question, string answer)
		{
			return MainClass.TestableMain(new[] { "-q", question, "-a", answer });
		}

        internal static object AskForAClue()
        {
            return MainClass.TestableMain(new[] { "--clue"});
        }
   }
}

