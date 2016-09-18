using System;
using System.Collections.Generic;

public abstract class Card
{
    public abstract string Question { get; }
    public abstract IList<string> Answers { get; }
    public abstract bool CaseSensitiveAnswers { get; }
    public abstract int Clue { get; }

    public abstract DateTime DueTime { get; }

    public abstract void Promote();
    public abstract void Reset();
    public abstract void ResetLapse();
    public abstract void IncreaseClue();
}

