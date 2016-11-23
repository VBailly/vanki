using System.Collections.Generic;

public class Deck
{
    public IList<Card> Cards { get; set; } = new List<Card>();
    public LastAnswer LastAnswer { get; set; } = LastAnswer.NullAnswer;
}
