﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Vanki
{
    public class MainClass
    {
        static IVerbalMessages verbalMessages = new EnglishMessages();

        public static int Main (string[] args)
        {
            var result = TestableMain (args, DateTime.UtcNow);
            
            var ret = result.StartsWith(verbalMessages.ThereIsNoNextQuestion, StringComparison.CurrentCulture) ? 7 : 0;
            Console.Write (result + "\n");
            return ret;
        }

        public static string TestableMain(string[] args, DateTime now)
        {
            var options = ArgsParser.Parse(args);

            if (options.Action == Action.Nothing)
                return verbalMessages.WrongCmdArgs;

            var deck = Persistence.Load();

            var ret = ExecuteAction(now, options.Questions, options.Answers, options.CaseSensitive, options.RevertLastWrongAnswerAdd, deck, options.Action);

            Persistence.Save(deck);

            return ret;
        }

        static string ExecuteAction(DateTime now, IEnumerable<string> questions, IEnumerable<string> answers, bool caseSensitive, bool addWithRevert, IDeck deck, Action action)
        {
            var state = StateFactory.CreateState(deck, now, verbalMessages);

            switch (action)
            {
                case (Action.AddCard):
                    return state.AddNewCard(questions, answers, caseSensitive);
                    
                case (Action.Answer):
                    return state.ProcessAnswer(answers.First());
                    
                case (Action.PrintNextQuestion):
                    return state.PrintNextQuestion();
                    
                case (Action.Revert):
                    return state.RevertLastWrongAnswer(addWithRevert);
                    
                default:
                    return string.Empty;
            }
        }





   }
}
