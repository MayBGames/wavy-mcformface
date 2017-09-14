using UnityEngine;

namespace bsgbryan {
  [System.Serializable]
  public class Harmonics : Enableable {

    public AnimationCurve UpperCurve;

    public AnimationCurve LowerCurve;

    [System.NonSerialized]
    public AnimationCurve CurrentUpperCurve = new AnimationCurve();

    [System.NonSerialized]
    public AnimationCurve CurrentLowerCurve = new AnimationCurve();

    [System.NonSerialized]
    public AnimationCurve PreviousUpperCurve = new AnimationCurve();

    [System.NonSerialized]
    public AnimationCurve PreviousLowerCurve = new AnimationCurve();

    [System.NonSerialized]
    public AnimationCurve FrozenUpperCurve = new AnimationCurve();

    [System.NonSerialized]
    public AnimationCurve FrozenLowerCurve = new AnimationCurve();

    public bool ShowDetails;

  }
}
