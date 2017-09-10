using System.Collections.Generic;
using UnityEngine;

namespace WaveformComposer {

  [System.Serializable]
  public class WaveformConfig : ScriptableObject {
    public WaveformComposer.Volume         Volume         = new WaveformComposer.Volume();
  	public WaveformComposer.SonicContext   Context        = new WaveformComposer.SonicContext();
  	public WaveformComposer.Noise          Noise          = new WaveformComposer.Noise();
  	public WaveformComposer.CurvedTime     Disruption     = new WaveformComposer.CurvedTime();
  	public WaveformComposer.RiseAndDecay   RiseAndDecay   = new WaveformComposer.RiseAndDecay();
  	public WaveformComposer.Harmonics      Harmonics      = new WaveformComposer.Harmonics();
  	public WaveformComposer.FrequencyLimit FrequencyLimit = new WaveformComposer.FrequencyLimit();
    public WaveformComposer.WaveTypes      Waves          = new WaveformComposer.WaveTypes();

    public float SamplingFrequency = 48f;

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
