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
			return "Hello World!\n";
		}
			
	}
}
