using System;
using System.Collections.Generic;
using System.Linq;
using DeckAPI;


namespace Vanki
{
    public class MainClass
    {
        static readonly IVerbalMessages verbalMessages = new EnglishMessages();

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

            if (options.Action == Action.Invalid)
                return verbalMessages.WrongCmdArgs;

            using (var deck = new OnDiskDeck())
            {
                return ExecuteAction(now, options.Questions, options.Answers, options.CaseSensitive, options.RevertLastWrongAnswerAdd, deck, options.Action);
            }
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
