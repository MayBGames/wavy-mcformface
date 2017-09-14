using System.Collections.Generic;
using UnityEngine;

namespace bsgbryan {
  [System.Serializable]
  public class WaveTypes : ShowableProperties {
    public float[] Selected = new float[] { 0f, 0f, 0f, 0f };

    [System.NonSerialized]
    public float[] CurrentSelected = new float[] { 0f, 0f, 0f, 0f };

    [System.NonSerialized]
    public float[] PreviousSelected = new float[] { 0f, 0f, 0f, 0f };
  }
}
