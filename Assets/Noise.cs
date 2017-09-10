using UnityEngine;

namespace WaveformComposer {
  [System.Serializable]
  public class Noise : Enableable {

    public float Level;

    public WaveformComposer.FloatRange Variance = new WaveformComposer.FloatRange();

    public AnimationCurve Curve;

  }
}
