using System;
using System.Xml.Linq;
using System.IO;

namespace Vanki
{
	public static class Storage
	{
		public static void SetVisited (bool value)
		{
			var xEl = new XElement ("visited", value ? "yes" : "no" );
			File.WriteAllText ("db.xml", xEl.ToString ());
		}

		public static bool HasBeenVisited ()
		{
			if (!File.Exists ("db.xml"))
				return false;

			var xdoc = XElement.Load ("db.xml");
			return xdoc.Value == "yes";
		}

	}
}

