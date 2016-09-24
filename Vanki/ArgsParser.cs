using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NDesk.Options;

namespace Vanki
{
    public class Options
    {
        public IList<string> Questions { get; set; } = new List<string>();
        public IList<string> Answers { get; set; } = new List<string>();
        public bool ShowNext { get ; set; }
        public bool CaseSensitive { get; set; }
    }

    public static class ArgsParser
    {
        public static Options Parse(string[] args)
        {
            var opt = new Options();

            var p = new OptionSet()
            {
                { "q|question=", "The {QUESTION} to add or to answer",
                    v => opt.Questions  = Regex.Split(v, "~\\|~").ToList()},
                    { "n|next", "Display the {NEXT} question", v => opt.ShowNext = true },
                    { "a|answer=",
                        "The {ANSWER} to the next question or to the new question",
                        v => opt.Answers = Regex.Split(v, "~\\|~").Where(s => !string.IsNullOrWhiteSpace(s)).ToList()},
                    { "i", "The new question is case sensitive", v => opt.CaseSensitive = true}
            };
            p.Parse (args);

            return opt;
        }
    }
}

