using UnityEngine;

namespace bsgbryan {
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
