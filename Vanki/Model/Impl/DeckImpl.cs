using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System;

namespace Vanki.Model.Impl
{
	public class DeckImpl : Deck
	{
		const string DataBaseFileName = "db.xml";

		public override IEnumerable<Card> Cards
		{
			get
			{
				if (!File.Exists(DataBaseFileName))
				{
					return new List<Card>();
				}
				return XElement.Load(DataBaseFileName).Elements("Card").Select(e => new CardImpl(e));
			}
		}

		public override void CreateCard(string question, string answer, DateTime time)
		{
#pragma warning disable RECS0026 // Possible unassigned object created by 'new'
			new CardImpl(question, answer, time);
#pragma warning restore RECS0026 // Possible unassigned object created by 'new'
		}
	}
}

