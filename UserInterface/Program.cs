using System;
using System.Collections.Generic;
using System.Threading;
using UserInterfaceAPI;

public class InteractiveCommandLine : UserInterface
{
    public override void Launch(string[] args)
    {
        if (args.Length == 0)
            Usage();

        switch (args[0]) {
            case "add":
                Add();
                break;
            case "play":
                Play();
                break;
            default:
                Usage();
                break;
        }
    }

    private static void Usage() {
        Console.Error.WriteLine("Usage: vanki.exe [add|play]");
    }

    private static void Add()
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
    }

    private static void Play()
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

