using System.Collections.Generic;
using UnityEngine;

namespace bsgbryan {

  [System.Serializable]
  public class ConfigieMcWaveface : ScriptableObject {
    public bsgbryan.Volume         Volume         = new bsgbryan.Volume();
  	public bsgbryan.PlayControls   PlayControls   = new bsgbryan.PlayControls();
  	public bsgbryan.Noise          Noise          = new bsgbryan.Noise();
  	public bsgbryan.CurvedTime     WibbleWobble   = new bsgbryan.CurvedTime();
  	public bsgbryan.Envelope Envelope = new bsgbryan.Envelope();
  	public bsgbryan.Harmonics      Harmonics      = new bsgbryan.Harmonics();
  	public bsgbryan.FrequencyLimit FrequencyLimit = new bsgbryan.FrequencyLimit();
    public bsgbryan.WaveTypes      WaveVolumes    = new bsgbryan.WaveTypes();

    public int SampleRate = 48000;

    [System.NonSerialized]
    public WavyMcFormface Generator;

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

    public float[] Phases = new float[] {
      0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f
    };
  }

}
