using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Storage;

namespace Persistence
{
    public static class PersistentDeck
    {

        public static IEnumerable<string> Cards
        {
            get
            {
                var xml = Repository.GetStoredString();
                if (string.IsNullOrEmpty(xml))
                {
                    return new List<string>();
                }


                return XElement.Parse(xml).Elements("Card").Select(e => e.Element("question").Value);
            }
        }
    }
}

