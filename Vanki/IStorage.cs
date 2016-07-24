using System;

namespace Vanki
{
	public interface IStorage
	{
		DateTime LastAnswerTime { get; set; }

		string Question { get; set; }

		int CurrentInterval { get; set; }

		string Answer { get; set; }

		bool DataExist();
	}
}