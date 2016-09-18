using System;
using System.Collections.Generic;
using System.Linq;
using Persistence;

public static class Deck
{
    public static void AddQuestion(string question, IList<string> answers)
    {
        var model = new PersistentCard();
        model.RegisterProperty("time", typeof(DateTime));
        model.RegisterProperty("lapse", typeof(int));
        model.RegisterProperty("question", typeof(string));
        model.RegisterProperty("answer", typeof(IList<string>));
        model.RegisterLegacyProperty("answer", typeof(string), 4);
        model.RegisterProperty("clue", typeof(int));
        model.RegisterIdFieldName("question");

        model.SetValue("time", Clock.CurrentGlobalTime);
        model.SetValue("lapse", 0);
        model.SetValue("question", question);
        model.SetValue("answer", answers);
        model.SetValue("clue", 0);
    }

    public static void AddQuestionCaseSensitive(string question, IList<string> answers)
    {
        var model = new PersistentCard();
        model.RegisterProperty("time", typeof(DateTime));
        model.RegisterProperty("lapse", typeof(int));
        model.RegisterProperty("question", typeof(string));
        model.RegisterProperty("answer", typeof(IList<string>));
        model.RegisterProperty("caseSensitive", typeof(bool));
        model.RegisterLegacyProperty("answer", typeof(string), 4);
        model.RegisterProperty("clue", typeof(int));
        model.RegisterIdFieldName("question");

        model.SetValue("time", Clock.CurrentGlobalTime);
        model.SetValue("lapse", 0);
        model.SetValue("question", question);
        model.SetValue("answer", answers);
        model.SetValue("caseSensitive", true);
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
