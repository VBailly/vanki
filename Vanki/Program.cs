using System;


namespace Vanki
{
	public class MainClass
	{
		const string newEntryRegistered = "New entry registered\n";
		const string thatIsACorrectAnswer = "That is a correct answer!\n";
		static readonly string theNextQuestionIs = "The next question is:\n\"{0}\"\n";
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
				return AddQuestion (time, options.Question, options.Answer);
			if (!string.IsNullOrEmpty (options.Answer))
				return ProcessAnswer (time, options.Answer);


			return "wrong command line arguments\n";
		}

		static string AddQuestion(DateTime time, string question, string answer)
		{
			Storage.LastAnswerTime = time;
			Storage.SetAnswer(answer);
			Storage.SetQuestion(question);
			return newEntryRegistered;
		}
			
		static string ProcessAnswer (DateTime time, string answer)
		{
			if (!IsLapsePassed(time) || !Storage.DataExist())
				return cannotAnswer;

			var correctAnswer = Storage.GetAnswer();

			if (answer != correctAnswer)
			{
				Storage.SetLapse(0);
				Storage.LastAnswerTime = time;
				return string.Format("WRONG! The correct answer is \"{0}\".\n", correctAnswer);

			}
			
			Storage.SetLapse (Math.Max(2, (time - Storage.LastAnswerTime).Minutes * 2));
			Storage.LastAnswerTime = time;

			return thatIsACorrectAnswer;
		}

		static bool IsLapsePassed (DateTime time)
		{
			var lapse = Storage.GetLapse();
			var storedTime = Storage.LastAnswerTime;
			return time > storedTime + TimeSpan.FromMinutes (lapse);
		}

		static string PrintNextQuestion (DateTime time)
		{
			if (IsLapsePassed (time) && Storage.DataExist())
				return string.Format(theNextQuestionIs, Storage.GetQuestion());
			return thereIsNoNextQuestion;
		}


	}
}
