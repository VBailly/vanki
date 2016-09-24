using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

public static class Persistence {

    public static void Save(Deck deck, string path) {
        var deckXml = new XElement("Deck");
        deckXml.Add(new XAttribute("version", "1"));

        foreach (Card card in deck.Cards) {
            var cardXml = new XElement("Card");

            cardXml.Add(new XAttribute("version", 5));
            cardXml.Add(new XElement("id", card.Id));
            cardXml.Add(new XElement("time", card.LastAnswerTime.ToString("o")));
            cardXml.Add(new XElement("question", card.Question));
            cardXml.Add(new XElement("answer", card.Answers.Select(answer => {
                return new XElement("alternative", answer);
            })));
            cardXml.Add(new XElement("lapse", card.CurrentInterval));
            cardXml.Add(new XElement("caseSensitive", card.CaseSensitiveAnswers));
            cardXml.Add(new XElement("clue", card.Clue));
            deckXml.Add(cardXml);
        }

        File.WriteAllText(path, deckXml.ToString(), Encoding.UTF8);
    }

    private static Card LoadCardV3(XElement xml)
    {
        return new Card {
            Id = Guid.NewGuid(),
            LastAnswerTime = DateTime.Parse(xml.Element("time").Value).ToUniversalTime(),
            Question = xml.Element("question").Value,
            Answers = new List<string> { xml.Element("answer").Value },
            CurrentInterval = int.Parse(xml.Element("lapse").Value)
        };
    }

    private static Card LoadCardV4(XElement xml)
    {
        return new Card {
            Id = Guid.NewGuid(),
            LastAnswerTime = DateTime.Parse(xml.Element("time").Value).ToUniversalTime(),
            Question = xml.Element("question").Value,
            Answers = xml.Element("answer").Elements("alternative").Select(e => e.Value).ToList(),
            CurrentInterval = int.Parse(xml.Element("lapse").Value),
            CaseSensitiveAnswers = bool.Parse(xml.Element("caseSensitive")?.Value ?? "false"),
            Clue = int.Parse(xml.Element("clue")?.Value ?? "0")
        };
    }

    private static Card LoadCardV5(XElement xml)
    {
        return new Card {
            Id = Guid.Parse(xml.Element("id").Value),
            LastAnswerTime = DateTime.Parse(xml.Element("time").Value).ToUniversalTime(),
            Question = xml.Element("question").Value,
            Answers = xml.Element("answer").Elements("alternative").Select(e => e.Value).ToList(),
            CurrentInterval = int.Parse(xml.Element("lapse").Value),
            CaseSensitiveAnswers = bool.Parse(xml.Element("caseSensitive").Value),
            Clue = int.Parse(xml.Element("clue").Value)
        };
    }

    private static Deck LoadDeckV1(XElement xml)
    {
        var deck = new Deck();

        foreach (var cardXml in xml.Elements("Card")) {
            Card card;

            switch (cardXml.Attribute("version").Value) {
                case "3":
                    card = LoadCardV3(cardXml);
                    break;
                case "4":
                    card = LoadCardV4(cardXml);
                    break;
                case "5":
                    card = LoadCardV5(cardXml);
                    break;
                default:
                    throw new ArgumentException("Unknown card version");
            }

            deck.Cards.Add(card);
        }

        return deck;
    }

    public static Deck Load(string path) {
        if (!File.Exists(path))
            return new Deck();

        var content = File.ReadAllText(path, Encoding.UTF8);

        if (string.IsNullOrWhiteSpace(content))
            return new Deck();

        var xml = XElement.Parse(content);

        switch (xml.Attribute("version").Value) {
            case "1":
                return LoadDeckV1(xml);
            default:
                throw new ArgumentException($"Unknown deck version {xml.Attribute("version").Value}");
        }
    }
}
