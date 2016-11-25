using System;
using DeckAPI;


namespace Vanki
{

    public static class StateFactory
    {
        public static State CreateState(IDeck deck, DateTime now, IVerbalMessages verbalMessages)
        {
            var deckState = deck.GetState(now);

            if (deckState == DeckState.Empty)
                return new EmptyState(deck, now, verbalMessages);
            
            if (deckState == DeckState.PendingCard)
                return new WithAPendingCardState(deck, now, verbalMessages);
                    
            return new WithoutPendingCardState(deck, now, verbalMessages);
        }
    }
    
}
