using UnityEngine;

namespace bsgbryan {
  [System.Serializable]
  public class Harmonics : Enableable {

    /*
      Specifies the volume frequencies in the upper
      harmonics (the octaves above the playing one)
      should play at
     */
    public AnimationCurve UpperCurve;

    /*
      Specifies the volume frequencies in the low
      harmonics (the octaves below the playing one)
      should play at
     */
    public AnimationCurve LowerCurve;

    /*
      !!! DO NOT MANIPULATE THIS FIELD DIRECTLY !!!
      This field is used internally by MavyMcFormface
      for Lerping
     */
    [System.NonSerialized]
    public AnimationCurve CurrentUpperCurve = new AnimationCurve();

    /*
      !!! DO NOT MANIPULATE THIS FIELD DIRECTLY !!!
      This field is used internally by MavyMcFormface
      for Lerping
     */
    [System.NonSerialized]
    public AnimationCurve CurrentLowerCurve = new AnimationCurve();

    /*
      !!! DO NOT MANIPULATE THIS FIELD DIRECTLY !!!
      This field is used internally by MavyMcFormface
      for Lerping
     */
    [System.NonSerialized]
    public AnimationCurve PreviousUpperCurve = new AnimationCurve();

    /*
      !!! DO NOT MANIPULATE THIS FIELD DIRECTLY !!!
      This field is used internally by MavyMcFormface
      for Lerping
     */
    [System.NonSerialized]
    public AnimationCurve PreviousLowerCurve = new AnimationCurve();

    /*
      !!! DO NOT MANIPULATE THIS FIELD DIRECTLY !!!
      This field is used internally by MavyMcFormface
      for Lerping
     */
    [System.NonSerialized]
    public AnimationCurve FrozenUpperCurve = new AnimationCurve();

    /*
      !!! DO NOT MANIPULATE THIS FIELD DIRECTLY !!!
      This field is used internally by MavyMcFormface
      for Lerping
     */
    [System.NonSerialized]
    public AnimationCurve FrozenLowerCurve = new AnimationCurve();

    /*
      !!! DO NOT MANIPULATE THIS FIELD DIRECTLY !!!
      This field is used internally by Unity to determine
      whether or not to display the Details section of the
      inspector
     */
    public bool ShowDetails;

  }
}
