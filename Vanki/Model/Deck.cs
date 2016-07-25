using System;
using System.Collections.Generic;

namespace Vanki.Model
{
	public abstract class Deck
	{
		public abstract IEnumerable<Card> Cards { get; }
		public abstract void CreateCard(string question, string answer);
	}


}

