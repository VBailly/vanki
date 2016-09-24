﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Vanki
{
    public class MainClass
    {
        const string thereIsNoNextQuestion = "There is no next question";
        const string cannotAnswer = "You cannot answer because there is no question pending";
        const string emptyDeckMessage = "There is no questions, the deck is empty";

        public static void Main (string[] args)
        {
            var result = TestableMain (args, DateTime.UtcNow);
            if (!string.IsNullOrEmpty(result))
                Console.Write (result + "\n");
        }

        public static string TestableMain(string[] args, DateTime now)
        {
            var options = ArgsParser.Parse (args);

            if (options.ShowNext)
                return PrintNextQuestion(now);
            if (!string.IsNullOrEmpty(options.Question) && !(options.Answers == null || options.Answers.Count() == 0))
            {
                if (options.CaseSensitive)
                    return AddQuestionCaseSensitive(options.Question, options.Answers, now);
                return AddQuestion(options.Question, options.Answers, now);
            }
            if (!(options.Answers == null || options.Answers.Count() == 0))
                return ProcessAnswer(now, options.Answers[0]);

            return "wrong command line arguments";
        }

        static string AddQuestionCaseSensitive(string question, IList<string> answers, DateTime time)
        {
            Deck.AddQuestionCaseSensitive(question, answers, time);
            return string.Empty;
        }

        static string AddQuestion(string question, IList<string> answers, DateTime time)
        {
            Deck.AddQuestion(question, answers, time);
            return string.Empty;
        }


        static Card GetNextCard(DateTime time)
        {
            return Deck.Cards.Where(c => c.DueTime <= time).OrderBy(c => c.DueTime).FirstOrDefault();
        }

        static string GetHint(string answer, int size)
        {
            var answers = answer.Split(',').Select(s => s.Trim());
            return string.Join(", ", answers.Select(w => string.Join(".", w.Split(' ').Select(s => new string(s.Take(size).ToArray())))));
        }

        static string ProcessAnswer (DateTime answerTime, string answer)
        {
            var card = GetNextCard(answerTime);
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

        static string PrintNextQuestion (DateTime answerTime)
        {
            if (!Deck.Cards.Any())
                return emptyDeckMessage;
            var card = GetNextCard(answerTime);
            if (card != null)
            {
                if (card.Clue == 0)
                    return card.Question;
                else
                    return card.Question + "\nclue: " + GetHint(card.Answers[0], card.Clue);
            }

            var nextCardTime = Deck.Cards.OrderBy(c => c.DueTime).FirstOrDefault().DueTime;
            return thereIsNoNextQuestion + string.Format("\nCome back at this time: {0} (in {1})\n", nextCardTime.ToLocalTime(), (nextCardTime - answerTime));
        }
    }
}
