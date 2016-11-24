using System;

namespace Vanki
{

    public static class StateFactory
    {
        public static State CreateState(IDeck deck, DateTime now, IVerbalMessages verbalMessages)
        {
            if (deck.GetState(now) == DeckState.Empty)
                return new EmptyState(deck, now, verbalMessages);
                    
            return new MyOnlyState(deck, now, verbalMessages);
        }
    }
    
}
