using System;
using NDesk.Options;

namespace Vanki
{
	public class Options
	{
		public string Question { get ; set; }
		public string Answer { get ; set; }
		public bool ShowNext { get ; set; }
	}

	public static class ArgsParser
	{
		public static Options Parse(string[] args)
		{
			var opt = new Options ();

			var p = new OptionSet()
			{
				{ "q|question=", "The {QUESTION} to add or to answer", v => opt.Question = v},
				{ "n|next", "Display the {NEXT} question", v => opt.ShowNext = true },
				{ "a|answer=", "The {ANSWER} to the next question or to the new question", v => opt.Answer = v}
			};
			p.Parse (args);

			return opt; 
		}
	}
}

