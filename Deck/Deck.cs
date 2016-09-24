using System.Collections.Generic;

public class Deck
{
    public IList<Card> Cards { get; set; } = new List<Card>();
    public WrongAnswer LastWrongAnswer { get; set; } = new WrongAnswer();
}
