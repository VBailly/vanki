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
        public bool RevertLastWrongAnswer { get; set; }
        public bool RevertLastWrongAnswerAdd { get; set; }
        public Action Action { get; set; }
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
                { "a|answer=", "The {ANSWER} to the next question or to the new question",
                    v => opt.Answers = Regex.Split(v, "~\\|~").Where(s => !string.IsNullOrWhiteSpace(s)).ToList()},
                { "i", "The new question is case sensitive", v => opt.CaseSensitive = true},
                { "r|revert=", "Revert the last wrong answers", v => {
                    opt.RevertLastWrongAnswer = true;
                    opt.RevertLastWrongAnswerAdd = v.Contains("add");
                }}
            };
            p.Parse (args);

            opt.Action = GetAction(opt);

            return opt;
        }

        private static Action GetAction(Options options)
        {
            if (options.ShowNext)
                return Action.PrintNextQuestion;

            if (options.Questions.Any() && options.Answers.Any())
                return Action.AddCard;

            if (!(options.Answers == null || !options.Answers.Any()))
                return Action.Answer;

            if (options.RevertLastWrongAnswer)
                return Action.Revert;

            return Action.Nothing;

        }
    }
}

