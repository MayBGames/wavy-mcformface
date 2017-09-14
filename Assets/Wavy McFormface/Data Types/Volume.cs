using UnityEngine;

namespace bsgbryan {
  [System.Serializable]
  public class Volume {

    public float Level;

    [System.NonSerialized]
    public float CurrentLevel;

    [System.NonSerialized]
    public float PreviousLevel;

  }
}
