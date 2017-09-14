using UnityEngine;

namespace bsgbryan {
  [System.Serializable]
  public class SimpleCurvedTime {

    /*
      The time in milliseconds a curve should play
      before looping
     */
    public int Time;

    /*
      How a waveform should be modified over time
     */
    public AnimationCurve Curve;

  }
}
