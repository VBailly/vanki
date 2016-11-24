using System;
using System.Collections.Generic;
using System.Linq;

namespace Vanki
{
    public class INeedToFindAName : IDeck, IDisposable
    {
        readonly IDeck deck;

        public INeedToFindAName()
        {
            deck = Persistence.Load();
        }

        public void AddLastAnswerAsCorrect()
        {
            deck.AddLastAnswerAsCorrect();
        }

        public void AddNewCard(IEnumerable<string> questions, IEnumerable<string> answers, bool caseSensitive, DateTime now)
        {
            deck.AddNewCard(questions, answers, caseSensitive, now);
        }

        public void Dispose()
        {
            Persistence.Save(deck);
        }

        public string GetHint()
        {
            return deck.GetHint();
        }

        public DateTime GetNextCardDueTime()
        {
            return deck.GetNextCardDueTime();
        }

        public string GetNextQuestion()
        {
            return deck.GetNextQuestion();
        }

        public DeckState GetState(DateTime now)
        {
            return deck.GetState(now);
        }

        public bool IsAnswerCorrect(string answer)
        {
            return deck.IsAnswerCorrect(answer);
        }

        public bool LastAnswerWasWrong()
        {
            return deck.LastAnswerWasWrong();
        }

        public bool NextCardNeedsAClue()
        {
            return deck.NextCardNeedsAClue();
        }

        public void SetAnswerWrong(string answer, DateTime now)
        {
            deck.SetAnswerWrong(answer, now);
        }

        public void TreatCorrectAnswer(DateTime now)
        {
            deck.TreatCorrectAnswer(now);
        }

        public void TreatLastAnswerAsCorrect()
        {
            deck.TreatLastAnswerAsCorrect();
        }
    }

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

            using (var deck = new INeedToFindAName())
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
