using System.Collections.Generic;
using System.Linq;
using Persistence;

namespace Vanki.Model.Impl
{
	public class DeckImpl : Deck
	{
		public override IEnumerable<Card> Cards
		{
			get
			{
                return PersistentDeck.Cards.Select(e => new CardImpl(e));
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

