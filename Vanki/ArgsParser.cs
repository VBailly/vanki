using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NDesk.Options;

namespace Vanki
{
    public class Options
    {
        public string Question { get ; set; }
        public IList<string> Answers { get ; set; }
        public bool ShowNext { get ; set; }
        public bool Clue { get; set;}
        public bool CaseSensitive { get; set; }
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
                    { "a|answer=",
                        "The {ANSWER} to the next question or to the new question",
                        v => opt.Answers = Regex.Split(v, "~|~").Where(s => !string.IsNullOrWhiteSpace(s)).ToList()},
                    { "c|clue", "Ask for a {CLUE} for the next question", v => opt.Clue = true},
                    { "i", "The new question is case sensitive", v => opt.CaseSensitive = true}
            };
            p.Parse (args);

            return opt;
        }
    }
}

