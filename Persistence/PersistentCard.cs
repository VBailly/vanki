using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Storage;

namespace Persistence
{
    public class PersistentCard
    {
        IDictionary<string, Type> properties_;
        IDictionary<string, Tuple<Type, int>> legacyProperties_;

        string idFieldName_;


        public PersistentCard(string id)
        {
            Id = id;

            properties_ = new Dictionary<string, Type>();
            legacyProperties_ = new Dictionary<string, Tuple<Type, int>>();

        }

        public PersistentCard()
        {
            Id = Guid.NewGuid().ToString();
            properties_ = new Dictionary<string, Type>();
            legacyProperties_ = new Dictionary<string, Tuple<Type, int>>();

            var xCard = new XElement("Card");
            xCard.Add(new XAttribute("version", "4"));
            xCard.Add(new XElement("id", Id));


            var deck = GetDeck();
            deck.Add(xCard);
            Repository.StoreString(deck.ToString());
        }

        public void RegisterIdFieldName(string idFieldName)
        {
            idFieldName_ = idFieldName;
        }

        public void RegisterProperty(string propertyName, Type propertyType)
        {
            properties_.Add(propertyName, propertyType);
        }

        public void RegisterLegacyProperty(string propertyName, Type propertyType, int untilVersion)
        {
            legacyProperties_.Add(propertyName, new Tuple<Type, int>(propertyType, untilVersion));
        }

        public void SetValue(string propertyName, object value)
        {
            var deck = GetDeck();
            var element = GetCard(deck).Element(propertyName);
            if (properties_[propertyName] == typeof(IList<string>))
            {
                if (element == null)
                {
                    element = new XElement(propertyName);
                    GetCard(deck).Add(element);
                }
                else
                    element.RemoveAll();
                foreach (var str in value as IList<string>)
                    element.Add(new XElement("alternative", str));
            }
            else {
                if (element == null)
                {
                    element = new XElement(propertyName);
                    GetCard(deck).Add(element);
                }
                element.Value = value.ToString();
            }

            if (propertyName == idFieldName_)
            {
                Id = (string)value;
                GetCard(deck).Element("id").Remove();
            }
                


            Repository.StoreString(deck.ToString());
        }

        internal string Id { get; set; }

        public object GetValue(string propertyName)
        {
            var deck = GetDeck();
            var result = GetCard(deck).Element(propertyName);


            if (legacyProperties_.ContainsKey(propertyName) && legacyProperties_[propertyName].Item2 > GetVersion())
            {
                if (legacyProperties_[propertyName].Item1 == typeof(string) && properties_[propertyName] == typeof(IList<string>))
                    return new List<string> { result?.Value };
            }


            if (properties_[propertyName] == typeof(int))
            {
                try { return int.Parse(result?.Value); }
                catch (Exception) { return 0; }
            }
            if (properties_[propertyName] == typeof(DateTime))
                return DateTime.Parse(result?.Value);
            if (properties_[propertyName] == typeof(IList<string>))
            {
                return result.Elements("alternative").Select(e => e.Value).ToList();
            }
            return result?.Value;
        }

        public int GetVersion()
        {
            var deck = GetDeck();
            return int.Parse(GetCard(deck).Attribute("version").Value);
        }

        public void SetVersion(int version)
        {
            var deck = GetDeck();
            GetCard(deck).Attribute("version").Value = version.ToString();
            Repository.StoreString(deck.ToString());
        }

        XElement GetCard(XElement deck)
        {
            return deck.Elements("Card").Single(c => (c.Element(idFieldName_) ?? c.Element("id")).Value == Id);
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

