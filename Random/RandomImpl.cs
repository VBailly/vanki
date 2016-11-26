using System;
using System.Collections.Generic;
using System.Linq;

public class RandomImpl : RandomAPI.Random
{
    public override T PickRandomly<T>(IEnumerable<T> elements)
    {
        return elements.OrderBy(o => Guid.NewGuid()).FirstOrDefault();
    }
}
