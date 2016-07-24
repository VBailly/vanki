using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;


namespace Vanki
{
	public class MainClass
	{
		const string newEntryRegistered = "New entry registered\n";
		const string thatIsACorrectAnswer = "That is a correct answer!\n";
		const string theNextQuestionIsWhatIsRed = "The next question is:\n\"What is red?\"\n";
		const string thereIsNoNextQuestion = "There is no next question\n";
		const string cannotAnswer = "You cannot answer because there is no question pending\n";

		public static void Main (string[] args)
		{
			var result = TestableMain (args, DateTime.Now);
			Console.Write (result);
		}

		public static string TestableMain(string[] args, DateTime time)
		{
			var options = ArgsParser.Parse (args);
			if (options.ShowNext)
				return PrintNextQuestion (time);
			if (!string.IsNullOrEmpty (options.Question) && !string.IsNullOrEmpty (options.Answer))
				return AddQuestion (time);
			if (!string.IsNullOrEmpty (options.Answer))
				return ProcessAnswer (time, options.Answer);


			return "wrong command line arguments";
		}

		static string AddQuestion(DateTime time)
		{
			Storage.SetTime (time);
			return newEntryRegistered;
		}
			
		static string ProcessAnswer (DateTime time, string answer)
		{
			if (answer != "a color")
			{
				return "WRONG! The correct answer is \"a color\".\n";
			}
			if (!IsLapsePassed(time))
				return cannotAnswer;
			
			Storage.SetLapse (Math.Max(2, (time - Storage.GetTime()).Minutes * 2));
			Storage.SetTime (time);

			return thatIsACorrectAnswer;
		}

		static bool IsLapsePassed (DateTime time)
		{
			var lapse = Storage.GetLapse();
			var storedTime = Storage.GetTime ();
			return time > storedTime + TimeSpan.FromMinutes (lapse);
		}

		static string PrintNextQuestion (DateTime time)
		{
			if (IsLapsePassed (time))
				return theNextQuestionIsWhatIsRed;
			return thereIsNoNextQuestion;
		}


	}
}
