using UnityEngine;

namespace bsgbryan {

  /*
    The fields below are intended purely to make editing a
    curve over time more intuitive. Instead of doing math to
    determine how many milliseconds are in 12 hours, 3 minutes
    34 seconds, or 89 seconds simply fill in the appropriate
    field with what you want.

    Math is hard, let machines do it :-)
   */
	[System.Serializable]
	public class TimeSpan {

    public int Milliseconds;
		public int Seconds;
		public int Minutes;
		public int Hours;

		public int Value {
			get {
				return Milliseconds + (Seconds * 1000) + (Minutes * 60 * 1000) + (Hours * 60 * 60 * 1000);
			}
		}
	}
}
