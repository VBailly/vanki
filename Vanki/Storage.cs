using System;
using System.Xml.Linq;
using System.IO;

namespace Vanki
{
	public static class Storage
	{
		const string DataBaseFileName = "db.xml";

		public static DateTime LastAnswerTime
		{
			get { return DateTime.Parse(GetValue("time")); }
			set { SetValue("time", value); }
		}

		public static string Question
		{
			get { return GetValue("question"); }
			set { SetValue("question", value); }
		}

		public static int CurrentInterval
		{
			get { return int.Parse(GetValue("lapse")); }
			set { SetValue("lapse", value); }
		}

		public static string Answer
		{
			get { return GetValue("answer"); }
			set { SetValue("answer", value); }
		}


		public static bool DataExist()
		{
			return File.Exists(DataBaseFileName);
		}

		static string GetValue(string id)
		{
			if (!DataExist())
				return GetNewData().Element(id).Value;
			return XElement.Load (DataBaseFileName).Element(id).Value;
		}

		static void SetValue(string id, object value)
		{
			var data = GetCurrentData();
			data.Element(id).Value = value.ToString();
			File.WriteAllText(DataBaseFileName, data.ToString());
		}

		static XElement GetCurrentData()
		{
			if (!DataExist())
				return GetNewData();
			return XElement.Load(DataBaseFileName);
		}

		static XElement GetNewData()
		{
			var time = DateTime.Now;
			var xTime = new XElement ("time", time.ToString ());
			var xLapse = new XElement ("lapse", "0");
			var xQuestion = new XElement("question", "");
			var xAnswer = new XElement("answer", "");

			var data = new XElement ("Data");
			data.Add (xTime);
			data.Add (xLapse);
			data.Add (xQuestion);
			data.Add (xAnswer);


			return data;
		}

	}
}

