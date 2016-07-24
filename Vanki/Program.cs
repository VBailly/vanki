using System;
using Vanki.Model;
using Vanki.Model.Impl;
using System.Linq;


namespace Vanki
{
	public class MainClass
	{
		const string newEntryRegistered = "New entry registered\n";
		const string thatIsACorrectAnswer = "That is a correct answer!\n";
		static readonly string theNextQuestionIs = "The next question is:\n\"{0}\"\n";
		const string thereIsNoNextQuestion = "There is no next question\n";
		const string cannotAnswer = "You cannot answer because there is no question pending\n";
		const string emptyDeckMessage = "There is no questions, the deck is empty\n";
		static readonly Deck deck = new DeckImpl();

		public static void Main (string[] args)
		{
			var result = TestableMain (args, DateTime.Now);
			Console.Write (result);
		}

		public static string TestableMain(string[] args, DateTime time)
		{
			var options = ArgsParser.Parse (args);
			if (options.ShowNext)
				return PrintNextQuestion (time);
			if (!string.IsNullOrEmpty (options.Question) && !string.IsNullOrEmpty (options.Answer))
				return AddQuestion (time, options.Question, options.Answer);
			if (!string.IsNullOrEmpty (options.Answer))
				return ProcessAnswer (time, options.Answer);


			return "wrong command line arguments\n";
		}

		static string AddQuestion(DateTime time, string question, string answer)
		{
			deck.CreateCard(question, answer, time);
			return newEntryRegistered;
		}
			

		static Card GetNextCard(DateTime time)
		{ 
			return deck.Cards.Where(c => c.DueTime <= time).OrderBy(c => c.DueTime).FirstOrDefault();
		}

		static string ProcessAnswer (DateTime time, string answer)
		{
			var card = GetNextCard(time);
			if (card == null)
				return cannotAnswer;

			var correctAnswer = card.Answer;

			if (answer != correctAnswer)
			{
				card.Reset(time);
				return string.Format("WRONG! The correct answer is \"{0}\".\n", correctAnswer);
			}

			card.Promote(time);

			return thatIsACorrectAnswer;
		}

		static string PrintNextQuestion (DateTime time)
		{
			if (!deck.Cards.Any())
				return emptyDeckMessage;
			var card = GetNextCard(time);
			if (card != null)
				return string.Format(theNextQuestionIs, card.Question);
			var nextCardTime = deck.Cards.OrderBy(c => c.DueTime).FirstOrDefault().DueTime;
			return thereIsNoNextQuestion + string.Format("Come back at this time: {0} (in {1})\n", nextCardTime, nextCardTime - time);
		}


	}
}
