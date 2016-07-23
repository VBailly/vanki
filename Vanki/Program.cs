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
			if (IsLapsePassed (time)) {
				Storage.SetLapse ((time - Storage.GetTime()).Minutes * 2);
				Storage.SetTime (time);
			}

			return thatIsACorrectAnswer;
		}

		static bool IsLapsePassed (DateTime time)
		{
			return (time - Storage.GetTime()) > TimeSpan.FromMinutes (Storage.GetLapse());
		}

		static string PrintNextQuestion (DateTime time)
		{
			bool visited = Storage.HasBeenVisited ();

			if (IsLapsePassed (time))
				return theNextQuestionIsWhatIsRed;
			if (!visited) {
				Storage.SetVisited (true);
				return theNextQuestionIsWhatIsRed;
			}
			return thereIsNoNextQuestion;
		}


	}
}
