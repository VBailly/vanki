using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System;
using Storage;

namespace Vanki.Model.Impl
{
	public class DeckImpl : Deck
	{
		const string DataBaseFileName = "db.xml";

		public override IEnumerable<Card> Cards
		{
			get
			{
                var xml = Repository.GetStoredString();
                if (string.IsNullOrEmpty(xml))
				{
					return new List<Card>();
				}

                return XElement.Parse(xml).Elements("Card").Select(e => new CardImpl(e));
			}
		}

		public override void CreateCard(string question, string answer)
		{
#pragma warning disable RECS0026 // Possible unassigned object created by 'new'
			new CardImpl(question, answer);
#pragma warning restore RECS0026 // Possible unassigned object created by 'new'
		}
	}
}

