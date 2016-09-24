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

        File.WriteAllText(path, deckXml.ToString(), Encoding.UTF8);
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

    private static Deck LoadDeckV1(XElement xml)
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
