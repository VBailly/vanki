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
                SetAnswerWrong(deck, answer, card, answerTime);
            else
                deck.TreatCorrectAnswer(answerTime);
            return string.Empty;
        }

        static void SetAnswerWrong(Deck deck, string answer, ICard card, DateTime now)
        {
            deck.SaveLastAnswer(answer, now);
            card.Reset();
        }

        static string PrintNextQuestion (Deck deck, DateTime answerTime)
        {
            if (!deck.Cards.Any())
                return verbalMessages.TheDeckIsEmpty;

            if (!deck.IsAnswerExpected(answerTime))
                return WaitABitPresentation(deck, answerTime);

            var card = deck.GetNextCardBefore(answerTime);
            return GetQuestionPresentation(card);
        }

        static string WaitABitPresentation(Deck deck, DateTime answerTime)
        {
            var nextCardTime = deck.GetNextCard().DueTime;
            return verbalMessages.ThereIsNoNextQuestion + "\n" + verbalMessages.ComeBackAtThisTime + ": " + nextCardTime.ToLocalTime() + " (" + verbalMessages.In + " " + (nextCardTime - answerTime) + ")" + "\n";
        }


        static string GetQuestionPresentation(ICard card)
        {
            var question = card.Questions.OrderBy(x => Guid.NewGuid()).First();

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
            var lastAnswer = deck.LastAnswer;
            deck.ResetLastAnswer();

            var card = deck.GetNextCard();

            return UpdateCardFromAnswer(card, add, lastAnswer);
        }

        static string UpdateCardFromAnswer(ICard card, bool add, LastAnswer lastAnswer)
        {
            card.PromoteFrom(lastAnswer.PreviousLapse);

            if (add)
                return AddAnswer(lastAnswer, card);

            return verbalMessages.RevertLast;
        }

        static string AddAnswer(LastAnswer lastWrongAnswer, ICard card)
        {
            card.AddAnswer(lastWrongAnswer.Answer);
            return verbalMessages.RevertAddLast;
        }
   }
}
