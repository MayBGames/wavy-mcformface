using UnityEngine;

namespace bsgbryan {
  [System.Serializable]
  public class CurvedTime : Enableable {

    /*
      The amount of time a CurvedTime plays before
      looping (for more info, please see TimeSpan.cs)
     */
    public TimeSpan Time;

    /*
      Describes how the waveform pitch will be altered
      over time
     */
    public AnimationCurve Curve;

    /*
      !!! DO NOT MANIPULATE THIS FIELD DIRECTLY !!!
      This field is used internally by MavyMcFormface
      for Lerping
     */
    [System.NonSerialized]
    public AnimationCurve CurrentCurve = new AnimationCurve();

    /*
      !!! DO NOT MANIPULATE THIS FIELD DIRECTLY !!!
      This field is used internally by MavyMcFormface
      for Lerping
     */
    [System.NonSerialized]
    public AnimationCurve PreviousCurve = new AnimationCurve();

    /*
      !!! DO NOT MANIPULATE THIS FIELD DIRECTLY !!!
      This field is used internally by MavyMcFormface
      for Lerping
     */
    [System.NonSerialized]
    public AnimationCurve FrozenCurve = new AnimationCurve();

  }
}
