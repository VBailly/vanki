using System;
using System.Xml.Linq;
using System.IO;

namespace Vanki
{
	public static class Storage
	{

		public static void SetTime (DateTime time)
		{
			var data = GetData ();
			data.Element ("time").Value = time.ToString();

			File.WriteAllText ("db.xml", data.ToString ());
		}

		public static DateTime GetTime()
		{
			var data = GetData ();

			return DateTime.Parse(data.Element ("time").Value);
		}

		public static void SetLapse (int minutes)
		{
			var data = GetData ();
			data.Element ("lapse").Value = minutes.ToString();

			File.WriteAllText ("db.xml", data.ToString ());
		}

		public static int GetLapse()
		{
			var data = GetData ();

			return int.Parse(data.Element ("lapse").Value);
		}

		public static XElement GetData()
		{
			if (!File.Exists ("db.xml"))
				return GetNewData();
			return XElement.Load ("db.xml");
		}

		public static XElement GetNewData()
		{
			var time = DateTime.Now;
			var xTime = new XElement ("time", time.ToString ());
			var xLapse = new XElement ("lapse", "0");

			var data = new XElement ("Data");
			data.Add (xTime);
			data.Add (xLapse);

			return data;
		}

	}
}

