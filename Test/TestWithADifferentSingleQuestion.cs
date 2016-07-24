using System.IO;
using NUnit.Framework;

namespace Test
{
	[TestFixture]
	public class TestWithADifferentSingleQuestion
	{
		[SetUp]
		public void SetUp()
		{
			File.Delete("db.xml");
		}

		[Test]
		public void Register_a_new_entry()
		{
			var response = Commands.RegisterQuestion("What color is the sky", "blue");

			Assert.AreEqual(ConsoleOutputs.NewEntryMessage, response);
		}
	}
}

