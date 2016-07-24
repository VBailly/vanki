﻿using System;
using Vanki;

namespace Test
{
	public static class Commands
	{

		public static string AnswerWrongly(DateTime time)
		{
			return MainClass.TestableMain(new[] { "--answer", "an animal" }, time);
		}

		public static string AnswerCorrectly(DateTime time)
		{
			return MainClass.TestableMain(new[] { "--answer", "a color" }, time);
		}

		public static string AskForNextQuestion(DateTime time)
		{
			return MainClass.TestableMain(new[] { "--next" }, time);
		}

		public static string RegisterQuestion(string question, string answer)
		{
			return MainClass.TestableMain(new[] { "-q", question, "-a", answer }, DateTime.Now);
		}
	}
}

