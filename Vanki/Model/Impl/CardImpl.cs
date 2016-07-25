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


		public CardImpl(string question, string answer)
		{
			question_ = question;

			var xCard = createXCard(question, answer);
			var deck = GetDeck();
			deck.Add(xCard);
			File.WriteAllText(DataBaseFileName, deck.ToString());
		}

		public CardImpl(XElement xCard)
		{
			question_ = xCard.Element("question").Value;
		}

		XElement createXCard(string question, string answer)
		{
			var xTime = new XElement("time", Clock.CurrentTime.ToString());
			var xLapse = new XElement("lapse", "0");
			var xQuestion = new XElement("question", question);
			var xAnswer = new XElement("answer", answer);

			var xCard = new XElement("Card");
			xCard.Add(new XAttribute("version", "1"));
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

		public override void Promote()
		{
			var time = Clock.CurrentTime;
			CurrentInterval = Math.Max(2, (int)(1.6 * (time - LastAnswerTime).TotalMinutes));
			LastAnswerTime = time;
		}

		public override void Reset()
		{
			CurrentInterval = 0;
			LastAnswerTime = Clock.CurrentTime;
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
				var deck = new XElement("Deck");
				deck.Add(new XAttribute("version", "1"));
				return deck;
			}
            return XElement.Parse(File.ReadAllText(DataBaseFileName));
		}


	}
}

