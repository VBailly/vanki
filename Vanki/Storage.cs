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
			get
			{
				var data = GetData();
				return DateTime.Parse(data.Element ("time").Value);
			}
			set
			{
				var data = GetData();
				data.Element ("time").Value = value.ToString();
				File.WriteAllText (DataBaseFileName, data.ToString ());
			}
		}

		public static string Question
		{
			get
			{
				var data = GetData();
				return data.Element("question").Value;
			}
			set
			{
				var data = GetData();
				data.Element("question").Value = value;

				File.WriteAllText(DataBaseFileName, data.ToString());
			}
		}

		public static int CurrentInterval
		{
			get
			{
				var data = GetData();
				return int.Parse(data.Element("lapse").Value);
			}
			set
			{
				var data = GetData();
				data.Element("lapse").Value = value.ToString();

				File.WriteAllText(DataBaseFileName, data.ToString());
			}
		}

		public static string Answer
		{
			get
			{
				var data = GetData();
				return data.Element("answer").Value;
			}
			set
			{
				var data = GetData();
				data.Element("answer").Value = value;
				File.WriteAllText(DataBaseFileName, data.ToString());
			}
		}


		public static bool DataExist()
		{
			return File.Exists(DataBaseFileName);
		}

		public static XElement GetData()
		{
			if (!DataExist())
				return GetNewData();
			return XElement.Load (DataBaseFileName);
		}

		public static XElement GetNewData()
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

