using System;
namespace DeckAPI
{

    public abstract class DisposableDeckFactory
    {
        public abstract IDisposableDeck GetDeck();

        public static DisposableDeckFactory Instance { get; set; }
    }
}
