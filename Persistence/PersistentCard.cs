using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Storage;

namespace Persistence
{
    public class PersistentCard
    {
        Dictionary<string, Type> properties_;

        public PersistentCard(string question)
        {
            Question = question;

            properties_ = new Dictionary<string, Type>();

        }

        public PersistentCard()
        {
            Question = Guid.NewGuid().ToString();
            properties_ = new Dictionary<string, Type>();

            var xCard = new XElement("Card");
            xCard.Add(new XAttribute("version", "3"));
            xCard.Add(new XElement("question", Question));


            var deck = GetDeck();
            deck.Add(xCard);
            Repository.StoreString(deck.ToString());
        }


        public void RegisterProperty(string propertyName, Type propertyType)
        {
            properties_.Add(propertyName, propertyType);
        }

        public void SetValue(string propertyName, object value)
        {
            var deck = GetDeck();
            var element = GetCard(deck).Element(propertyName);
            if (element == null)
            {
                element = new XElement(propertyName);
                GetCard(deck).Add(element);
            }
            element.Value = value.ToString();
            Repository.StoreString(deck.ToString());

            if (propertyName == "question")
                Question = (string)value;
        }

        internal string Question { get; set; }

        public object GetValue(string propertyName)
        {
            var deck = GetDeck();
            var result = GetCard(deck).Element(propertyName)?.Value;
            switch (properties_[propertyName].ToString())
            {
                case "System.Int32":
                    try
                    {
                        return int.Parse(result);
                    }
                    catch (Exception)
                    {
                        return 0;
                    }

                case "System.DateTime":
                    return DateTime.Parse(result);
                default:
                    return result;
            }
        }

        public string GetVersion()
        {
            var deck = GetDeck();
            return GetCard(deck).Attribute("version").Value;
        }

        public void SetVersion(string version)
        {
            var deck = GetDeck();
            GetCard(deck).Attribute("version").Value = version;
            Repository.StoreString(deck.ToString());
        }

        XElement GetCard(XElement deck)
        {
            return deck.Elements("Card").Single(c => c.Element("question").Value == Question);
        }

        static XElement GetDeck()
        {
            var db = Repository.GetStoredString();

            if (string.IsNullOrEmpty(db))
            {
                var deck = new XElement("Deck");
                deck.Add(new XAttribute("version", "1"));
                return deck;
            }
            return XElement.Parse(db);
        }
    }
}

