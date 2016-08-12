using System;
using System.Collections.Generic;
using System.Linq;
using Persistence;

public static class Deck
{
    public static void AddQuestion(string question, string answer)
    {
        var model = new PersistentCard();
        model.RegisterProperty("time", typeof(DateTime));
        model.RegisterProperty("lapse", typeof(int));
        model.RegisterProperty("question", typeof(string));
        model.RegisterProperty("answer", typeof(string));
        model.RegisterProperty("clue", typeof(int));

        model.SetValue("time", Clock.CurrentGlobalTime);
        model.SetValue("lapse", 0);
        model.SetValue("question", question);
        model.SetValue("answer", answer);
        model.SetValue("clue", 0);
    }

    public static IEnumerable<Card> Cards
    {
        get
        {
            return PersistentDeck.Cards.Select(e => new CardImpl(e));
        }
    }
}
