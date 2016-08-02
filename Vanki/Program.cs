using System;
using Vanki.Model;
using Vanki.Model.Impl;
using System.Linq;


namespace Vanki
{
	public class MainClass
	{
		const string thereIsNoNextQuestion = "There is no next question";
		const string cannotAnswer = "You cannot answer because there is no question pending";
		const string emptyDeckMessage = "There is no questions, the deck is empty";
		static readonly Deck deck = new DeckImpl();

		public static void Main (string[] args)
		{
			var result = TestableMain (args);
            if (!string.IsNullOrEmpty(result))
			    Console.Write (result + "\n");
		}

		public static string TestableMain(string[] args)
		{
			var options = ArgsParser.Parse (args);

            if (options.ShowNext)
                return PrintNextQuestion();
            if (!string.IsNullOrEmpty(options.Question) && !string.IsNullOrEmpty(options.Answer))
                return AddQuestion(options.Question, options.Answer);
            if (!string.IsNullOrEmpty(options.Answer))
                return ProcessAnswer(options.Answer);
            if (options.Clue)
                return GetAClue();
            
            return "wrong command line arguments";
		}

        static string GetAClue()
        {
            var card = GetNextCard();
            if (card == null)
                return string.Empty;
            card.ResetLapse();
            card.IncreaseClue();
            return GetHint(card.Answer, card.Clue);
        }

        static string AddQuestion(string question, string answer)
		{
			deck.CreateCard(question, answer);
            return string.Empty;
		}
			

		static Card GetNextCard()
		{ 
            return deck.Cards.Where(c => c.DueTime <= Clock.CurrentLocalTime).OrderBy(c => c.DueTime).FirstOrDefault();
		}

        static string GetHint(string answer, int size)
        {
            var answers = answer.Split(',').Select(s => s.Trim());
            return string.Join(", ", answers.Select(w => string.Join(".", w.Split(' ').Select(s => new string(s.Take(size).ToArray())))));
        }

		static string ProcessAnswer (string answer)
		{
			var card = GetNextCard();
			if (card == null)
				return cannotAnswer;

			var correctAnswer = card.Answer;

			if (answer != correctAnswer)
			{
				card.Reset();
				return correctAnswer;
			}

			card.Promote();

            return string.Empty;
		}

		static string PrintNextQuestion ()
		{
			if (!deck.Cards.Any())
				return emptyDeckMessage;
			var card = GetNextCard();
            if (card != null)
            {
                if (card.Clue == 0)
                    return card.Question;
                else
                    return card.Question + "\nclue: " + GetHint(card.Answer, card.Clue);
            }
				
			var nextCardTime = deck.Cards.OrderBy(c => c.DueTime).FirstOrDefault().DueTime;
            return thereIsNoNextQuestion + string.Format("\nCome back at this time: {0} (in {1})\n", nextCardTime, nextCardTime - Clock.CurrentLocalTime);
		}


	}
}
