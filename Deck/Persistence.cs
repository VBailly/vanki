using System;
using System.Linq;
using System.Xml.Linq;
using DeckAPI;


public static class Persistence {

    public static void Save(IDeck theDeck) {
        var deck = (Deck)theDeck;
        var deckXml = new XElement("Deck");
        deckXml.Add(new XAttribute("version", "2"));

        foreach (Card card in deck.Cards)
        {
            var cardXml = new XElement("Card");

            cardXml.Add(new XAttribute("version", 6));
            cardXml.Add(new XElement("id", card.Id));
            cardXml.Add(new XElement("time", card.LastAnswerTime.ToString("o")));
            cardXml.Add(new XElement("questions", card.Questions.Select(question =>
            {
                return new XElement("alternative", question);
            })));
            cardXml.Add(new XElement("answer", card.Answers.Select(answer => {
                return new XElement("alternative", answer);
            })));
            cardXml.Add(new XElement("lapse", card.CurrentInterval));
            cardXml.Add(new XElement("caseSensitive", card.CaseSensitiveAnswers));
            cardXml.Add(new XElement("clue", card.Clue));
            deckXml.Add(cardXml);
        }

        if (deck.LastAnswer != LastAnswer.NullAnswer)
        {

            deckXml.Add(new XElement("lastWrongAnswer", new[] {
            new XElement("version", 1),
            new XElement("answer", deck.LastAnswer.Answer),
            new XElement("previousLapse", deck.LastAnswer.PreviousLapse)
            }));
        }

        Storage.Repository.StoreString(deckXml.ToString());
    }

    private static Card LoadCardV6(XElement xml)
    {
        return new Card {
            Id = Guid.Parse(xml.Element("id").Value),
            LastAnswerTime = DateTime.Parse(xml.Element("time").Value).ToUniversalTime(),
            Questions = xml.Element("questions").Elements("alternative").Select(e => e.Value).ToList(),
            Answers = xml.Element("answer").Elements("alternative").Select(e => e.Value).ToList(),
            CurrentInterval = int.Parse(xml.Element("lapse").Value),
            CaseSensitiveAnswers = bool.Parse(xml.Element("caseSensitive").Value),
            Clue = int.Parse(xml.Element("clue").Value)
        };
    }

    private static LastAnswer LoadWrongAnswerV1(XElement xml)
    {
        if (xml == null)
            return LastAnswer.NullAnswer;
        
        return new LastAnswer {
            Answer = xml.Element("answer").Value,
            PreviousLapse = int.Parse(xml.Element("previousLapse").Value)
        };
    }

    private static IDeck LoadDeckV1(XElement xml)
    {
        var deck = new Deck();

        foreach (var cardXml in xml.Elements("Card")) {
            Card card;

            switch (cardXml.Attribute("version").Value) {
                case "6":
                    card = LoadCardV6(cardXml);
                    break;
                default:
                    throw new ArgumentException("Unknown card version");
            }

            deck.Cards.Add(card);
        }

        return deck;
    }

    private static IDeck LoadDeckV2(XElement xml)
    {
        var deck = new Deck();

        foreach (var cardXml in xml.Elements("Card")) {
            Card card;

            switch (cardXml.Attribute("version").Value) {
                case "6":
                    card = LoadCardV6(cardXml);
                    break;
                default:
                    throw new ArgumentException("Unknown card version");
            }

            deck.Cards.Add(card);
        }

        deck.LastAnswer = LoadWrongAnswerV1(xml.Element("lastWrongAnswer"));

        return deck;
    }

    public static IDeck Load() {
        var content = Storage.Repository.LoadString();
        if (string.IsNullOrEmpty(content))
            return new Deck();

        if (string.IsNullOrWhiteSpace(content))
            return new Deck();

        var xml = XElement.Parse(content);

        switch (xml.Attribute("version").Value) {
            case "1":
                return LoadDeckV1(xml);
            case "2":
                return LoadDeckV2(xml);
            default:
                throw new ArgumentException($"Unknown deck version {xml.Attribute("version").Value}");
        }
    }
}
