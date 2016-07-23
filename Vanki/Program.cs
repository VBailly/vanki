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

			
			bool visited = false;

			if (File.Exists ("db.xml")) 
			{
				var xdoc = XElement.Load("db.xml");
				visited = xdoc.Value == "yes";
			}


			if (args.Length == 1)
			{
				if ((time - DateTime.Now) > TimeSpan.FromMinutes(2))
					return "The next question is:\n\"What is red?\"\n";
				if (!visited) {
					var xEl = new XElement ("visited", "yes");
					File.WriteAllText ("db.xml", xEl.ToString ());
					return "The next question is:\n\"What is red?\"\n";

				}
				return "There is no next question\n";
			}
			if (args.Length == 2)
				return "That is a correct answer!\n";
			return "New entry registered\n";
		}
			
	}
}
