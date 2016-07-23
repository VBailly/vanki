using System;


namespace Vanki
{
	public class MainClass
	{
		const string newEntryRegistered = "New entry registered\n";
		const string thatIsACorrectAnswer = "That is a correct answer!\n";
		const string theNextQuestionIsWhatIsRed = "The next question is:\n\"What is red?\"\n";
		const string thereIsNoNextQuestion = "There is no next question\n";

		public static void Main (string[] args)
		{
			var result = TestableMain (args, DateTime.Now);
			Console.Write (result);
		}

		public static string TestableMain(string[] args, DateTime time)
		{
			if (args.Length == 1)
				return PrintNextQuestion (time);
			if (args.Length == 2)
				return ProcessAnswer (time);
			Storage.SetTime (time);
			return newEntryRegistered;
		}
			
		static string ProcessAnswer (DateTime time)
		{
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
