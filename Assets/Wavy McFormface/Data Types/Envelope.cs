using UnityEngine;

namespace bsgbryan {
  [System.Serializable]
  public class Envelope : ShowableProperties {

    /*
      Specifies how long a waveform should take to reach
      full volume, and what the path to full volume looks
      like.

      For more info, please see SimpleCurvedTime.cs
     */
    public bsgbryan.SimpleCurvedTime Attack = new bsgbryan.SimpleCurvedTime();

    /*
      Specifies how long a waveform should take to go from
      full volume to silent, and what that path looks like.

      For more info, please see SimpleCurvedTime.cs
     */
    public bsgbryan.SimpleCurvedTime Decay = new bsgbryan.SimpleCurvedTime();
  }
}
