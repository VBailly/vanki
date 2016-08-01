using System;
using System.Linq;
using System.Xml.Linq;
using Storage;

namespace Vanki.Model.Impl
{
	public class CardImpl : Card
	{

		string question_;


		public CardImpl(string question, string answer)
		{
			question_ = question;

			var xCard = createXCard(question, answer);
			var deck = GetDeck();
			deck.Add(xCard);
            Repository.StoreString(deck.ToString());
		}

		public CardImpl(XElement xCard)
		{
			question_ = xCard.Element("question").Value;
		}

		XElement createXCard(string question, string answer)
		{
            var xTime = new XElement("time", Clock.CurrentTime.ToUniversalTime().ToString());
			var xLapse = new XElement("lapse", "0");
			var xQuestion = new XElement("question", question);
			var xAnswer = new XElement("answer", answer);
            var xClue = new XElement("clue", "0");

			var xCard = new XElement("Card");
			xCard.Add(new XAttribute("version", "3"));
			xCard.Add(xTime);
			xCard.Add(xLapse);
			xCard.Add(xQuestion);
			xCard.Add(xAnswer);
            xCard.Add(xClue);

			return xCard;
		}

        public override int Clue
        {
            get
            {
                var clue = GetValue("clue");
                return clue == null?  0 : int.Parse(clue);
            }
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
            get
            {
                if (GetVersion() == "1")
                    return DateTime.Parse(GetValue("time"));
                return DateTime.Parse(GetValue("time")).ToLocalTime(); 
            }
			set
            {
                SetVersion("3");
                SetValue("time", value.ToUniversalTime()); 
            }
		}

		int CurrentInterval
		{
			get { return int.Parse(GetValue("lapse")); }
			set { SetValue("lapse", value); }
		}

        public override void Promote()
		{
			var time = Clock.CurrentTime;
            if (CurrentInterval == 0)
                CurrentInterval = 2;
            else
			    CurrentInterval = Math.Max(2, (int)(1.6 * (time - LastAnswerTime).TotalMinutes));
			LastAnswerTime = time;
            DecreaseClue();
		}

		public override void Reset()
		{
			CurrentInterval = 0;
			LastAnswerTime = Clock.CurrentTime;
		}

        XElement GetCard(XElement deck)
        {
            return deck.Elements("Card").Single(c => c.Element("question").Value == question_);
        }

		string GetValue(string id)
		{
            var deck = GetDeck();
            return GetCard(deck).Element(id)?.Value;
		}

        string GetVersion()
        {
            var deck = GetDeck();
            return GetCard(deck).Attribute("version").Value;
        }

        void SetVersion(string version)
        {
            var deck = GetDeck();
            GetCard(deck).Attribute("version").Value = version;
            Repository.StoreString(deck.ToString());
        }


        void SetValue(string id, object value)
		{
			var deck = GetDeck();
            var element = GetCard(deck).Element(id);
            if (element == null)
            {
                element = new XElement(id);
                GetCard(deck).Add(element);
            }
            element.Value = value.ToString();
			Repository.StoreString(deck.ToString());
		}

		static XElement GetDeck()
		{
            var db = Repository.GetStoredString();

            if (string.IsNullOrEmpty(db))
			{
				var deck = new XElement("Deck");
				deck.Add(new XAttribute("version", "1"));
				return deck;
			}
            return XElement.Parse(db);
		}

        public override void IncreaseClue()
        {
            SetVersion("3");
            SetValue("clue", Clue + 1);
        }

        private void DecreaseClue()
        {
            SetVersion("3");
            SetValue("clue", Math.Max(0, Clue - 1));
        }

        public override void ResetLapse()
        {
            CurrentInterval = 0;
        }
    }
}

