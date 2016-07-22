using System;

namespace Vanki
{
	public class MainClass
	{
		public static void Main (string[] args)
		{
			var result = TestableMain (args);
			Console.Write (result);
		}

		public static string TestableMain(string[] args)
		{
			if (args.Length == 1)
				return "The next question is:\n\"What is red?\"\n";
			return "New entry registered\n";
		}
			
	}
}
