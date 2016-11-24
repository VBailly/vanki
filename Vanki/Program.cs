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

        static string ExecuteAction(DateTime now, Options options, Deck deck)
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

        static string AddNewCard(DateTime now, Options options, Deck deck)
        {
            deck.AddNewCard(options.Questions, options.Answers, options.CaseSensitive, now);
            return string.Empty;
        }

        static string ProcessAnswer (Deck deck, DateTime answerTime, string answer)
        {
            var card = deck.GetNextCardBefore(answerTime);
            if (card == null)
                return verbalMessages.NothingToAnswer;

            if (!card.IsAnswerCorrect(answer))
                deck.SetAnswerWrong(answer, answerTime);
            else
                deck.TreatCorrectAnswer(answerTime);
            return string.Empty;
        }

       

        static string PrintNextQuestion (Deck deck, DateTime answerTime)
        {
            if (!deck.Cards.Any())
                return verbalMessages.TheDeckIsEmpty;

            if (!deck.IsAnswerExpected(answerTime))
                return WaitABitPresentation(deck, answerTime);

            var card = deck.GetNextCardBefore(answerTime);
            return GetQuestionPresentation(card, deck);
        }

        static string WaitABitPresentation(Deck deck, DateTime answerTime)
        {
            var nextCardTime = deck.GetNextCardDueTime();
            return verbalMessages.ThereIsNoNextQuestion + "\n" + verbalMessages.ComeBackAtThisTime + ": " + nextCardTime.ToLocalTime() + " (" + verbalMessages.In + " " + (nextCardTime - answerTime) + ")" + "\n";
        }


        static string GetQuestionPresentation(ICard card, Deck deck)
        {
            var question = deck.GetNextQuestion();

            if (!card.NeedsAClue())
                return question;
            
            return question + "\n" + verbalMessages.Clue + ": " + card.GetHint();
        }

        static string RevertLastWrongAnswer(Deck deck, bool add)
        {
            if (deck.LastAnswer == LastAnswer.NullAnswer)
                return verbalMessages.NothingToRevert;
            
            return RevertLastAnswer(deck, add);
        }

        static string RevertLastAnswer(Deck deck, bool add)
        {
            var ret = verbalMessages.RevertLast;

            if (add)
                ret = AddLastAnwerAsCorrect(deck);
            
            deck.TreatLastAnswerAsCorrect();

            return ret;
        }

        static string AddLastAnwerAsCorrect(Deck deck)
        {
            deck.AddLastAnswerAsCorrect();
            return verbalMessages.RevertAddLast;
        }
   }
}
