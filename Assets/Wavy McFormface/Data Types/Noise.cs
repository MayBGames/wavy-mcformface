using UnityEngine;

namespace bsgbryan {
  [System.Serializable]
  public class Noise : Enableable {

    /*
      The degree to which this noise effects the waveform.

      The higher this value, the more distorted the waveform
      will be.
     */
    public float Level;

    /*
      The minimum and maximum distance from the Curve a bit
      of randomly generated noise may be.

      A bit of randomly generated noise that does not fall
      within the Min/Max bounds specified here will not be
      included in the output and will not effect the waveform.

      The range can be used to add variety to the noise effecting
      a waveform.
     */
    public bsgbryan.FloatRange Variance = new bsgbryan.FloatRange();

    /*
      This is similar to the WibbleWobble in that it's a repeating
      pattern. It's different from the WibbleWobble in that it's
      length isn't variable (for now. I'd like it to be eventually).

      This curve can be thought of as the spine at the center of the
      Variance range. For noise to be added to the waveform, it's
      randomly generated value must be within:

      Curve + Variance.Min <-> Curve + Variance.Max

      The x axis of the Curve (time) is the iteration in the
      OnAudioFilterRead loop; meaning iteration 512 of the
      OnAudioFilterRead loop would be 0.5 on the x (time) axis
      of this curve.
     */
    public AnimationCurve Curve;

    /*
      !!! DO NOT MANIPULATE THIS FIELD DIRECTLY !!!
      This field is used internally by MavyMcFormface
      for Lerping
     */
    [System.NonSerialized]
    public float CurrentLevel;

    /*
      !!! DO NOT MANIPULATE THIS FIELD DIRECTLY !!!
      This field is used internally by MavyMcFormface
      for Lerping
     */
    [System.NonSerialized]
    public float PreviousLevel;

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
