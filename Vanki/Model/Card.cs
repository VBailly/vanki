﻿using System;

namespace Vanki.Model
{
	public abstract class Card
	{
		public abstract string Question { get; }
		public abstract string Answer { get; }

		public abstract DateTime DueTime { get; }

		public abstract void Promote(DateTime time);
		public abstract void Reset(DateTime time);
	}
}
