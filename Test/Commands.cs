﻿using System;
using System.Collections.Generic;
using Vanki;

namespace Test
{
    public static class Commands
    {
        public static string Answer(DateTime time, string answer)
        {
            return MainClass.TestableMain(new[] { "--answer", answer }, time);
        }

        public static string AskForNextQuestion(DateTime time)
        {
            return MainClass.TestableMain(new[] { "--next" }, time);
        }

        public static string RevertLastWrongAnswer(DateTime time)
        {
            return MainClass.TestableMain(new[] { "--revert", "discard" }, time);
        }

        public static string RevertAddLastWrongAnswer(DateTime time)
        {
            return MainClass.TestableMain(new[] { "--revert", "add" }, time);
        }

        public static string RegisterQuestion(DateTime time, string question, string answer)
        {
            return MainClass.TestableMain(new[] { "-q", question, "-a", answer }, time);
        }

        public static string RegisterQuestionCaseSensitive(DateTime time, string question, string answer)
        {
            return MainClass.TestableMain(new[] { "-q", question, "-a", answer, "-i" }, time);
        }

        public static string RegisterQuestion(DateTime time, string question, IEnumerable<string> answers)
        {
            return MainClass.TestableMain(new[] { "-q", question, "-a", string.Join("~|~", answers) }, time);
        }

        public static string RegisterQuestion(DateTime time, IEnumerable<string> questions, IEnumerable<string> answers)
        {
            return MainClass.TestableMain(new[] { "-q", string.Join("~|~", questions), "-a", string.Join("~|~", answers) }, time);
        }
   }
}

