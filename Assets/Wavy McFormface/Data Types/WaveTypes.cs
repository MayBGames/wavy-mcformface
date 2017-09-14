using System.Collections.Generic;
using UnityEngine;

namespace bsgbryan {
  [System.Serializable]
  public class WaveTypes : ShowableProperties {

    /*
      Volume levels for all four supported wave types.
     */
    public float[] Selected = new float[] {
      0f, // Sine Wave
      0f, // Square Wave
      0f, // Triangle Wave
      0f  // Sawtooth Wave
    };

    /*
      !!! DO NOT MANIPULATE THIS FIELD DIRECTLY !!!
      This field is used internally by MavyMcFormface
      for Lerping
     */
    [System.NonSerialized]
    public float[] CurrentSelected = new float[] { 0f, 0f, 0f, 0f };

    /*
      !!! DO NOT MANIPULATE THIS FIELD DIRECTLY !!!
      This field is used internally by MavyMcFormface
      for Lerping
     */
    [System.NonSerialized]
    public float[] PreviousSelected = new float[] { 0f, 0f, 0f, 0f };
  }
}
