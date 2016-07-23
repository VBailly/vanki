using System;
using System.Xml.Linq;
using System.IO;

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
			return newEntryRegistered;
		}

		static void SetVisited (bool value)
		{
			var xEl = new XElement ("visited", value ? "yes" : "no" );
			File.WriteAllText ("db.xml", xEl.ToString ());
		}

		static string ProcessAnswer (DateTime time)
		{
			if (LapseGreaterThan2Min (time)) {
				SetVisited (false);
			}
			return thatIsACorrectAnswer;
		}

		static bool LapseGreaterThan2Min (DateTime time)
		{
			return (time - DateTime.Now) > TimeSpan.FromMinutes (2);
		}

		static string PrintNextQuestion (DateTime time)
		{
			bool visited = HasBeenVisited ();

			if (LapseGreaterThan2Min (time))
				return theNextQuestionIsWhatIsRed;
			if (!visited) {
				SetVisited (true);
				return theNextQuestionIsWhatIsRed;
			}
			return thereIsNoNextQuestion;
		}

		static bool HasBeenVisited ()
		{
			if (!File.Exists ("db.xml"))
				return false;
			
			var xdoc = XElement.Load ("db.xml");
			return xdoc.Value == "yes";

		}
	}
}
