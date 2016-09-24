using System;
using System.Collections.Generic;
using System.Threading;

namespace UserInterface
{
    class MainClass
    {
        public static int Main(string[] args)
        {
            if (args.Length == 0)
                return Usage();

            switch (args[0]) {
                case "add":
                    return Add();
                case "play":
                    return Play();
                default:
                    return Usage();
            }
        }

        private static int Usage() {
            Console.Error.WriteLine("Usage: vanki.exe [add|play]");
            return 1;
        }

        private static int Add()
        {
            string question;

            while (!string.IsNullOrWhiteSpace(question = PromptQuestion())) {
                string answer;
                var answers = new List<string>();

                while (!string.IsNullOrWhiteSpace(answer = PromptAnswerEntry())) {
                    answers.Add(answer);
                }

                Vanki.MainClass.Main(new []{"-q", question, "-a", string.Join("~|~", answers)});

                Console.WriteLine(new String('-', 25));
            }

            return 0;
        }

        private static int Play()
        {
            string answer = string.Empty;
            do {
                Console.Clear();
                var ret = Vanki.MainClass.Main(new []{"-n"});

                if (ret == 7) {
                    Thread.Sleep(1000);
                    continue;
                }

                answer = PromptAnswer();
                Vanki.MainClass.Main(new []{"-a", answer});
            } while (answer != null);

            return 0;
        }

        private static string PromptQuestion()
        {
            Console.Write("Type the question: ");
            return Console.ReadLine();
        }

        private static string PromptAnswerEntry()
        {
            Console.Write("Type the answer (empty if no more): ");
            return Console.ReadLine();
        }

        private static string PromptAnswer()
        {
            Console.Write("Answer: ");
            return Console.ReadLine();
        }
    }
}
