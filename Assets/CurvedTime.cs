using UnityEngine;

namespace WaveformComposer {
  [System.Serializable]
  public class CurvedTime : Enableable {

    public TimeSpan Time;

    public AnimationCurve Curve;

  }
}
