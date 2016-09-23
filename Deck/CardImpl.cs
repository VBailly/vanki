using System;
using System.Collections.Generic;
using Persistence;

public class CardImpl : Card
{
    PersistentCard model_;

    public CardImpl(string question)
    {
        model_ = new PersistentCard(question);
        RegisterProperties();

    }

    void RegisterProperties()
    {
        model_.RegisterProperty("time", typeof(DateTime));
        model_.RegisterProperty("lapse", typeof(int));
        model_.RegisterProperty("question", typeof(string));
        model_.RegisterProperty("answer", typeof(IList<string>));
        model_.RegisterLegacyProperty("answer", typeof(string), 4);
        model_.RegisterProperty("clue", typeof(int));
        model_.RegisterProperty("caseSensitive", typeof(bool));
        model_.RegisterIdFieldName("question");
    }

    public override int Clue => (int)model_.GetValue("clue");

    public override IList<string> Answers => (IList<string>)model_.GetValue("answer");

    public override string Question => (string)model_.GetValue("question");

    public override DateTime DueTime
    {
        get { return LastAnswerTime + TimeSpan.FromMinutes(CurrentInterval); }
    }

    DateTime LastAnswerTime
    {
        get
        {
            var val = model_.GetValue("time");
            var time = (DateTime)val;
            if (model_.GetVersion() == 1)
                return time;
            return time;
        }
        set
        {
            model_.SetValue("time", value);
        }
    }

    int CurrentInterval
    {
        get { return (int)model_.GetValue("lapse"); }
        set { model_.SetValue("lapse", value); }
    }

    public override bool CaseSensitiveAnswers
    {
        get
        {
            return (bool)model_.GetValue("caseSensitive");
        }
    }

    public override void Promote(DateTime answerTime)
    {
        if (CurrentInterval == 0)
            CurrentInterval = 2;
        else
            CurrentInterval = Math.Max(2, (int)(1.6 * (answerTime - LastAnswerTime).TotalMinutes));
        LastAnswerTime = answerTime;
        DecreaseClue();
    }

    public override void Reset(DateTime answerTime)
    {
        CurrentInterval = 0;
        LastAnswerTime = answerTime;
    }


    public override void IncreaseClue()
    {
        model_.SetValue("clue", Clue + 1);
    }

    private void DecreaseClue()
    {
        model_.SetValue("clue", Math.Max(0, Clue - 1));
    }

    public override void ResetLapse()
    {
        CurrentInterval = 0;
    }
}
