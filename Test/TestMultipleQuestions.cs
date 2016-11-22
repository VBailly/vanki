﻿using System;
using NUnit.Framework;

namespace Test
{
    [TestFixture]
    public class TestMultipleQuestions
    {
        [Test]
        public void We_can_register_multiple_version_of_a_question()
        {
            var time = DateTime.UtcNow;
            Commands.RegisterQuestion(time, new[] { "What is red?", "What is blue?" }, new[] { "a color" });

            var response = Commands.AskForNextQuestion(time);

            Assert.Contains(response, new[] { "What is red?", "What is blue?" }); 
        }
    }
}