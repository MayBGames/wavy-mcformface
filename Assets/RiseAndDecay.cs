using UnityEngine;

namespace WaveformComposer {
  [System.Serializable]
  public class RiseAndDecay {

    public WaveformComposer.SimpleCurvedTime Rise = new WaveformComposer.SimpleCurvedTime();

    public WaveformComposer.SimpleCurvedTime Decay = new WaveformComposer.SimpleCurvedTime();
  }
}
