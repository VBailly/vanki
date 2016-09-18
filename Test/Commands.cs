using System;
using System.Collections.Generic;
using System.Linq;
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

        public static string RegisterQuestionCaseSensitive(string question, string answer)
        {
            return MainClass.TestableMain(new[] { "-q", question, "-a", answer, "-i" });
        }

        public static string RegisterQuestion(string question, IEnumerable<string> answers)
        {
            return MainClass.TestableMain(new[] { "-q", question, "-a", string.Join("|", answers) });
        }

        internal static object AskForAClue()
        {
            return MainClass.TestableMain(new[] { "--clue"});
        }
   }
}

