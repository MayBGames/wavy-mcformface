using UnityEngine;

namespace bsgbryan {
  [System.Serializable]
  public class FrequencyLimit : Enableable {

    public AnimationCurve Curve;

    [System.NonSerialized]
    public AnimationCurve CurrentCurve = new AnimationCurve();

    [System.NonSerialized]
    public AnimationCurve PreviousCurve = new AnimationCurve();

    [System.NonSerialized]
    public AnimationCurve FrozenCurve = new AnimationCurve();

    public bool ShowDetails;

  }
}
