using System;
using System.Collections.Generic;
using System.Linq;

public class Deck
{
    public IList<ICard> Cards { get; set; } = new List<ICard>();
    public LastAnswer LastAnswer { get; set; } = LastAnswer.NullAnswer;

    public ICard GetNextCardAfter(DateTime time)
    {
        return Cards.Where(c => c.DueTime <= time).OrderBy(c => c.DueTime).FirstOrDefault();
    }

    public void AddNewCard(IEnumerable<string> questions, IEnumerable<string> answers, bool caseSensitive, DateTime now)
    {
        Cards.Add(new Card(questions, answers, caseSensitive, now));
    }
}
