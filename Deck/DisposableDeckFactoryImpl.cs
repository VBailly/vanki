namespace DeckAPI
{
    public class DisposableDeckFactoryImpl : DisposableDeckFactory
    {
        public override IDisposableDeck GetDeck()
        {
            return new OnDiskDeck();
        }
    }
}
