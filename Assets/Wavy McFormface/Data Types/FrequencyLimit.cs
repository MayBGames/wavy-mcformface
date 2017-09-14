using UnityEngine;

namespace bsgbryan {
  [System.Serializable]
  public class FrequencyLimit : Enableable {

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

    /*
      !!! DO NOT MANIPULATE THIS FIELD DIRECTLY !!!
      This field is used internally by Unity to determine
      whether or not to display the Details section of the
      inspector
     */
    public bool ShowDetails;

  }
}
