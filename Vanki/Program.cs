using System;
using System.Collections.Generic;
using System.Linq;

namespace Vanki
{

    public enum Action
    {
        AddCard,
        Revert,
        Answer,
        PrintNextQuestion,
        Nothing
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

            var deck = Persistence.Load();

            var ret = ExecuteActionFromOption(now, options, deck);

            Persistence.Save(deck);

            return ret;
        }

        static string ExecuteActionFromOption(DateTime now, Options options, IDeck deck)
        {
            return ExecuteAction(now, options.Questions, options.Answers, options.CaseSensitive, options.RevertLastWrongAnswerAdd, deck, options.Action);
        }

        static string ExecuteAction(DateTime now, IEnumerable<string> questions, IEnumerable<string> answers, bool caseSensitive, bool addWithRevert, IDeck deck, Action action)
        {
            switch (action)
            {
                case (Action.AddCard):
                    return AddNewCard(now, questions, answers, caseSensitive, deck);
                    
                case (Action.Answer):
                    return ProcessAnswer(deck, now, answers.First());
                    
                case (Action.PrintNextQuestion):
                    return PrintNextQuestion(deck, now);
                    
                case (Action.Revert):
                    return RevertLastWrongAnswer(deck, addWithRevert);
                    
                default:
                    return string.Empty;
            }
        }

        static string AddNewCard(DateTime now, IEnumerable<string> questions, IEnumerable<string> answers, bool caseSensitive, IDeck deck)
        {
            deck.AddNewCard(questions, answers, caseSensitive, now);
            return string.Empty;
        }

        static string ProcessAnswer (IDeck deck, DateTime answerTime, string answer)
        {

            if (!deck.HasPendingQuestion(answerTime))
                return verbalMessages.NothingToAnswer;

            if (!deck.IsAnswerCorrect(answer))
                deck.SetAnswerWrong(answer, answerTime);
            else
                deck.TreatCorrectAnswer(answerTime);
            return string.Empty;
        }

        static string PrintNextQuestion (IDeck deck, DateTime answerTime)
        {
            if (deck.IsEmpty())
                return verbalMessages.TheDeckIsEmpty;

            if (!deck.IsAnswerExpected(answerTime))
                return WaitABitPresentation(deck, answerTime);

            return GetQuestionPresentation(deck);
        }

        static string WaitABitPresentation(IDeck deck, DateTime answerTime)
        {
            var nextCardTime = deck.GetNextCardDueTime();
            return verbalMessages.ThereIsNoNextQuestion + "\n" + verbalMessages.ComeBackAtThisTime + ": " + nextCardTime.ToLocalTime() + " (" + verbalMessages.In + " " + (nextCardTime - answerTime) + ")" + "\n";
        }

        static string GetQuestionPresentation(IDeck deck)
        {
            var question = deck.GetNextQuestion();

            if (!deck.NextCardNeedsAClue())
                return question;
            
            return question + "\n" + verbalMessages.Clue + ": " + deck.GetHint();
        }

        static string RevertLastWrongAnswer(IDeck deck, bool add)
        {
            if (!deck.LastAnswerWasWrong())
                return verbalMessages.NothingToRevert;
            
            return RevertLastAnswer(deck, add);
        }

        static string RevertLastAnswer(IDeck deck, bool add)
        {
            var ret = verbalMessages.RevertLast;

            if (add)
                ret = AddLastAnwerAsCorrect(deck);
            
            deck.TreatLastAnswerAsCorrect();

            return ret;
        }

        static string AddLastAnwerAsCorrect(IDeck deck)
        {
            deck.AddLastAnswerAsCorrect();
            return verbalMessages.RevertAddLast;
        }
   }
}
