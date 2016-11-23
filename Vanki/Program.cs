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

            string ret = string.Empty;
            ret = ExecuteAction(now, options, deck, ret);

            Persistence.Save(deck);

            return ret;
        }

        static string ExecuteAction(DateTime now, Options options, Deck deck, string ret)
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
            deck.Cards.Add(new Card
            {
                Id = Guid.NewGuid(),
                Questions = options.Questions,
                Answers = options.Answers,
                CaseSensitiveAnswers = options.CaseSensitive,
                LastAnswerTime = now,
            });
            return string.Empty;
        }

        static Card GetNextCard(Deck deck, DateTime time)
        {
            return deck.Cards.Where(c => c.DueTime <= time).OrderBy(c => c.DueTime).FirstOrDefault();
        }

        static string GetHint(string answer, int size)
        {
            var answers = answer.Split(',').Select(s => s.Trim());
            return string.Join(", ", answers.Select(w => string.Join(" ", w.Split(' ').Select(s => new string(s.Take(size).ToArray())))));
        }

        static string ProcessAnswer (Deck deck, DateTime answerTime, string answer)
        {
            var card = GetNextCard(deck, answerTime);
            if (card == null)
                return verbalMessages.NothingToAnswer;

            if (!IsAnswerCorrect(answer, card))
                SetAnswerWrong(deck, answer, card);
            else
                TreatCorrectAnswer(deck, answerTime, card);
            return string.Empty;
        }

        static void TreatCorrectAnswer(Deck deck, DateTime answerTime, Card card)
        {
            deck.LastAnswer = LastAnswer.NullAnswer;
            card.Promote(answerTime);
        }

        static bool IsAnswerCorrect(string answer, Card card)
        {
            Func<string, bool> check = GetCheckingFunction(answer, card);

            return card.Answers.Any(check);
        }

        static Func<string, bool> GetCheckingFunction(string answer, Card card)
        {
            if (card.CaseSensitiveAnswers)
                return a => a == answer;
            
            return a => a.ToLower() == answer.ToLower();
        }

        static void SetAnswerWrong(Deck deck, string answer, Card card)
        {
            deck.LastAnswer = new LastAnswer
            {
                Answer = answer,
                PreviousLapse = card.CurrentInterval
            };
            card.Reset();
        }

        static string PrintNextQuestion (Deck deck, DateTime answerTime)
        {
            if (!deck.Cards.Any())
                return verbalMessages.TheDeckIsEmpty;
            var card = GetNextCard(deck, answerTime);
            if (card != null)
                return GetQuestionPresentation(card);

            var nextCardTime = GetNextCard(deck).DueTime;
            return verbalMessages.ThereIsNoNextQuestion + "\n" + verbalMessages.ComeBackAtThisTime + ": " + nextCardTime.ToLocalTime() + " (" + verbalMessages.In + " " + (nextCardTime - answerTime) + ")" + "\n";
        }

        static Card GetNextCard(Deck deck)
        {
            return deck.Cards.OrderBy(c => c.DueTime).First();
        }

        static string GetQuestionPresentation(Card card)
        {
            var question = card.Questions.OrderBy(x => Guid.NewGuid()).First();

            if (card.Clue == 0)
                return question;
            
            return question + "\n" + verbalMessages.Clue + ": " + GetHint(card.Answers[0], card.Clue);
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
            deck.LastAnswer = LastAnswer.NullAnswer;

            var card = GetNextCard(deck);

            return UpdateCardFromAnswer(card, add, lastAnswer);
        }

        static string UpdateCardFromAnswer(Card card, bool add, LastAnswer lastAnswer)
        {
            PromoteCardFrom(card, lastAnswer);

            if (add)
                return AddAnswer(lastAnswer, card);

            return verbalMessages.RevertLast;
        }

        static void PromoteCardFrom(Card card, LastAnswer lastAnswer)
        {
            card.CurrentInterval = lastAnswer.PreviousLapse;
            card.DecreaseClue();

        }

        static string AddAnswer(LastAnswer lastWrongAnswer, Card card)
        {
            card.Answers.Add(lastWrongAnswer.Answer);
            return verbalMessages.RevertAddLast;
        }
   }
}
