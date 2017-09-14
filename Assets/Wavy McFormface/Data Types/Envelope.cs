using UnityEngine;

namespace bsgbryan {
  [System.Serializable]
  public class Envelope : ShowableProperties {

    public bsgbryan.SimpleCurvedTime Rise = new bsgbryan.SimpleCurvedTime();

    public bsgbryan.SimpleCurvedTime Decay = new bsgbryan.SimpleCurvedTime();
  }
}
