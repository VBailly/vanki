using System;

namespace Vanki
{
	public static class Clock
	{
		public static Func<DateTime> Getter { get; set; }
		public static DateTime CurrentTime { get { return Getter == null ? DateTime.Now : Getter.Invoke(); } }
	}
}

