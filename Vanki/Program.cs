using System;
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
            var deck = Persistence.Load();

            var ret = ExecuteAction(now, options, deck);

            Persistence.Save(deck);

            return ret;
        }

        static string ExecuteAction(DateTime now, Options options, IDeck deck)
        {
            if (options.ShowNext)
                return PrintNextQuestion(deck, now);
            if (options.Questions.Any() && options.Answers.Any())
                return AddNewCard(now, options, deck);
            if (!(options.Answers == null || !options.Answers.Any()))
                return ProcessAnswer(deck, now, options.Answers[0]);
            if (options.RevertLastWrongAnswer)
                return RevertLastWrongAnswer(deck, options.RevertLastWrongAnswerAdd);
            return verbalMessages.WrongCmdArgs;
        }

        static string AddNewCard(DateTime now, Options options, IDeck deck)
        {
            deck.AddNewCard(options.Questions, options.Answers, options.CaseSensitive, now);
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
