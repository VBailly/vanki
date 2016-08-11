using System;
using Persistence;

namespace Vanki.Model.Impl
{
	public class CardImpl : Card
	{

		PersistentCard model_;


		public CardImpl(string question, string answer)
        {
            model_ = new PersistentCard();
            RegisterProperties();
            InitializeValues(question, answer);
        }

        public CardImpl(string question)
        {
            model_ = new PersistentCard(question);
            RegisterProperties();

        }

        void InitializeValues(string question, string answer)
        {
            model_.SetValue("time", Clock.CurrentGlobalTime);
            model_.SetValue("lapse", 0);
            model_.SetValue("question", question);
            model_.SetValue("answer", answer);
            model_.SetValue("clue", 0);
        }

        void RegisterProperties()
        {
            model_.RegisterProperty("time", typeof(DateTime));
            model_.RegisterProperty("lapse", typeof(int));
            model_.RegisterProperty("question", typeof(string));
            model_.RegisterProperty("answer", typeof(string));
            model_.RegisterProperty("clue", typeof(int));
        }

        public override int Clue => (int)model_.GetValue("clue");

		public override string Answer => (string)model_.GetValue("answer");

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
                if (model_.GetVersion() == "1")
                    return time;
                return Clock.ToLocalTime(time); 
            }
			set
            {
                model_.SetValue("time", value.ToUniversalTime()); 
            }
		}

		int CurrentInterval
		{
            get { return (int)model_.GetValue("lapse"); }
			set { model_.SetValue("lapse", value); }
		}

        public override void Promote()
		{
            var time = Clock.CurrentLocalTime;
            if (CurrentInterval == 0)
                CurrentInterval = 2;
            else
			    CurrentInterval = Math.Max(2, (int)(1.6 * (time - LastAnswerTime).TotalMinutes));
			LastAnswerTime = time;
            DecreaseClue();
		}

		public override void Reset()
		{
			CurrentInterval = 0;
            LastAnswerTime = Clock.CurrentLocalTime;
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
}

