using System.Collections.Generic;

namespace RandomAPI
{
    public abstract class Random
    {
        public abstract T PickRandomly<T>(IEnumerable<T> elements);

        public static Random Instance { get; set; }
    }
}
