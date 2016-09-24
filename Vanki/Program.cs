using System;
using System.Collections.Generic;
using System.Linq;

namespace Vanki
{
    public class MainClass
    {
        const string thereIsNoNextQuestion = "There is no next question";
        const string cannotAnswer = "You cannot answer because there is no question pending";
        const string emptyDeckMessage = "There is no questions, the deck is empty";

        public static int Main (string[] args)
        {
            var result = TestableMain (args, DateTime.UtcNow);
            if (!string.IsNullOrEmpty(result)) {
                var ret = result.StartsWith(thereIsNoNextQuestion) ? 7 : 0;
                Console.Write (result + "\n");
                return ret;
            }
            return 0;
        }

        public static string TestableMain(string[] args, DateTime now)
        {
            var options = ArgsParser.Parse (args);
            var deck = Persistence.Load("db.xml");

            string ret = string.Empty;

            if (options.ShowNext)
                ret = PrintNextQuestion(deck, now);
            else if (!string.IsNullOrEmpty(options.Question) && !(options.Answers == null || !options.Answers.Any()))
            {
                deck.Cards.Add(new Card() {
                    Id = Guid.NewGuid(),
                    Question = options.Question,
                    Answers = options.Answers,
                    CaseSensitiveAnswers = options.CaseSensitive,
                    Clue = 0,
                    LastAnswerTime = now,
                    CurrentInterval = 0
                });
            }
            else if (!(options.Answers == null || !options.Answers.Any()))
                ret = ProcessAnswer(deck, now, options.Answers[0]);
            else
                ret = "wrong command line arguments";

            Persistence.Save(deck, "db.xml");

            return ret;
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
                return cannotAnswer;

            string correctAnswer;

            if (card.CaseSensitiveAnswers)
                correctAnswer = card.Answers.FirstOrDefault(a => a == answer);
            else
                correctAnswer = card.Answers.FirstOrDefault(a => a.ToLower() == answer.ToLower());

            if (correctAnswer == null)
            {
                card.Reset(answerTime);
                return card.Answers.First();
            }

            card.Promote(answerTime);

            return string.Empty;
        }

        static string PrintNextQuestion (Deck deck, DateTime answerTime)
        {
            if (!deck.Cards.Any())
                return emptyDeckMessage;
            var card = GetNextCard(deck, answerTime);
            if (card != null)
            {
                if (card.Clue == 0)
                    return card.Question;
                else
                    return card.Question + "\nclue: " + GetHint(card.Answers[0], card.Clue);
            }

            var nextCardTime = deck.Cards.OrderBy(c => c.DueTime).FirstOrDefault().DueTime;
            return thereIsNoNextQuestion + string.Format("\nCome back at this time: {0} (in {1})\n", nextCardTime.ToLocalTime(), (nextCardTime - answerTime));
        }
    }
}
