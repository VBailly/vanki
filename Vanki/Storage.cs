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

			

		public static void SetVisited (bool value) 
		{
			var data = GetData ();
			data.Element ("visited").Value = value ? "yes" : "no";

			File.WriteAllText ("db.xml", data.ToString ());
		}

		public static bool HasBeenVisited ()
		{
			var data = GetData ();
			return data.Element ("visited").Value == "yes";
		}

		public static XElement GetData()
		{
			if (!File.Exists ("db.xml"))
				return GetNewData();
			return XElement.Load ("db.xml");
		}

		public static XElement GetNewData()
		{
			var xEl = new XElement ("visited", "no" );

			var time = DateTime.Now;
			var xTime = new XElement ("time", time.ToString ());
			var xLapse = new XElement ("lapse", "2");

			var data = new XElement ("Data");
			data.Add (xEl);
			data.Add (xTime);
			data.Add (xLapse);

			return data;
		}

	}
}

