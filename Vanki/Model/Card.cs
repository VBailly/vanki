using System;

namespace Vanki.Model
{
	public abstract class Card
	{
		public abstract string Question { get; }
		public abstract string Answer { get; }
        public abstract int Clue { get; }

		public abstract DateTime DueTime { get; }

		public abstract void Promote();
		public abstract void Reset();
        public abstract void IncreaseClue();
	}
}

