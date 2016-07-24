using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Vanki.Model.Impl
{
	public class CardImpl : Card
	{
		const string DataBaseFileName = "db.xml";

		string question_;


		public CardImpl(string question, string answer, DateTime time)
		{
			question_ = question;

			var xCard = createXCard(question, answer, time);
			var deck = GetDeck();
			deck.Add(xCard);
			File.WriteAllText(DataBaseFileName, deck.ToString());
		}

		public CardImpl(XElement xCard)
		{
			question_ = xCard.Element("question").Value;
		}

		XElement createXCard(string question, string answer, DateTime time)
		{
			var xTime = new XElement("time", time.ToString());
			var xLapse = new XElement("lapse", "0");
			var xQuestion = new XElement("question", question);
			var xAnswer = new XElement("answer", answer);

			var xCard = new XElement("Card");
			xCard.Add(xTime);
			xCard.Add(xLapse);
			xCard.Add(xQuestion);
			xCard.Add(xAnswer);

			return xCard;
		}

		public override string Answer
		{
			get { return GetValue("answer"); }
		}

		public override DateTime DueTime
		{
			get { return LastAnswerTime + TimeSpan.FromMinutes(CurrentInterval); }
		}

		public override string Question
		{
			get { return GetValue("question"); }
		}

		DateTime LastAnswerTime
		{
			get { return DateTime.Parse(GetValue("time")); }
			set { SetValue("time", value); }
		}

		int CurrentInterval
		{
			get { return int.Parse(GetValue("lapse")); }
			set { SetValue("lapse", value); }
		}

		public override void Promote(DateTime time)
		{
			CurrentInterval = Math.Max(2, 2 * (time - LastAnswerTime).Minutes);
			LastAnswerTime = time;
		}

		public override void Reset(DateTime time)
		{
			CurrentInterval = 0;
			LastAnswerTime = time;
		}

		string GetValue(string id)
		{
			var deck = GetDeck();
			return deck.Elements("Card").Single(c => c.Element("question").Value == question_).Element(id).Value;
		}

		void SetValue(string id, object value)
		{
			var deck = GetDeck();
			deck.Elements("Card").Single(c => c.Element("question").Value == question_).Element(id).Value = value.ToString();
			File.WriteAllText(DataBaseFileName, deck.ToString());
		}

		static XElement GetDeck()
		{
			if (!File.Exists(DataBaseFileName))
			{
				return new XElement("Deck");
			}
			return XElement.Load(DataBaseFileName);
		}


	}
}

