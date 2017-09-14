using UnityEngine;

namespace bsgbryan {
  [System.Serializable]
  public class Noise : Enableable {

    public float Level;

    public bsgbryan.FloatRange Variance = new bsgbryan.FloatRange();

    public AnimationCurve Curve;

    [System.NonSerialized]
    public AnimationCurve CurrentCurve = new AnimationCurve();

    [System.NonSerialized]
    public AnimationCurve PreviousCurve = new AnimationCurve();

    [System.NonSerialized]
    public AnimationCurve FrozenCurve = new AnimationCurve();

  }
}
