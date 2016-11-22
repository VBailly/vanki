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
            if (!string.IsNullOrEmpty(result)) {
                var ret = result.StartsWith(verbalMessages.ThereIsNoNextQuestion, StringComparison.CurrentCulture) ? 7 : 0;
                Console.Write (result + "\n");
                return ret;
            }
            return 0;
        }

        public static string TestableMain(string[] args, DateTime now)
        {
            var options = ArgsParser.Parse (args);
            var deck = Persistence.Load();

            string ret = string.Empty;

            if (options.ShowNext)
                ret = PrintNextQuestion(deck, now);
            else if (options.Questions.Any() && options.Answers.Any())
            {
                deck.Cards.Add(new Card() {
                    Id = Guid.NewGuid(),
                    Questions = options.Questions,
                    Answers = options.Answers,
                    CaseSensitiveAnswers = options.CaseSensitive,
                    Clue = 0,
                    LastAnswerTime = now,
                    CurrentInterval = 0
                });
            }
            else if (!(options.Answers == null || !options.Answers.Any()))
                ret = ProcessAnswer(deck, now, options.Answers[0]);
            else if (options.RevertLastWrongAnswer)
                ret = RevertLastWrongAnswer(deck, options.RevertLastWrongAnswerAdd);
            else
                ret = verbalMessages.WrongCmdArgs;

            Persistence.Save(deck);

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
                return verbalMessages.NothingToAnswer;

            string correctAnswer;

            if (card.CaseSensitiveAnswers)
                correctAnswer = card.Answers.FirstOrDefault(a => a == answer);
            else
                correctAnswer = card.Answers.FirstOrDefault(a => a.ToLower() == answer.ToLower());

            if (correctAnswer == null)
            {
                deck.LastWrongAnswer = new WrongAnswer {
                    QuestionId = card.Id,
                    Answer = answer,
                    PreviousLapse = card.CurrentInterval
                };
                card.Reset();
                return string.Empty;
            }

            deck.LastWrongAnswer = new WrongAnswer();

            card.Promote(answerTime);

            return string.Empty;
        }

        static string PrintNextQuestion (Deck deck, DateTime answerTime)
        {
            if (!deck.Cards.Any())
                return verbalMessages.TheDeckIsEmpty;
            var card = GetNextCard(deck, answerTime);
            if (card != null)
            {
                if (card.Clue == 0)
                    return card.Questions.OrderBy(x => Guid.NewGuid()).First();
                else
                    return card.Questions.OrderBy(x => Guid.NewGuid()).First() + "\n" + verbalMessages.Clue + ": " + GetHint(card.Answers[0], card.Clue);
            }

            var nextCardTime = deck.Cards.OrderBy(c => c.DueTime).FirstOrDefault().DueTime;
            return verbalMessages.ThereIsNoNextQuestion + "\n" + verbalMessages.ComeBackAtThisTime + ": " + nextCardTime.ToLocalTime() + " (" + verbalMessages.In + " " + (nextCardTime - answerTime) + ")" + "\n";
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

            if (add) {
                card.Answers.Add(lastWrongAnswer.Answer);
                ret = verbalMessages.RevertAddLast;
            }

            deck.LastWrongAnswer = new WrongAnswer();

            return ret;
        }
    }
}
