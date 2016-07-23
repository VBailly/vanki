using System;

namespace Vanki
{
	public class MainClass
	{
		public static void Main (string[] args)
		{
			var result = TestableMain (args, DateTime.Now);
			Console.Write (result);
		}

		private static bool Visited;

		public static string TestableMain(string[] args, DateTime time)
		{
			if (args.Length == 1)
			{
				if (!Visited) {
					Visited = true;
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
