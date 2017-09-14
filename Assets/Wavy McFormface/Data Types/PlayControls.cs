using UnityEngine;

namespace bsgbryan {
  [System.Serializable]
  public class PlayControls : ShowableProperties {

    /*
      The lowest octave of audio rendered for a waveform
     */
    public int LowOctave;

    /*
      The highest octave of audio reneder for a waveform
     */
    public int HighOctave;

    /*
      This property is not serialized so that multiple
      GameObjects can share the same waveform.
     */
    [System.NonSerialized]
  	public int CurrentOctave;

    /*
      This property is not serialized so that multiple
      GameObjects can share the same waveform.
     */
    [System.NonSerialized]
    public int NoteIndex;
  }
}
