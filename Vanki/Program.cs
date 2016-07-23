using System;
using System.Xml.Linq;
using System.IO;

namespace Vanki
{
	public class MainClass
	{
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
			return "New entry registered\n";
		}

		static void SetVisited (bool value)
		{
			var xEl = new XElement ("visited", value ? "yes" : "no" );
			File.WriteAllText ("db.xml", xEl.ToString ());
		}

		static string ProcessAnswer (DateTime time)
		{
			if ((time - DateTime.Now) > TimeSpan.FromMinutes (2)) {
				SetVisited (false);
			}
			return "That is a correct answer!\n";
		}

		static string PrintNextQuestion (DateTime time)
		{
			bool visited = HasBeenVisited ();

			if ((time - DateTime.Now) > TimeSpan.FromMinutes (2))
				return "The next question is:\n\"What is red?\"\n";
			if (!visited) {
				SetVisited (true);
				return "The next question is:\n\"What is red?\"\n";
			}
			return "There is no next question\n";
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
