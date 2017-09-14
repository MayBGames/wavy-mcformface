using System.Collections.Generic;
using UnityEngine;

namespace bsgbryan {

  /*
    This maintains all properties for our waveform. All properties, except those
    marked [System.NonSerialized] in their type, are serialized to an asset.

    Assets can be shared across instances of WavyMcFormface. Since
    Envelope.CurrentOctave and Envelope.NoteIndex are not serialized, each
    instance of WavyMcFormface sharing an asset may play their own notes.

    For info on each property, please see the property's type in the Data Types
    directory.
   */
  [System.Serializable]
  public class ConfigieMcWaveface : ScriptableObject {
    public bsgbryan.Volume         Volume         = new bsgbryan.Volume();
  	public bsgbryan.PlayControls   PlayControls   = new bsgbryan.PlayControls();
  	public bsgbryan.Noise          Noise          = new bsgbryan.Noise();
  	public bsgbryan.CurvedTime     WibbleWobble   = new bsgbryan.CurvedTime();
  	public bsgbryan.Envelope       Envelope       = new bsgbryan.Envelope();
  	public bsgbryan.Harmonics      Harmonics      = new bsgbryan.Harmonics();
  	public bsgbryan.FrequencyLimit FrequencyLimit = new bsgbryan.FrequencyLimit();
    public bsgbryan.WaveTypes      WaveVolumes    = new bsgbryan.WaveTypes();

    /*
      Because we can't get access to the sample rate from the audio thread,
      we have to manually force a different sample rate for mobile
     */
    #if UNITY_IOS || UNITY_ANDROID || UNITY_TVOS || UNITY_TIZEN
      public int SampleRate = 24000;
    #else
      public int SampleRate = 48000;
    #endif

    [System.NonSerialized]
    public WavyMcFormface Generator;

    /*
      The core notes available.

      Caclulating notes in other octaves is done by calling Wave.DetermineOctave().
     */
    public float[] Notes = new float[12] {
      16.351f, // C
      17.324f, // C# / Db
      18.354f, // D
      19.445f, // D# / Eb
      20.601f, // E
      21.827f, // F
      23.124f, // F# / Gb
      24.499f, // G
      25.956f, // G# / Ab
      27.5f,   // A
      29.135f, // A# / Bb
      30.868f  // B
    };

    /*
      This isn't currently used for anything, but it's how I originally
      built up the base Notes array, and I think it's cool, so I keep
      it around :-)

      Who knows, maybe you'll find some cool way to use it!
     */
    public float[] Offsets = new float[] {
      0f,     // C
      0.973f, // C# / Db
      1.03f,  // D
      1.091f, // D# / Eb
      1.156f, // E
      1.226f, // F
      1.297f, // F# / Gb
      1.375f, // G
      1.457f, // G# / Ab
      1.544f, // A
      1.635f, // A# / Bb
      1.733f  // B
    };

    /*
      !!! DO NOT MANIPULATE THIS FIELD DIRECTLY !!!
      This field is used internally by MavyMcFormface
      to generate waveforms
     */
    [System.NonSerialized]
    public float[] Phases = new float[] {
      0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f
    };
  }

}
