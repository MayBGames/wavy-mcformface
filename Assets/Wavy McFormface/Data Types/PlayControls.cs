using UnityEngine;

namespace bsgbryan {
  [System.Serializable]
  public class PlayControls : ShowableProperties {

    public int LowOctave;
    public int HighOctave;

    [System.NonSerialized]
  	public int CurrentOctave;

    [System.NonSerialized]
    public int NoteIndex;
  }
}
