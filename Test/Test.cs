using NUnit.Framework;
using System;
using System.IO;
using Vanki;

namespace Test
{
	[TestFixture ()]
	public class Test
	{
		TextWriter tmp_;
		MemoryStream memoryStream_;
		StreamWriter sw_;

		[SetUp]
		public void SetUp ()
		{
			tmp_ = Console.Out;
			memoryStream_ = new MemoryStream ();
			sw_ = new StreamWriter (memoryStream_);
			Console.SetOut (sw_);
		}

		[TearDown]
		public void TearDown ()
		{
			Console.SetOut (tmp_);
		}


		[Test ()]
		public void TestCase ()
		{
			MainClass.Main (null);
			sw_.Flush ();
			memoryStream_.Position = 0;

			var sr = new StreamReader(memoryStream_);
			var myStr = sr.ReadToEnd();

			Assert.AreEqual (myStr, "Hello World!\n");
		}
	}
}

