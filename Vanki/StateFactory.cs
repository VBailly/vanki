using System;

namespace Vanki
{

    public static class StateFactory
    {
        public static State CreateState(IDeck deck, DateTime now, IVerbalMessages verbalMessages)
        {
            return new MyOnlyState(deck, now, verbalMessages);
        }
    }
    
}
