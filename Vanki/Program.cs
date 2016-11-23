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
            var options = ArgsParser.Parse (args);
            var deck = Persistence.Load();

            string ret = string.Empty;

            if (options.ShowNext)
                ret = PrintNextQuestion(deck, now);
            else if (options.Questions.Any() && options.Answers.Any())
                AddNewCard(now, options, deck);
            else if (!(options.Answers == null || !options.Answers.Any()))
                ret = ProcessAnswer(deck, now, options.Answers[0]);
            else if (options.RevertLastWrongAnswer)
                ret = RevertLastWrongAnswer(deck, options.RevertLastWrongAnswerAdd);
            else
                ret = verbalMessages.WrongCmdArgs;

            Persistence.Save(deck);

            return ret;
        }

        static void AddNewCard(DateTime now, Options options, Deck deck)
        {
            deck.Cards.Add(new Card
            {
                Id = Guid.NewGuid(),
                Questions = options.Questions,
                Answers = options.Answers,
                CaseSensitiveAnswers = options.CaseSensitive,
                Clue = 0,
                LastAnswerTime = now,
                CurrentInterval = 0
            });
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
            deck.LastWrongAnswer = WrongAnswer.NoWrongAnswer;
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
            deck.LastWrongAnswer = new WrongAnswer
            {
                QuestionId = card.Id,
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

            var nextCardTime = deck.Cards.OrderBy(c => c.DueTime).FirstOrDefault().DueTime;
            return verbalMessages.ThereIsNoNextQuestion + "\n" + verbalMessages.ComeBackAtThisTime + ": " + nextCardTime.ToLocalTime() + " (" + verbalMessages.In + " " + (nextCardTime - answerTime) + ")" + "\n";
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
            if (deck.LastWrongAnswer.QuestionId == Guid.Empty)
                return verbalMessages.NothingToRevert;

            var lastWrongAnswer = deck.LastWrongAnswer;
            var card = deck.Cards.First(e => e.Id == lastWrongAnswer.QuestionId);

            card.CurrentInterval = lastWrongAnswer.PreviousLapse;
            card.DecreaseClue();

            var ret = verbalMessages.RevertLast;

            if (add)
                ret = AddAnswer(lastWrongAnswer, card);

            deck.LastWrongAnswer = WrongAnswer.NoWrongAnswer;

            return ret;
        }

        static string AddAnswer(WrongAnswer lastWrongAnswer, Card card)
        {
            string ret;
            card.Answers.Add(lastWrongAnswer.Answer);
            ret = verbalMessages.RevertAddLast;
            return ret;
        }
   }
}
