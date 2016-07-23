using System;
using System.Xml.Linq;
using System.IO;

namespace Vanki
{
	public static class Storage
	{

		public static void SetTime (DateTime time)
		{
			var visited = HasBeenVisited ();
			var xEl = new XElement ("visited", visited ? "yes" : "no" );

			var xTime = new XElement ("time", time.ToString ());

			var data = new XElement ("Data");
			data.Add (xEl);
			data.Add (xTime);

			File.WriteAllText ("db.xml", data.ToString ());
		}

		public static DateTime GetTime()
		{
			if (!File.Exists ("db.xml"))
				return DateTime.Now;

			var xdoc = XElement.Load ("db.xml");
			return DateTime.Parse(xdoc.Element ("time").Value);
		}
			

		public static void SetVisited (bool value) 
		{
			var xEl = new XElement ("visited", value ? "yes" : "no" );

			var time = GetTime ();
			var xTime = new XElement ("time", time.ToString ());

			var data = new XElement ("Data");
			data.Add (xEl);
			data.Add (xTime);

			File.WriteAllText ("db.xml", data.ToString ());
		}

		public static bool HasBeenVisited ()
		{
			if (!File.Exists ("db.xml"))
				return false;

			var xdoc = XElement.Load ("db.xml");
			return xdoc.Element ("visited").Value == "yes";
		}

	}
}

