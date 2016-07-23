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

		public static string TestableMain(string[] args, DateTime time)
		{
			if (args.Length == 1)
				return "The next question is:\n\"What is red?\"\n";
			if (args.Length == 2)
				return "That is a correct answer!\n";
			return "New entry registered\n";
		}
			
	}
}
